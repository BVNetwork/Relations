using System.Collections.Generic;
using EPiServer.Core;

namespace EPiCode.Relations.Db.PageSearch
{
    public interface IPageSearch
    {
        IEnumerable<PageData> SearchRelations(int pageId, string searchKeyWord, IEnumerable<string> pageTypeNames, PageReference hierarchyStart);
    }
}
