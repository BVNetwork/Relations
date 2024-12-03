using EPiServer.Core;
using EPiServer;
using EPiServer.ServiceLocation;

namespace EPiCode.Relations.Core
{
    public class PageEngine
    {
        public static PageDataCollection GetDescendents(int page, Rule rule, Relation relation)
        {
            var pr = new PageReference(GetRelatedPageID(relation, page));
            var referencedPage = ServiceLocator.Current.GetInstance<IContentLoader>().Get<PageData>(pr);
            return RuleEngine.Instance.SearchRelations(rule, referencedPage.PageLink.ID, string.Empty, pr);
        }

        private static int GetRelatedPageID(Relation relation, int page)
        {
            return (relation.PageIDLeft == page) ? relation.PageIDRight : relation.PageIDLeft;
        }

        public static PageData GetPage(int page)
        {
            var pageRef = new ContentReference(page);
            if (pageRef != ContentReference.EmptyReference)
            {
                if (ServiceLocator.Current.GetInstance<IContentRepository>().TryGet(pageRef, out PageData pageData))
                {
                    return pageData;
                }
            }
            return null;
        }
    }
}