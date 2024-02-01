using System.Collections.Generic;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.Filters;
using EPiServer.ServiceLocation;

namespace EPiCode.Relations.Db.PageSearch
{
    public class PageSearchFindPagesWithCriteria : IPageSearch
    {
        public IEnumerable<PageData> SearchRelations(int pageId, string searchKeyword, IEnumerable<string> pageTypeNames, PageReference hierarchyStart)
        {
            var pageTypeCollection = pageTypeNames.ToList();
            var pageTypeCriterias = new PropertyCriteriaCollection();

            if (hierarchyStart == null || hierarchyStart == PageReference.EmptyReference)
                hierarchyStart = ContentReference.StartPage;

            foreach (string s in pageTypeCollection)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    var criteria = new PropertyCriteria
                    {
                        Condition = CompareCondition.Equal,
                        Name = "PageTypeName",
                        Type = PropertyDataType.String,
                        Value = s
                    };
                    pageTypeCriterias.Add(criteria);
                }
            }

            PageDataCollection pages = ServiceLocator.Current.GetInstance<IPageCriteriaQueryService>().FindPagesWithCriteria(hierarchyStart, pageTypeCriterias);

            PageData rootPage = ServiceLocator.Current.GetInstance<IContentLoader>().Get<PageData>(hierarchyStart);
            if (pageTypeCollection.Contains(rootPage.PageTypeName) && !pages.Contains(rootPage))
                pages.Add(rootPage);


            new FilterSort(FilterSortOrder.Alphabetical).Sort(pages);

            var result = new PageDataCollection();

            if (!string.IsNullOrEmpty(searchKeyword))
                foreach (PageData t in pages.Where(t => t.PageName.ToLower().Contains(searchKeyword.ToLower())))
                {
                    result.Add(t);
                }
            else
                result = pages;

            return result;
        }
    }
}