using System.Collections.Generic;
using EPiServer.Core;

namespace EPiCode.Relations.Db.PageSearch
{
    public class PageSearchDummy : IPageSearch
    {
        public IEnumerable<PageData> SearchRelations(int pageId, string searchKeyword, IEnumerable<string> pageTypeNames, PageReference hierarchyStart)
        {            
            return new List<PageData>();
        }
    }
}