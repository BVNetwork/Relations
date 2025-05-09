﻿using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer;
using EPiServer.Data;
using EPiServer.Data.Dynamic;
using EPiServer.Framework.Cache;
using EPiServer.ServiceLocation;
using Microsoft.Extensions.Configuration;
using Timer = EPiCode.Relations.Diagnostics.Timer;

namespace EPiCode.Relations.Core.RelationProviders.DynamicDataStoreProvider
{
    public class DDSRelationProvider : RelationProviderBase
    {

        private const string DEBUG_CATEGORY = "RelationsCacheBase";
        private const string DISABLE_ALL_CACHING = "EPfCustomCachingDisableAllCache";
        public const string RelationsCacheKey = "RelationsCache.Version";

        public static RelationProviderBase Instance => RelationProviderManager.Provider;

        private static string GetCacheKey(int pageid, string rule, Rule.Direction direction)
        {
            return "Relations-" + pageid + "-" + rule + "-" + direction;
        }

        private static DynamicDataStore RelationDataStore
        {
            get
            {
                return typeof(Relation).GetStore();
            }
        }


        public override void AddRelation(string rule, int pageLeft, int pageRight)
        {
            Relation relation = new Relation();
            relation.PageIDLeft = pageLeft;
            relation.PageIDRight = pageRight;
            relation.RuleName = rule;
            Save(relation);
            UpdateLocalOnly();
            RaiseOnAddedRelation(new RelationEventArgs { CurrentRelation = relation, RelationName = relation.RuleName });
        }

        public override void Save(Relation relation)
        {
            if (RelationExists(relation.RuleName, relation.PageIDLeft, relation.PageIDRight))
            {
                Relation existingRelation = GetRelation(relation.RuleName, relation.PageIDLeft, relation.PageIDRight);
                RelationDataStore.Save(relation, existingRelation.Id);
            }

            RelationDataStore.Save(relation);
        }

        public override List<Relation> GetRelationsForPage(int pageID)
        {
            string cacheKey = GetCacheKey(pageID, "", Rule.Direction.Both);
            List<Relation> relationsForPage = (GetFromCache(cacheKey)) as List<Relation>;
            if (relationsForPage != null)
                return relationsForPage;
            Timer timer = new Timer("Query GetRelatedRelations with page " + pageID);
            var pageTypeQuery = (from relations in RelationDataStore.Items<Relation>()
                                 where (relations.PageIDLeft == pageID || relations.PageIDRight == pageID)
                                 select relations);
            relationsForPage = pageTypeQuery.ToList();
            StoreInCache(cacheKey, relationsForPage);
            timer.Stop();
            return relationsForPage;
        }


        public override List<Relation> GetRelationsForPage(int pageID, Rule rule)
        {
            string cacheKey = GetCacheKey(pageID, rule.RuleName, Rule.Direction.Both);
            List<Relation> relationsForPage = (GetFromCache(cacheKey)) as List<Relation>;
            if (relationsForPage != null)
                return relationsForPage;
            Timer timer = new Timer("Query GetRelatedRelations for rule '" + rule.RuleName + "' with page " + pageID);
            var pageTypeQuery = (from relations in RelationDataStore.Items<Relation>()
                                 where (relations.RuleName == rule.RuleName && (relations.PageIDLeft == pageID || relations.PageIDRight == pageID))
                                 select relations);

            relationsForPage = pageTypeQuery.ToList();
            timer.Stop();
            ValidationFilter(relationsForPage);
            StoreInCache(cacheKey, relationsForPage);
            return relationsForPage;
        }


        public override List<Relation> GetRelationsForPage(int pageID, Rule rule, Rule.Direction direction)
        {
            string cacheKey = GetCacheKey(pageID, rule.RuleName, direction);
            List<Relation> relationsForPage = (GetFromCache(cacheKey)) as List<Relation>;
            if (relationsForPage != null)
                return relationsForPage;
            Timer timer = new Timer("Query GetRelatedRelations for rule '" + rule.RuleName + "' with page " + pageID);
            switch (direction)
            {
                case Rule.Direction.Both:
                    return GetRelationsForPage(pageID, rule);
                case Rule.Direction.Left:
                    var pageTypeQueryLeft = (from relations in RelationDataStore.Items<Relation>()
                                         where (relations.RuleName == rule.RuleName && (relations.PageIDLeft == pageID))
                                         select relations);
                    relationsForPage = pageTypeQueryLeft.ToList();
                    break;
                case Rule.Direction.Right:
                    var pageTypeQueryRight = (from relations in RelationDataStore.Items<Relation>()
                                         where (relations.RuleName == rule.RuleName && (relations.PageIDRight == pageID))
                                         select relations);
                    relationsForPage = pageTypeQueryRight.ToList();
                    break;
            }

            timer.Stop();
            ValidationFilter(relationsForPage);
            StoreInCache(cacheKey, relationsForPage);
            return relationsForPage;
        }

        public override List<int> GetRelationPagesForPage(int pageID, Rule rule)
        {
            string cacheKey = GetCacheKey(pageID, rule.RuleName + "Int", Rule.Direction.Both);
            List<int> relationsForPageInt = (GetFromCache(cacheKey)) as List<int>;
            if (relationsForPageInt != null)
                return relationsForPageInt;

            List<Relation> relationsForPage = Instance.GetRelationsForPage(pageID, rule);
            List<int> result = new List<int>();
            ValidationFilter(relationsForPage);
            foreach (Relation relation in relationsForPage)
                result.Add(relation.PageIDLeft == pageID ? relation.PageIDRight : relation.PageIDLeft);
            StoreInCache(cacheKey, result);
            return result;
        }

        public override List<int> GetRelationPagesForPage(int pageID, Rule rule, Rule.Direction direction)
        {
            string cacheKey = GetCacheKey(pageID, rule.RuleName + "Int", direction);
            List<int> relationsForPageInt = (GetFromCache(cacheKey)) as List<int>;
            if (relationsForPageInt != null)
                return relationsForPageInt;

            List<Relation> relationsForPage = new List<Relation>();
            List<int> result = new List<int>();
            relationsForPage = GetRelationsForPage(pageID, rule, direction);
            foreach (Relation relation in relationsForPage)
                result.Add((direction == Rule.Direction.Left) ? relation.PageIDRight : relation.PageIDLeft);
            StoreInCache(cacheKey, result);
            return result;
        }

        public override List<int> GetRelationsForPageTwoHop(int pageID, Rule firstRule, Rule.Direction firstDirection, Rule secondRule, Rule.Direction secondDirection)
        {
            List<int> primaryRelations = Instance.GetRelationPagesForPage(pageID, firstRule, firstDirection);
            List<int> secondaryRelations = new List<int>();
            foreach (int secondary in primaryRelations)
                secondaryRelations.AddRange(Instance.GetRelationPagesForPage(secondary, secondRule, secondDirection));
            return secondaryRelations.ToList();
        }

        public override Relation GetRelation(string rule, int pageLeft, int pageRight)
        {
            Timer timer = new Timer("Starting query GetRelation for rule '" + rule + "' with pages " + pageLeft.ToString() + " and " + pageRight.ToString());
            var pageTypeQuery = (from relations in RelationDataStore.Items<Relation>()
                                 where (relations.RuleName == rule && (relations.PageIDLeft == pageLeft && relations.PageIDRight == pageRight) || (relations.PageIDLeft == pageRight && relations.PageIDRight == pageLeft))
                                 select relations);
            List<Relation> existingRelations = pageTypeQuery.ToList();
            timer.Stop();
            if (existingRelations != null && existingRelations.Count > 0)
                return existingRelations.First();
            else
                return new Relation();
        }

        public override Relation GetRelation(Identity id)
        {
            var relation = from r in RelationDataStore.Items<Relation>()
                           where r.Id == id
                           select r;
            return relation.First();
        }


        public override void DeleteRelation(Relation relationToDelete)
        {
            RelationDataStore.Delete(relationToDelete.Id);
            UpdateCache();
            RaiseOnDeletedRelation(new RelationEventArgs { CurrentRelation = relationToDelete, RelationName = relationToDelete.RuleName });
        }

        public override bool RelationExists(string rule, int pageLeft, int pageRight)
        {
            var pageTypeQuery = (from relations in RelationDataStore.Items<Relation>()
                                 where (relations.RuleName == rule && ((relations.PageIDLeft == pageLeft && relations.PageIDRight == pageRight) || (relations.PageIDLeft == pageRight && relations.PageIDRight == pageLeft)))
                                 select relations);
            List<Relation> existingRelations = pageTypeQuery.ToList();
            return (existingRelations.Count > 0);
        }

        public override int GetRelationsCount(string rule)
        {
            return RelationDataStore.Items<Relation>().Where(p => p.RuleName == rule).Select(p => new { p.RuleName }).Count();
        }

        public override List<Relation> GetAllRelations(string rule)
        {
            Timer timer = new Timer("Starting query GetAllRelation for rule '" + rule + "'");
            var relationQuery = (from relations in RelationDataStore.Items<Relation>()
                                 where (relations.RuleName == rule)
                                 select relations);
            List<Relation> result = relationQuery.ToList();
            timer.Stop();
            return result;
        }

        public override void DeleteAll()
        {
            RelationDataStore.DeleteAll();
            UpdateCache();
        }

        public override void DeleteAll(string rule)
        {
            List<Relation> relations = GetAllRelations(rule);
            foreach (Relation rel in relations)
                RelationDataStore.Delete(rel.Id);
            UpdateCache();
        }

        private static void ValidationFilter(List<Relation> relations)
        {
            for (int i = 0; i < relations.Count; i++)
                if (Validator.Validate(relations[i].RuleName, relations[i].PageIDLeft, relations[i].PageIDRight) != Validator.ValidationResult.AlreadyExists)
                    relations.RemoveAt(i);
        }

        private static void StoreInCache(string cacheKey, object relations)
        {
            // Check if caching is disabled - can be for debugging or troubleshooting
            // Assumes caching is on unless it has been specified in the web.config file.
            bool disabled = ServiceLocator.Current.GetInstance<IConfiguration>().GetValue<bool>(DISABLE_ALL_CACHING);
            if(disabled) return;
            
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
            System.Diagnostics.Debug.Write("Storing: Relations in cache: '" + cacheKey + "'", DEBUG_CATEGORY);
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

            System.Diagnostics.Debug.Write("Attempt Get from cacheKey: " + cacheKey + "'", DEBUG_CATEGORY);

            if (ServiceLocator.Current.GetInstance<IObjectInstanceCache>().Get(cacheKey) != null)
            {
                // There are pages in the cache
                relations = ServiceLocator.Current.GetInstance<IObjectInstanceCache>().Get(cacheKey);
                System.Diagnostics.Debug.Write("Found relations in cache for: '" + cacheKey + "'", DEBUG_CATEGORY);
            }
            else
            {
                System.Diagnostics.Debug.Write("No relations found in cache for: '" + cacheKey + "'", DEBUG_CATEGORY);
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
    }
}