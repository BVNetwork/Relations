using System.Collections.Generic;
using System.Linq;
using EPiServer.Core;
using EPiCode.Relations.Core;
using EPiServer.Filters;
using EPiServer.ServiceLocation;

namespace EPiCode.Relations.Helpers
{
    public class PageHelper
    {
        /// <summary>
        /// Getting related pages through one rule
        /// </summary>
        /// <param name="page">The page to find relations to</param>
        /// <param name="ruleName">The relation rule to search through</param>
        /// <returns>All related pages</returns>
        public static PageDataCollection GetPagesRelated(PageReference page, string ruleName) {
            return GetPagesRelated(page, ruleName, Rule.Direction.Both);
        }

        public static PageDataCollection GetPagesRelated(PageReference page, string ruleName, Rule.Direction direction)
        {
            Rule currentRule = RuleEngine.Instance.GetRule(ruleName);
            
            List<int> relations = new List<int>();
            switch (direction) {
                case Rule.Direction.Both:
                    relations = RelationEngine.Instance.GetRelationPagesForPage(page.ID, currentRule);
                    break;
                case Rule.Direction.Left:
                case Rule.Direction.Right:
                    relations = RelationEngine.Instance.GetRelationPagesForPage(page.ID,currentRule, direction);
                    break;
            }
            FilterSortOrder sortOrder;
            PageDataCollection result = PageIDListToPages(relations);
            sortOrder = (FilterSortOrder)((direction == Rule.Direction.Left)
                ? currentRule.SortOrderLeft
                : currentRule.SortOrderRight);
            new EPiServer.Filters.FilterSort(sortOrder).Sort(result);
            
            return result;
        }

        /// <summary>
        /// Getting related pages through two rules.
        /// </summary>
        /// <param name="page">Page to find relations to</param>
        /// <param name="firstRuleName">The first relation rule to search through</param>
        /// <param name="secondRuleName">The second relation rule to search through</param>
        /// <returns>All related pages</returns>
        public static PageDataCollection GetPagesRelated(PageReference page, string firstRuleName, Rule.Direction firstRuleDirection, string secondRuleName, Rule.Direction secondRuleDirection) {
            List<int> relations = RelationEngine.Instance.GetRelationsForPageTwoHop(page.ID, RuleEngine.Instance.GetRule(firstRuleName), firstRuleDirection, RuleEngine.Instance.GetRule(secondRuleName), secondRuleDirection);
            return PageIDListToPages(relations);
        }

        public static PageDataCollection GetPagesRelated(PageReference page, string firstRuleName, string secondRuleName) {
            Rule.Direction firstDirection = (RuleEngine.Instance.IsLeftRule(page.ID, RuleEngine.Instance.GetRule(firstRuleName))) ? Rule.Direction.Left : Rule.Direction.Right;
            Rule.Direction secondDirection = (RuleEngine.Instance.IsLeftRule(page.ID, RuleEngine.Instance.GetRule(secondRuleName))) ? Rule.Direction.Left : Rule.Direction.Right;
            return GetPagesRelated(page, firstRuleName, firstDirection, secondRuleName, secondDirection);
        }

        /*
        public static PageDataCollection GetRelatedPagesRoundTripHierarchy(Rule rule, Relation relation, int pageID)
        {
            PageReference pr = new PageReference(pageID);
            PageData origPage = DataFactory.Instance.GetPage(pr);
            List<int> relationPages = RelationEngine.GetRelationsForPageRoundTripHierarchy(pageID, rule, relation);
            return PageIDListToPages(relationPages);
        }*/

        /// <summary>
        /// Helper method for page relation getters to convert relations to pages.
        /// </summary>
        /// <param name="pageIDList">Collection of page IDs</param>
        /// <returns>Collection of pages</returns>
        public static PageDataCollection PageIDListToPages(List<int> pageIDList) {
            PageDataCollection pages = new PageDataCollection();
            foreach (int pgid in pageIDList)
            {
                PageData pd = PageEngine.GetPage(pgid);
                if (pd != null && !pages.Contains(pd))
                    pages.Add(pd);
            }
            return pages;
        }


    }
}