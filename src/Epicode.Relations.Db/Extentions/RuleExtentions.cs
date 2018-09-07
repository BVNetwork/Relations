using System.Linq;
using System.Web;
using EPiCode.Relations.Core;

namespace EPiCode.Relations.Db.Extentions
{
    public static class RuleExtentions
    {
        public static bool IsLegalPageType(this Rule rule, string pageTypeName, bool validateLeftSide)
        {
            var pageTypes = HttpUtility.UrlDecode(validateLeftSide ? rule.PageTypeLeft : rule.PageTypeRight);
            
            if(!string.IsNullOrWhiteSpace(pageTypes))
                return pageTypes.Split(';').Any(x => x == pageTypeName);
            
            return false;
        }
    }
}
