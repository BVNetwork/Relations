using EPiCode.Relations.Core;
using EPiCode.Relations.Db.Data;
using EPiCode.Relations.Db.Extentions;
using EPiCode.Relations.Db.Mapping;
using EPiCode.Relations.Diagnostics;
using EPiServer;
using EPiServer.Core;
using EPiServer.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Caching;
using Castle.Core.Internal;
using Rule = EPiCode.Relations.Core.Rule;


namespace EPiCode.Relations.Db
{
    public class RuleDbProvider : Core.RelationProviders.RuleProviderBase 
    {
        private readonly EntityMapperTranslator<Rule, Data.Rule> _ruleEntityMapperTranslator = new RuleTranslator();

        private RelationsContext GetContext()
        {            
            return RelationsContext.CreateContext();
        }

        public override Rule AddNewRule(string name, string pageTypeLeft, string pageTypeRight, string ruleTextLeft, string ruleTextRight)
        {
            if (RuleExists(name))
                return GetRule(name);
            
            var newRule = new Rule(name, pageTypeLeft, pageTypeRight)
                    {
                        RuleTextLeft = ruleTextLeft,
                        RuleTextRight = ruleTextRight,
                    };
            
            Save(newRule);

            return newRule;        
        }

        public override Identity Save(Rule rule)
        {
            var newRule = _ruleEntityMapperTranslator.Translate<Data.Rule>(rule);

            using (var ctx = GetContext())
            {
                var existingRule = ctx.Rules.Include(s=>s.PageTypeList).FirstOrDefault(x => x.RuleId == rule.Id.ExternalId);
               
                if (existingRule != null)
                {
                    if (existingRule.PageTypeList == null)
                        existingRule.PageTypeList = new List<RulePageType>();

                    var existingName = existingRule.RuleName;

                    ctx.Entry(existingRule).CurrentValues.SetValues(newRule);

                    //Identify items to remove
                    var remove = existingRule.PageTypeList.Where(x => !newRule.PageTypeList.Any(r => r.PageTypeName == x.PageTypeName && r.Direction == x.Direction)).ToList();

                    //Remove them
                    foreach (var rulePageType in remove)
                    {
                        existingRule.PageTypeList.Remove(rulePageType);
                        ctx.RulePageType.Remove(rulePageType);
                    }
                   
                    //Add new items
                    existingRule.PageTypeList.AddRange(newRule.PageTypeList.Where(x=> !existingRule.PageTypeList.Any(r=>r.PageTypeName==x.PageTypeName && r.Direction == x.Direction)));

                    if (existingName != rule.RuleName)
                    {
                        // Update relations
                        ctx.Relations.Where(x => x.RuleName == existingName)
                                     .ForEach(x => x.RuleName = rule.RuleName);
                    }
                }
                else
                {
                    ctx.Rules.Add(newRule);
                }
                ctx.SaveChanges();
                UpdateCache();
            }
            return Identity.NewIdentity(newRule.RuleId);
        }

    
        public override void DeleteAll()
        {
            using (var ctx = GetContext())
            {                
                ctx.Rules.Include(x => x.PageTypeList)
                         .ToList()
                         .ForEach(x => ctx.Rules.Remove(x));

                ctx.SaveChanges();
                UpdateCache();
            }
        }

        public override void DeleteRule(string name)
        {
            using (var ctx = GetContext())
            {
                ctx.Rules
                    .Include(x => x.PageTypeList)                    
                    .Where(o => o.RuleName == name)
                    .ToList()
                    .ForEach(x => ctx.Rules.Remove(x));

                ctx.SaveChanges();
                UpdateCache();
            }
        }


        public override IEnumerable<Rule> GetAllRules()
        {
            using (var ctx = GetContext())
            {
                return ctx.Rules
                          .Include(x => x.PageTypeList)   
                          .AsEnumerable()
                          .Select(o => _ruleEntityMapperTranslator.Translate<Rule>(o))
                          .ToList();
            }
        }

        public override List<Rule> GetAllRulesList()
        {
            return GetAllRules().ToList();
        }

        public override Rule GetRule(Identity id)
        {
            using (var ctx = GetContext())
            {
                return ctx.Rules
                          .Include(x => x.PageTypeList)
                          .Where(r => r.RuleId == id.ExternalId)
                          .AsEnumerable()
                          .Select(o => _ruleEntityMapperTranslator.Translate<Rule>(o))
                          .FirstOrDefault() ?? new Rule {RuleName = "New rule", Id = id};
            }
        }

        public override Rule GetRule(string name)
        {
            string cacheKey = string.Format("Relations.Db-Rulenamestored-{0}", name);
           
            var rule = (GetFromCache(cacheKey)) as Rule;

            if (rule != null)
                return rule;

            using (var ctx = GetContext())
            {
                rule = ctx.Rules
                          .Include(x => x.PageTypeList)
                          .Where(o => o.RuleName == name)
                          .AsEnumerable()
                          .Select(o => _ruleEntityMapperTranslator.Translate<Rule>(o))
                          .FirstOrDefault();
            }

            if (rule != null)
            {
                StoreInCache(cacheKey, rule);
                return rule;
            }

            return new Rule();
        }

        public override IEnumerable<Rule> GetRules(int pageId)
        {
            PageData page = PageEngine.GetPage(pageId);

            if (page == null)
                return new List<Rule>();

            return GetRules(page.PageTypeName);
        }

        public override IEnumerable<Rule> GetRules(string pageTypeName)
        {

            var timer = new Timer(string.Format("GetRules pageTypeName={0}", pageTypeName));

            List<Rule> allRules;

            using (var ctx = GetContext())
            {
                allRules = ctx.Rules
                              .Include(x => x.PageTypeList)
                              .Where(rule => rule.PageTypeList.Any(p => p.PageTypeName.ToLower() == pageTypeName.ToLower()))
                              .AsEnumerable()
                              .Select(o => _ruleEntityMapperTranslator.Translate<Rule>(o))
                              .ToList();
            }

            timer.Stop();

            foreach (var rule in allRules)
            {
                rule.RuleDirection = GetSpecificRuleDirection(pageTypeName, rule);
            }

            return allRules;
            
        }

        private Rule.Direction GetSpecificRuleDirection(string pagetypeName, Rule rule)
        {
            if (IsLeftAndRightRule(pagetypeName, rule))
                return Rule.Direction.Both;
            if (IsRightRule(pagetypeName, rule))
                return Rule.Direction.Right;

            return Rule.Direction.Left;
        }

        private bool IsRightRule(string pageTypeName, Rule rule)
        {
            var mappedRule = _ruleEntityMapperTranslator.Translate<Data.Rule>(rule);
            var direction = Rule.Direction.Right.ToString();

            return mappedRule.PageTypeList.Any(p => p.PageTypeName.ToLower() == pageTypeName.ToLower() &&
                                                    p.Direction == direction); 
        }

        private bool IsLeftAndRightRule(string pageTypeName, Rule rule)
        {
            var mappedRule = _ruleEntityMapperTranslator.Translate<Data.Rule>(rule);
            var direction = Rule.Direction.Both.ToString();

            return mappedRule.PageTypeList.Any(p => p.PageTypeName.ToLower() == pageTypeName.ToLower() &&
                                                    p.Direction == direction);
        }

        public override bool IsDescendent(int pageId, int startId)
        {
            var page = new PageReference(pageId);
            var rootPage = new PageReference(startId);
            while (page.ID != ContentReference.RootPage.ID)
            {
                if (page.ID == rootPage.ID)
                    return true;

                page = DataFactory.Instance.GetPage(page).ParentLink;
            }
            return false;
        }

        public override bool IsLeftRule(int pageId, Rule rule)
        {
            var page = PageEngine.GetPage(pageId);

            if (page == null)
                return false;

            return IsLeftRule(page.PageTypeName, rule);
        }

        public override bool IsLeftRule(string pageTypeName, Rule rule)
        {
            var mappedRule = _ruleEntityMapperTranslator.Translate<Data.Rule>(rule);
            var direction = Rule.Direction.Left.ToString();

            return mappedRule.PageTypeList.Any(p => p.PageTypeName.ToLower() == pageTypeName.ToLower() &&
                                                    p.Direction == direction);            
        }

        public override bool RuleExists(Identity id)
        {
            using (var ctx = GetContext())
            {
                return ctx.Rules
                          .Any(r => r.RuleId == id);
            }
        }

        public override bool RuleExists(string name)
        {
            using (var ctx = GetContext())
            {
                return ctx.Rules
                          .Any(r => r.RuleName.ToLower() == name.ToLower());
            }
        }


        public override IEnumerable<Rule> GetRulesLeft(int pageId)
        {
            return GetRulesByDirection(pageId, Rule.Direction.Left);
        }

        public override IEnumerable<Rule> GetRulesRight(int pageId)
        {
            return GetRulesByDirection(pageId, Rule.Direction.Right);            
        }

        private IEnumerable<Rule> GetRulesByDirection(int pageId, Rule.Direction direction)
        {
            List<Rule> allRules;

            PageData page = PageEngine.GetPage(pageId);
            if (page == null)
                return new List<Rule>();

            var strDirection = direction.ToString();
            var pageTypeName = page.PageTypeName;

            var timer = new Timer(string.Format("GetRulesByDirection pageTypeName={0}({1}) direction={2}", pageTypeName, pageId, strDirection));

            using (var ctx = GetContext())
            {
                allRules = ctx.Rules
                              .Include(x => x.PageTypeList)
                              .Where(rules => rules.PageTypeList.Any(p => p.PageTypeName.ToLower() == pageTypeName.ToLower() &&
                                                                          p.Direction == strDirection))
                              .AsEnumerable()
                              .Select(o => _ruleEntityMapperTranslator.Translate<Rule>(o))
                              .ToList();
            }
            timer.Stop();

            var relevantRules = new List<Rule>();

            if (direction == Rule.Direction.Both)
                return allRules;

            foreach (var rule in allRules)
            {
                int startPage = (direction == Rule.Direction.Right) ? rule.RelationHierarchyStartRight : rule.RelationHierarchyStartLeft;
                if (startPage < 1 || IsDescendent(pageId, startPage))
                {
                    rule.RuleDirection = direction;
                    relevantRules.Add(rule);
                }
            }
            return relevantRules;
        }

        public override PageDataCollection SearchRelations(Rule rule, int pageId, string searchKeyWord, PageReference hierarchyStart)
        {
            return SearchRelations(rule, pageId, searchKeyWord, hierarchyStart, RuleEngine.Instance.IsLeftRule(pageId, rule));
        }

        public override PageDataCollection SearchRelations(Rule rule, int pageId, string searchKeyWord, bool isLeftRule)
        {
            int hierarchyStartPageId = (isLeftRule) ? rule.RelationHierarchyStartRight : rule.RelationHierarchyStartLeft;
            var hierarchyStart = new PageReference(hierarchyStartPageId);
            if (hierarchyStart == ContentReference.EmptyReference)
                hierarchyStart = ContentReference.StartPage;
            return SearchRelations(rule, pageId, searchKeyWord, hierarchyStart, isLeftRule);
        }

    
        public override PageDataCollection SearchRelations(Rule rule, int pageId, string searchKeyword, PageReference hierarchyStart, bool isLeftRule)
       
        {
            var timer = new Timer(string.Format("SearchRelations pageId={0}, rule name={1}", pageId, rule.RuleName));

            var mappedRule = _ruleEntityMapperTranslator.Translate<Data.Rule>(rule);
            var pageTypenames = isLeftRule ? mappedRule.PageTypeList.Where(o => o.Direction == Rule.Direction.Right.ToString())
                                                                    .Select(p => p.PageTypeName)
                                           : mappedRule.PageTypeList.Where(o => o.Direction == Rule.Direction.Left.ToString())
                                                                    .Select(p => p.PageTypeName);

            

            var pages = new List<PageData>();
            int searchPageId;
            if (int.TryParse(searchKeyword.Trim(), out searchPageId))
            {
                // Try to get page
                PageData pageData;              
                if (DataFactory.Instance.TryGet(new ContentReference(searchPageId), out pageData) &&
                    rule.IsLegalPageType(pageData.PageTypeName, !isLeftRule))
                {                   
                    pages.Add(pageData);
                }
            }

            var timer2 = new Timer(string.Format("SearchRelations (do search) pageId={0}, rule name={1}", pageId, rule.RuleName));
            pages.AddRange(RelationsConfiguration.Instance.PageSearch.SearchRelations(pageId, searchKeyword, pageTypenames, hierarchyStart));
            timer2.Stop();
            timer.Stop();
            return new PageDataCollection(pages);
        }


        #region Cache

        private const string DebugCategory = "RelationsCacheBase";
        private const string DisableAllCaching = "EPfCustomCachingDisableAllCache";
        public const string RelationsCacheKey = "RelationsCache.Version";

        private static void StoreInCache(string cacheKey, object relations)
        {
            // Check if caching is disabled - can be for debugging or troubleshooting
            // Assumes caching is on unless it has been specified in the web.config file.
            string disabledSetting = ConfigurationManager.AppSettings[DisableAllCaching];
            bool disabled;
            if (bool.TryParse(disabledSetting, out disabled))
            {
                if (disabled) return;
            }

            // Make the cache dependent on the EPiServer cache, so we'll remove this
            // when new pages are published, pages are deleted or we are notified by
            // another server that the cache needs refreshing
            var pageCacheDependencyKey = new String[1];
            pageCacheDependencyKey[0] = RelationsCacheKey;
            var dependency = new CacheDependency(null, pageCacheDependencyKey);

            // Add to cache, with dependencies but no expiration policies
            // If the cached item should be cached for a limited time (regardless of
            // the cache dependency), add an absolute expiration date or a
            // sliding expiration to the item.
            // Also note, we use the Insert method that will overwrite any existing
            // cache item with the same key. The Add method will throw an exception
            // if an item with the same key exists.
            HttpRuntime.Cache.Insert(cacheKey, relations, dependency);
        }

        /// <summary>
        /// Will get the pages from the cache, if it exists.
        /// </summary>
        /// <returns>A PageDataCollection with pages, or null if not in cache</returns>
        private static object GetFromCache(string cacheKey)
        {
            object relations = null;

            // Call inheriting class' implementation
            if (HttpRuntime.Cache[cacheKey] != null)
            {
                // There are pages in the cache
                relations = HttpRuntime.Cache[cacheKey];
            }
            else
            {
                System.Diagnostics.Debug.Write("No relations found in cache for: '" + cacheKey + "'", DebugCategory);
            }

            return relations;
        }

        private static void SetKey()
        {
            System.Diagnostics.Debug.Write("Setting relations cache key.");
            SetKey(DateTime.UtcNow.Ticks);
        }

        private static void SetKey(long value)
        {
            HttpRuntime.Cache.Insert(RelationsCacheKey, value, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, RemovedCallback);
        }

        internal static void UpdateCache()
        {
            UpdateLocalOnly();
            UpdateRemoteOnly();
        }

        internal static void UpdateLocalOnly()
        {
            HttpRuntime.Cache.Insert(RelationsCacheKey, DateTime.Now.Ticks, null, DateTime.MaxValue, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, RemovedCallback);
        }

        internal static void UpdateRemoteOnly()
        {
            CacheManager.RemoveRemoteOnly(RelationsCacheKey);
        }

        private static void RemovedCallback(string key, object value, CacheItemRemovedReason reason)
        {
            EnsureKey();
        }

        internal static void EnsureKey()
        {
            if (CacheManager.Get(RelationsCacheKey) == null)
            {
                SetKey();
            }
        }

        #endregion
    }
}
