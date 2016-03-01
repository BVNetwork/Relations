using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Caching;
using EPiServer;
using EPiServer.Core;
using EPiServer.Data;
using EPiServer.Data.Dynamic;

namespace EPiCode.Relations.Core.RelationProviders.DynamicDataStoreProvider
{
    public class DDSRuleProvider : RuleProviderBase
    {

        public static RuleProviderBase Instance
        {
            get
            {
                return RuleProviderManager.Provider;
            }
        }

        private const string DEBUG_CATEGORY = "RelationsCacheBase";
        private const string DISABLE_ALL_CACHING = "EPfCustomCachingDisableAllCache";
        public const string RelationsCacheKey = "RelationsCache.Version";

        public class RuleEventArgs
        {
            public string RuleName;
            public Rule CurrentRule;
            public bool CancelEvent;
        }

        public delegate void RuleEventHandler(object sender, RuleEventArgs e);

        public static event RuleEventHandler OnDeletedRule;
        public static event RuleEventHandler OnDeletingRule;
        public static event RuleEventHandler OnSavingRule;
        public static event RuleEventHandler OnSavedRule;
        public static event RuleEventHandler OnCreatingRule;
        public static event RuleEventHandler OnCreatedRule;


        private DynamicDataStore RuleDataStore
        {
            get
            {
                return typeof(Rule).GetStore();
            }
        }

        public override IEnumerable<Rule> GetAllRules()
        {
            return RuleDataStore.Items<Rule>();
        }

        public override List<Rule> GetAllRulesList()
        {
            return RuleDataStore.Items<Rule>().ToList<Rule>();
        }

        public override Identity Save(Rule rel)
        {
            Identity id;
            RuleEventArgs e = new RuleEventArgs();
            e.RuleName = rel.RuleName;
            e.CurrentRule = rel;
            if (OnSavingRule != null)
                OnSavingRule(null, e);
            if (e.CurrentRule != null && e.CurrentRule.Id != null && !e.CancelEvent && RuleExists(rel.Id))
            {
                Rule existingRule = GetRule(rel.Id);
                id = RuleDataStore.Save(rel, existingRule.Id);
                if (OnSavedRule != null)
                    OnSavedRule(null, e);
                UpdateCache();
                return id;
            }
            UpdateCache();
            throw new Exception("Rule not found.");
        }

        public override IEnumerable<Rule> GetRules(int pageID)
        {
            PageData page = PageEngine.GetPage(pageID);
            if (page != null)
            {
                string pageTypeName = HttpUtility.UrlEncode(page.PageTypeName);
                var pageTypeQuery = (from rules in RuleDataStore.Items<Rule>()
                                     where rules.PageTypeLeft.ToLower().Contains(pageTypeName.ToLower())
                                     || rules.PageTypeRight.ToLower().Contains(pageTypeName.ToLower())
                                     select rules);
                List<Rule> allRules = pageTypeQuery.ToList();
                List<Rule> relevantRules = new List<Rule>();
                foreach (Rule rule in allRules)
                {
                    int startPage = (RuleEngine.Instance.IsLeftRule(pageID, rule) ? rule.RelationHierarchyStartLeft : rule.RelationHierarchyStartRight);
                    if (startPage < 1 || IsDescendent(pageID, startPage))
                        relevantRules.Add(rule);
                }
                return relevantRules;
            }
            return new List<Rule>();

        }

        public override IEnumerable<Rule> GetRules(string pageTypeName)
        {
            string encodedPageTypeName = HttpUtility.UrlEncode(pageTypeName);
            var pageTypeQuery = (from rules in RuleDataStore.Items<Rule>()
                                 where rules.PageTypeLeft.ToLower().Contains(encodedPageTypeName.ToLower())
                                    || rules.PageTypeRight.ToLower().Contains(encodedPageTypeName.ToLower())
                                 select rules);
            List<Rule> allRules = pageTypeQuery.ToList();

            return allRules;
        }

        public override IEnumerable<Rule> GetRulesLeft(int pageID)
        {
            PageData page = PageEngine.GetPage(pageID);
            if (page != null)
            {
                string pageTypeName = HttpUtility.UrlEncode(page.PageTypeName);
                var pageTypeQuery = (from rules in RuleDataStore.Items<Rule>()
                                     where rules.PageTypeLeft.ToLower().Contains(pageTypeName.ToLower())
                                     select rules);
                List<Rule> allRules = pageTypeQuery.ToList();
                List<Rule> relevantRules = new List<Rule>();
                foreach (Rule rule in allRules)
                {
                    int startPage = (rule.RelationHierarchyStartLeft);
                    if (startPage < 1 || IsDescendent(pageID, startPage))
                    {
                        rule.RuleDirection = Rule.Direction.Left;
                        relevantRules.Add(rule);
                    }
                }
                return relevantRules;
            }
            return new List<Rule>();
        }

        public override IEnumerable<Rule> GetRulesRight(int pageID)
        {
            PageData page = PageEngine.GetPage(pageID);
            if (page != null)
            {
                string pageTypeName = HttpUtility.UrlEncode(page.PageTypeName);
                var pageTypeQuery = (from rules in RuleDataStore.Items<Rule>()
                                     where rules.PageTypeRight.ToLower().Contains(pageTypeName.ToLower())
                                     select rules);
                List<Rule> allRules = pageTypeQuery.ToList();
                List<Rule> relevantRules = new List<Rule>();
                foreach (Rule rule in allRules)
                {
                    int startPage = (rule.RelationHierarchyStartRight);
                    if (startPage < 1 || IsDescendent(pageID, startPage))
                    {
                        rule.RuleDirection = Rule.Direction.Right;
                        relevantRules.Add(rule);
                    }
                }
                return relevantRules;
            }
            return new List<Rule>();
        }

        public override Rule AddNewRule(string name, string pageTypeLeft, string pageTypeRight, string ruleTextLeft, string ruleTextRight)
        {
            if (!RuleExists(name))
            {
                RuleEventArgs e = new RuleEventArgs();
                e.RuleName = name;
                e.CurrentRule = null;
                if (OnCreatingRule != null)
                    OnCreatingRule(null, e);
                if (!e.CancelEvent)
                {
                    Rule newRule = new Rule(name, pageTypeLeft, pageTypeRight);
                    newRule.RuleTextLeft = ruleTextLeft;
                    newRule.RuleTextRight = ruleTextRight;
                    RuleDataStore.Save(newRule);
                    e.CurrentRule = newRule;
                    if (OnCreatedRule != null)
                        OnCreatedRule(null, e);
                    UpdateCache();
                    return newRule;
                }
                else
                    return null;
            }
            return null;
        }

        public override bool RuleExists(string name)
        {
            if (RuleDataStore.Find<Rule>("RuleName", name).Count() > 0)
                return RuleDataStore.Find<Rule>("RuleName", name).First() != null;
            return false;
        }

        public override bool RuleExists(Identity id)
        {
            var rules = from r in RuleDataStore.Items<Rule>()
                        where r.Id == id
                        select r;
            return (rules.Count<Rule>() > 0);
        }


        public override Rule GetRule(string name)
        {
            string cacheKey = "Relations-Rulenamestored-" + name;
            Rule rule = (GetFromCache(cacheKey)) as Rule;
            if (rule != null)
                return rule;

            var ruleList = RuleDataStore.Find<Rule>("RuleName", name);
            if (ruleList.Count() > 0)
            {
                rule = ruleList.First() as Rule;
                StoreInCache(cacheKey, rule);
                return rule;
            }
            return new Rule();
        }

        public override Rule GetRule(Identity id)
        {
            var rules = from r in RuleDataStore.Items<Rule>()
                        where r.Id == id
                        select r;
            if (rules.Count() > 0)
                return rules.First<Rule>();
            Rule newRule = AddNewRule("New rule", "", "", "", "");
            return newRule;
        }


        public override void DeleteRule(string name)
        {
            RuleEventArgs e = new RuleEventArgs();
            e.RuleName = name;
            e.CurrentRule = GetRule(name);
            if (e.CurrentRule != null && e.CurrentRule.Id != null)
            {
                if (OnDeletingRule != null)
                    OnDeletingRule(null, e);
                if (!e.CancelEvent)
                {
                    RuleDataStore.Delete(e.CurrentRule);
                }
                if (OnDeletedRule != null)
                    OnDeletedRule(null, e);
            }
            UpdateCache();
        }

        public override void DeleteAll()
        {
            RuleDataStore.DeleteAll();
            UpdateCache();
        }

        public override bool IsLeftRule(string pageTypeName, Rule rule)
        {
            string pageTypeLeft = HttpUtility.UrlDecode(rule.PageTypeLeft);
            return pageTypeLeft.Contains(pageTypeName);

        }

        public override bool IsLeftRule(int pageID, Rule rule)
        {
            PageReference pageRef = new PageReference(pageID);
            PageData pageData = DataFactory.Instance.GetPage(pageRef);
            string pageTypeLeft = HttpUtility.UrlDecode(rule.PageTypeLeft);
            string pageTypeRight = HttpUtility.UrlDecode(rule.PageTypeRight);
            if (pageTypeLeft.Contains(pageData.PageTypeName))
            {
                if (pageTypeRight.Contains(pageData.PageTypeName))
                {
                    if (rule.RelationHierarchyStartRight > 0)
                        if (IsDescendent(pageRef.ID, rule.RelationHierarchyStartRight))
                            return false;
                }
                return true;
            }
            return false;
        }

        public override bool IsDescendent(int pageID, int startID)
        {
            if (startID == PageReference.StartPage.ID || startID == PageReference.RootPage.ID)
                return true;
            PageReference page = new PageReference(pageID);
            PageReference rootPage = new PageReference(startID);
            while (page.ID != PageReference.RootPage.ID)
            {
                if (page.ID == rootPage.ID)
                    return true;
                page = DataFactory.Instance.GetPage(page).ParentLink;
            }
            return false;
        }

        public override PageDataCollection SearchRelations(Rule rule, int pageID, string searchKeyWord, PageReference hierarchyStart)
        {
            return SearchRelations(rule, pageID, searchKeyWord, hierarchyStart, RuleEngine.Instance.IsLeftRule(pageID, rule));
        }

        public override PageDataCollection SearchRelations(Rule rule, int pageID, string searchKeyWord, PageReference hierarchyStart, bool isLeftRule)
        {
            string pageTypes = HttpUtility.UrlDecode(isLeftRule ? rule.PageTypeRight : rule.PageTypeLeft);
            PageDataCollection result = new PageDataCollection();

            if (!string.IsNullOrEmpty(pageTypes))
            {
                string[] pageTypeCollection = pageTypes.Split(';');
                PropertyCriteriaCollection pageTypeCriterias = new PropertyCriteriaCollection();

                if (hierarchyStart == null || hierarchyStart == PageReference.EmptyReference)
                    hierarchyStart = PageReference.StartPage;

                foreach (string s in pageTypeCollection)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        PropertyCriteria criteria = new PropertyCriteria();
                        criteria.Condition = EPiServer.Filters.CompareCondition.Equal;
                        criteria.Name = "PageTypeName";
                        criteria.Type = PropertyDataType.String;
                        criteria.Value = s;
                        pageTypeCriterias.Add(criteria);
                    }
                }

                PageDataCollection pages = DataFactory.Instance.FindPagesWithCriteria(hierarchyStart, pageTypeCriterias);

                PageData rootPage = DataFactory.Instance.GetPage(hierarchyStart);
                if (pageTypeCollection.Contains<string>(rootPage.PageTypeName) && !pages.Contains(rootPage))
                    pages.Add(rootPage);


                new EPiServer.Filters.FilterSort(EPiServer.Filters.FilterSortOrder.Alphabetical).Sort(pages);

             
                if (!string.IsNullOrEmpty(searchKeyWord))
                    for (int i = 0; i < pages.Count; i++)
                    {
                        if (pages[i].PageName.ToLower().Contains(searchKeyWord.ToLower()))
                            result.Add(pages[i]);
                    }
                else
                    result = pages;
            }

            return result;

        }

        public override PageDataCollection SearchRelations(Rule rule, int pageID, string searchKeyWord, bool isLeftRule)
        {
            PageReference hierarchyStart = new PageReference((isLeftRule) ? rule.RelationHierarchyStartRight : rule.RelationHierarchyStartLeft);
            if (hierarchyStart == null || hierarchyStart == PageReference.EmptyReference)
                hierarchyStart = PageReference.StartPage;
            return SearchRelations(rule, pageID, searchKeyWord, hierarchyStart, isLeftRule);
        }

		private static void StoreInCache(string cacheKey, object relations)
		{
			// Check if caching is disabled - can be for debugging or troubleshooting
            // Assumes caching is on unless it has been specified in the web.config file.
            string disabledSetting = ConfigurationManager.AppSettings[DISABLE_ALL_CACHING];
            bool disabled = false;
            if (bool.TryParse(disabledSetting, out disabled))
            {
                if (disabled) return;
            }

			// Make the cache dependent on the EPiServer cache, so we'll remove this
			// when new pages are published, pages are deleted or we are notified by
			// another server that the cache needs refreshing
			string[] pageCacheDependencyKey = new String[1];
            pageCacheDependencyKey[0] = RelationsCacheKey;
			CacheDependency dependency = new CacheDependency(null, pageCacheDependencyKey);

			// Add to cache, with dependencies but no expiration policies
			// If the cached item should be cached for a limited time (regardless of
			// the cache dependency), add an absolute expiration date or a
			// sliding expiration to the item.
			// Also note, we use the Insert method that will overwrite any existing
			// cache item with the same key. The Add method will throw an exception
			// if an item with the same key exists.
			System.Diagnostics.Debug.Write("Storing: Relations in cache: '" + cacheKey + "'", DEBUG_CATEGORY);
			HttpContext.Current.Cache.Insert(cacheKey, relations, dependency);
		}

		/// <summary>
		/// Will get the pages from the cache, if it exists.
		/// </summary>
		/// <returns>A PageDataCollection with pages, or null if not in cache</returns>
		private static object GetFromCache(string cacheKey)
		{
			object relations = null;
			// Call inheriting class' implementation

			System.Diagnostics.Debug.Write("Attempt Get from cacheKey: " + cacheKey + "'", DEBUG_CATEGORY);

			if (HttpContext.Current.Cache[cacheKey] != null)
			{
				// There are pages in the cache
				relations = HttpContext.Current.Cache[cacheKey];
				System.Diagnostics.Debug.Write("Found relations in cache for: '" + cacheKey + "'", DEBUG_CATEGORY);
			}
			else
			{
                System.Diagnostics.Debug.Write("No relations found in cache for: '" + cacheKey + "'", DEBUG_CATEGORY);
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
            HttpRuntime.Cache.Insert(RelationsCacheKey, value, null, 
                Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, 
                CacheItemPriority.NotRemovable, new CacheItemRemovedCallback(RemovedCallback));
        }

        internal static void UpdateCache() {
            UpdateLocalOnly();
            UpdateRemoteOnly();
        }

        internal static void UpdateLocalOnly()
        {
            
            HttpRuntime.Cache.Insert(RelationsCacheKey, DateTime.Now.Ticks, null, 
                DateTime.MaxValue, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, 
                new CacheItemRemovedCallback(RemovedCallback));
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
            if (CacheManager.RuntimeCacheGet(RelationsCacheKey) == null)
            {
                SetKey();
            }
        }

 


    }
}