using EPiServer.Core;

namespace EPiCode.Relations.Extensions
{
    public static partial class PageReferenceRelationExtension
    {
        public static PageDataCollection GetRelatedPages(this PageData pageData, string relationRuleName) {
            return Helpers.PageHelper.GetPagesRelated(pageData.PageLink, relationRuleName);
        }

        public static PageDataCollection GetRelatedPages(this PageData pageData, string firstRuleName, string secondRuleName)
        {
            return Helpers.PageHelper.GetPagesRelated(pageData.PageLink, firstRuleName, secondRuleName);
        }

    }
}