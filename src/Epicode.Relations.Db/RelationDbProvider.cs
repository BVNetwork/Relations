using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using EPiCode.Relations.Core;
using EPiCode.Relations.Core.RelationProviders;
using EPiCode.Relations.Db.Extentions;
using EPiCode.Relations.Db.Mapping;
using EPiCode.Relations.Diagnostics;
using EPiServer;
using EPiServer.Data;
using EPiServer.Framework.Cache;
using EPiServer.ServiceLocation;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace EPiCode.Relations.Db
{
    public class RelationDbProvider : RelationProviderBase
    {
        private readonly EntityMapperTranslator<Relation, Data.Relation> _entityMapperTranslator = new RelationTranslator();

        private static RelationsContext GetContext()
        {
            return RelationsContext.CreateContext();
        }

        public override void AddRelation(string rule, int pageLeft, int pageRight)
        {           
            var relation = new Relation {PageIDLeft = pageLeft, PageIDRight = pageRight, RuleName = rule};

            var newRelation = _entityMapperTranslator.Translate<Data.Relation>(relation);

            using (var ctx = GetContext())
            {
                ctx.Relations.Add(newRelation);
                ctx.SaveChanges();

                var e = new RelationEventArgs { CurrentRelation = _entityMapperTranslator.Translate<Relation>(newRelation) };
                UpdateCache();
                RaiseOnAddedRelation(e);                
            }
        }

        public override void Save(Relation relation)
        {
            throw new NotImplementedException();
        }

        public override Relation GetRelation(Identity id)
        {
            using (var ctx = GetContext())
            {
                return ctx.Relations
                          .Where(rel => rel.RelationId == id)
                          .Translate(_entityMapperTranslator)
                          .FirstOrDefault();
            }
        }

        public override Relation GetRelation(string rule, int pageLeft, int pageRight)
        {
            using (var ctx = GetContext())
            {
                return ctx.Relations
                          .Where(rel => rel.RuleName == rule &&
                                        ((rel.PageIdLeft == pageLeft && rel.PageIdRight == pageRight) ||
                                       (rel.PageIdLeft == pageRight && rel.PageIdRight == pageLeft)))
                          .AsEnumerable()
                          .Translate(_entityMapperTranslator)
                          .FirstOrDefault();
            }
        }

        public override void DeleteRelation(Relation relation)
        {
            using (var ctx = GetContext())
            {
                var relationToDelete = ctx.Relations.FirstOrDefault(o => o.RelationId == relation.Id.ExternalId);
                ctx.Relations.Remove(relationToDelete);
                ctx.SaveChanges();

                var e = new RelationEventArgs { CurrentRelation = _entityMapperTranslator.Translate<Relation>(relationToDelete) };
                
                UpdateCache();
                RaiseOnDeletedRelation(e);
            }
        }

        public override bool RelationExists(string rule, int pageLeft, int pageRight)
        {
            using (var ctx = GetContext())
            {
                return ctx.Relations
                    .Any(rel => rel.RuleName.ToLower() == rule.ToLower() &&
                                ((rel.PageIdLeft == pageLeft && rel.PageIdRight == pageRight) ||
                                 (rel.PageIdLeft == pageRight && rel.PageIdRight == pageLeft)));
            }
        }

        public override int GetRelationsCount(string rule)
        {
            using (var ctx = GetContext())
            {
                return ctx.Relations
                    .Count(p => p.RuleName == rule);
            }
        }


        public override List<Relation> GetRelationsForPage(int pageId, Rule rule)
        {
            var timer = new Timer(string.Format("GetRelationsForPage (*) pageId={0}, rule name={1}", pageId, rule.RuleName));

            var relations = GetRelationsForPage(pageId).Where(rel => rel.RuleName == rule.RuleName && (rel.PageIDLeft == pageId || rel.PageIDRight == pageId))
                                                        .ToList();
            timer.Stop();

            return relations;
        }

        public override List<Relation> GetRelationsForPage(int pageId, Rule rule, Rule.Direction direction)
        {
            var relations = new List<Relation>();

            var timer = new Timer(string.Format("GetRelationsForPage (*) pageId={0}, rule name={1}, direction={2}", pageId, rule.RuleName, direction));
            switch (direction)
            {
                case Rule.Direction.Both:
                    relations = GetRelationsForPage(pageId, rule);
                    break;
                case Rule.Direction.Left:
                    relations = GetRelationsForPage(pageId).Where(rel => rel.RuleName == rule.RuleName && rel.PageIDLeft == pageId)
                                                           .ToList();
                    break;
                case Rule.Direction.Right:
                    relations = GetRelationsForPage(pageId).Where(rel => rel.RuleName == rule.RuleName && rel.PageIDRight == pageId)
                                                           .ToList();
                    break;
            }
            timer.Stop();

            return relations;
        }


        public override List<Relation> GetRelationsForPage(int pageId)
        {
            var cacheKey = GetCacheKey(pageId, "", Rule.Direction.Both);
            var relations = GetFromCache(cacheKey) as List<Relation>;

            if (relations != null)
                return relations;


            var timer = new Timer(string.Format("GetRelationsForPage pageId={0}", pageId));

            using (var ctx = GetContext())
            {
                relations = ctx.Relations
                               .Where(rel => rel.PageIdLeft == pageId || rel.PageIdRight == pageId)
                               .AsEnumerable()
                               .Translate(_entityMapperTranslator)
                               .ToList();
            }

            timer.Stop();

            StoreInCache(cacheKey, relations);

            return relations;
        }


        public override List<int> GetRelationPagesForPage(int pageId, Rule rule)
        {
            return GetRelationsForPage(pageId, rule).Select(x => x.PageIDLeft == pageId ? x.PageIDRight : x.PageIDLeft).ToList();
        }

        public override List<int> GetRelationPagesForPage(int pageId, Rule rule, Rule.Direction direction)
        {
            return GetRelationsForPage(pageId, rule, direction).Select(x => direction == Rule.Direction.Left ? x.PageIDRight : x.PageIDLeft).ToList();
        }

        public override List<int> GetRelationsForPageTwoHop(int pageId, Rule firstRule, Rule.Direction firstDirection, Rule secondRule, Rule.Direction secondDirection)
        {
            var primaryRelations = GetRelationPagesForPage(pageId, firstRule, firstDirection);
            var secondaryRelations = new List<int>();
            foreach (var secondary in primaryRelations)
            {
                secondaryRelations.AddRange(GetRelationPagesForPage(secondary, secondRule, secondDirection));
            }
            return secondaryRelations.ToList();
        }



        public override List<Relation> GetAllRelations(string rule)
        {
            using (var ctx = GetContext())
            {

                return ctx.Relations
                          .Where(rel => rel.RuleName == rule)
                          .AsEnumerable()
                          .Translate(_entityMapperTranslator)
                          .ToList();
            }
        }

        public override void DeleteAll()
        {
            throw new NotImplementedException();
        }

        public override void DeleteAll(string rule)
        {
            throw new NotImplementedException();
        }


        #region Caching
        private const string DebugCategory = "RelationsCacheBase";
        private const string DisableAllCaching = "EPfCustomCachingDisableAllCache";
        public const string RelationsCacheKey = "RelationsCache.Version";

        private static string GetCacheKey(int pageid, string rule, Rule.Direction direction)
        {
            return string.Format("Relations-{0}-{1}-{2}", pageid, rule, direction);
        }

        private static void StoreInCache(string cacheKey, object relations)
        {
            // Check if caching is disabled - can be for debugging or troubleshooting
            // Assumes caching is on unless it has been specified in the web.config file.
            bool disabled = ServiceLocator.Current.GetInstance<IConfiguration>().GetValue<bool>(DisableAllCaching);
            if (disabled) return;


            // Make the cache dependent on the EPiServer cache, so we'll remove this
            // when new pages are published, pages are deleted or we are notified by
            // another server that the cache needs refreshing
            string[] pageCacheDependencyKey = new String[1];
            pageCacheDependencyKey[0] = RelationsCacheKey;
            var cacheEvictionPolicy = new CacheEvictionPolicy(null, pageCacheDependencyKey);

            // Add to cache, with dependencies but no expiration policies
            // If the cached item should be cached for a limited time (regardless of
            // the cache dependency), add an absolute expiration date or a
            // sliding expiration to the item.
            // Also note, we use the Insert method that will overwrite any existing
            // cache item with the same key. The Add method will throw an exception
            // if an item with the same key exists.
            System.Diagnostics.Debug.Write("Storing: Relations in cache: '" + cacheKey + "'", DebugCategory);
            ServiceLocator.Current.GetInstance<IObjectInstanceCache>().Insert(cacheKey, relations, cacheEvictionPolicy);
        }

        /// <summary>
        /// Will get the pages from the cache, if it exists.
        /// </summary>
        /// <returns>A PageDataCollection with pages, or null if not in cache</returns>
        private static object GetFromCache(string cacheKey)
        {
            object relations = null;
            // Call inheriting class' implementation

            System.Diagnostics.Debug.Write("Attempt Get from cacheKey: " + cacheKey + "'", DebugCategory);

            if (ServiceLocator.Current.GetInstance<IObjectInstanceCache>().Get(cacheKey) != null)
            {
                // There are pages in the cache
                relations = ServiceLocator.Current.GetInstance<IObjectInstanceCache>().Get(cacheKey);
                System.Diagnostics.Debug.Write("Found relations in cache for: '" + cacheKey + "'", DebugCategory);
            }
            else
            {
                System.Diagnostics.Debug.Write("No relations found in cache for: '" + cacheKey + "'", DebugCategory);
            }

            return relations;
        }

        internal static void UpdateCache()
        {
            UpdateLocalOnly();
            UpdateRemoteOnly();
        }

        internal static void UpdateLocalOnly()
        {
            CacheManager.RemoveLocalOnly(RelationsCacheKey);
        }

        internal static void UpdateRemoteOnly()
        {
            CacheManager.RemoveRemoteOnly(RelationsCacheKey);
        }

        #endregion
    }
}
