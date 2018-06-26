using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using EPiCode.Relations.Core;
using EPiServer;
using EPiServer.Core;
using EPiServer.Filters;
using EPiServer.ServiceLocation;

namespace EPiCode.Relations.Helpers
{
    public class FacetsHelper
    {
        /// <summary>
        /// Returns a PageDataCollection with a distinct list of pages related through the given rule. Number of relations are added to PageName.
        /// </summary>
        /// <param name="allPages">PageDataCollection to create facets from</param>
        /// <param name="ruleName">Rule used for facet relations</param>
        /// <param name="direction">Direction of facet relations</param>
        /// <returns></returns>
        public static PageDataCollection FacetList(PageDataCollection allPages, string ruleName,
            Rule.Direction direction)
        {
            List<Relation> relations = RelationEngine.Instance.GetAllRelations(ruleName);

            List<int> facets;
            if (direction == Rule.Direction.Right)
            {
                facets = (from id in relations select id.PageIDRight).Distinct().ToList();
            }
            else
            {
                facets = (from id in relations select id.PageIDLeft).Distinct().ToList();
            }

            PageDataCollection facetPages = PageHelper.PageIDListToPages(facets);
            PageDataCollection facetsWithCount = new PageDataCollection();

            foreach (PageData facetPage in facetPages)
            {
                PageData writableFacet = facetPage.CreateWritableClone();
                if (direction == Rule.Direction.Right)
                {
                    writableFacet.PageName = writableFacet.PageName + " (" +
                                          (from id in relations where id.PageIDRight == facetPage.PageLink.ID select id)
                                              .Count() + ")";
                }
                else
                {
                    writableFacet.PageName = writableFacet.PageName + " (" +
                                         (from id in relations where id.PageIDLeft == facetPage.PageLink.ID select id)
                                             .Count() + ")";
                }
                facetsWithCount.Add(writableFacet);
            }
            new EPiServer.Filters.FilterSort(FilterSortOrder.Alphabetical).Sort(facetsWithCount);
            return facetsWithCount;
        }

        /// <summary>
        /// Returns a list of pages filtered for a given facet
        /// </summary>
        /// <param name="allPages">All pages</param>
        /// <param name="ruleName">Rule for filtered relations</param>
        /// <param name="direction">Direction of rule for filtered relations</param>
        /// <param name="filterPageID">The facet to filter for</param>
        /// <returns></returns>
        public static PageDataCollection FacetFilter(string ruleName, int filterPageID)
        {
            PageData filterPage =
                ServiceLocator.Current.GetInstance<IContentRepository>().Get<PageData>(new PageReference(filterPageID));
            return PageHelper.GetPagesRelated(new PageReference(filterPageID), ruleName, Rule.Direction.Both);

            /*
            if (direction == Rule.Direction.Left)
                return (from page in allPages
                        where
                            (from relation in RelationEngine.Instance.GetAllRelations(ruleName) where relation.PageIDRight == page.ContentLink.ID select relation.PageIDLeft)
                                .Contains(filterPageID)
                        select page).ToList();
            else
            {
                return (from page in allPages
                        where
                            (from relation in RelationEngine.Instance.GetAllRelations(ruleName) where relation.PageIDLeft == page.ContentLink.ID select relation.PageIDRight)
                                .Contains(filterPageID)
                        select page).ToList();

            }*/
        }
    }

    public class ConfigurationHelper
    {
        /// <summary>
        /// Gets the value of an appSettings key as an bool. If not defined
        /// or not parsable, the defaultValue parameter will be returned.
        /// </summary>
        /// <param name="key">The key in the appSettings section whose value to return.</param>
        /// <param name="defaultValue">The default value to returned if the setting is null or not parsable.</param>
        /// <returns>The appSettings value if parsable, defaultValue if not</returns>
        public static bool GetAppSettingsConfigValueBool(string key, bool defaultValue)
        {
            string stringValue = WebConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(stringValue))
                return defaultValue;

            bool retValue;
            bool parsed = false;
            parsed = bool.TryParse(stringValue, out retValue);
            if (parsed)
                return retValue;

            // Could not parse, return default value
            return defaultValue;
        }
    }
}