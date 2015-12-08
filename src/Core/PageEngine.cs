using EPiServer.Core;
using EPiServer;


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
            PageReference pr = new PageReference(page);
            try {
                if (pr != null && pr != PageReference.EmptyReference)
                    return DataFactory.Instance.GetPage(pr);
                return null;
            }
            catch (PageNotFoundException pageNotFound) {
                return null;
            }
        }
    }
}