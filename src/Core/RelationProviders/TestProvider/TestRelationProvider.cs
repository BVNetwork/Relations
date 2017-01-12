using System;
using System.Collections.Generic;
using EPiServer;
using EPiServer.Core;
using EPiServer.Data;

namespace EPiCode.Relations.Core.RelationProviders.TestProvider
{
    public class TestRelationProvider : RelationProviderBase
    {
        private const string DEBUG_CATEGORY = "RelationsCacheBase";
        private const string DISABLE_ALL_CACHING = "EPfCustomCachingDisableAllCache";
        public const string RelationsCacheKey = "RelationsCache.Version";


        public override void AddRelation(string rule, int pageLeft, int pageRight)
        {
        }

        public override void Save(Relation relation)
        {
        }

        private List<Relation> getFakeList(int pageID, Rule rule)
        {
            List<Relation> relations = new List<Relation>();
            Random rnd = new Random();
            int j = rnd.Next(2, 15);

            for (int i = 0; i < j; i++)
                relations.Add(new Relation { RuleName = rule.RuleName, PageIDLeft = pageID, PageIDRight = GetRandomPageID()});
            return relations;
        }

        private List<int> getFakeIntList()
        {
            Random rnd = new Random();
            int j = rnd.Next(2, 15);

            List<int> relations = new List<int>();
            for(int i = 0; i < j; i++)
                relations.Add(GetRandomPageID());
            return relations;
        }

        public override List<Relation> GetRelationsForPage(int pageID)
        {
            return getFakeList(pageID, new Rule());
        }


        public override List<Relation> GetRelationsForPage(int pageID, Rule rule)
        {
            return getFakeList(pageID, rule);
        }

        public override List<Relation> GetRelationsForPage(int pageID, Rule rule, Rule.Direction dir)
        {
            return getFakeList(pageID, rule);
        }

        public override List<int> GetRelationPagesForPage(int pageID, Rule rule)
        {
            return getFakeIntList();
        }

        public override List<int> GetRelationPagesForPage(int pageID, Rule rule, Rule.Direction direction)
        {
            return getFakeIntList();
        }

        private Relation relation {
            get {
                Relation rel = new Relation();
                rel.RuleName = "Random relation";
                rel.PageIDLeft = GetRandomPageID();
                rel.PageIDRight = GetRandomPageID();
                return rel;
            }
        }

        private int GetRandomPageID() {
            Random rnd = new Random();

            PageDataCollection pdc = AllPages;
            return pdc[rnd.Next(0, pdc.Count - 1)].ContentLink.ID;
        }


        private static PageDataCollection _allPages;

        public static PageDataCollection AllPages { 
            get {
                if (_allPages == null)
                    _allPages = GetAllPages(DataFactory.Instance.GetPage(PageReference.StartPage), new PageDataCollection());
                return _allPages;
            }
        }

        private static PageDataCollection GetAllPages(PageData parent, PageDataCollection allPages) {
            PageDataCollection children = DataFactory.Instance.GetChildren(parent.ContentLink.ToPageReference());
            foreach (PageData pd in children) {
                allPages.Add(pd);
                GetAllPages(pd, allPages);
            }
            return allPages;


        }

        public override List<int> GetRelationsForPageTwoHop(int pageID, Rule firstRule, Rule.Direction firstDirection, Rule secondRule, Rule.Direction secondDirection)
        {
            return getFakeIntList();
        }

        public override Relation GetRelation(string rule, int pageLeft, int pageRight)
        {
            return relation;
        }

        public override Relation GetRelation(Identity id)
        {
            return relation;
        }


        public override void DeleteRelation(Relation relationToDelete)
        {
        }

        public override bool RelationExists(string rule, int pageLeft, int pageRight)
        {
            return false;
        }

        public override int GetRelationsCount(string rule)
        {
            return 1;
        }

        public override List<Relation> GetAllRelations(string rule)
        {
            return getFakeList(PageReference.StartPage.ID, RuleEngine.Instance.GetRule(rule));

        }

        public override void DeleteAll()
        {
        }

        public override void DeleteAll(string rule)
        {
        }



    }
}