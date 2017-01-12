using EPiServer.Core;
using EPiServer;
using EPiServer.ServiceLocation;

namespace EPiCode.Relations.Core
{
    public class PageEngine
    {
        public static PageDataCollection GetDescendents(int page, Rule rule, Relation relation)
        {
            PageReference pr = new PageReference(GetRelatedPageID(relation, page));
            PageData referencedPage = DataFactory.Instance.GetPage(pr);
            return RuleEngine.Instance.SearchRelations(rule, referencedPage.PageLink.ID, string.Empty, pr);
        }

        private static int GetRelatedPageID(Relation relation, int page)
        {
            return (relation.PageIDLeft == page) ? relation.PageIDRight : relation.PageIDLeft;
        }

        public static PageData GetPage(int page)
        {
            ContentReference pageRef = new ContentReference(page);
            if (pageRef != null && pageRef != ContentReference.EmptyReference)
            {
                PageData pageData = ServiceLocator.Current.GetInstance<IContentRepository>().Get<IContent>(pageRef) as PageData;
                return pageData;
            }
            return null;
        }
    }
}