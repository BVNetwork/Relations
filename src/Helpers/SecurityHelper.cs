using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiCode.Relations.Helpers
{
    public class SecurityHelper
    {
        public static bool IsRelationsAdmin() {
            return (EPiServer.Security.PrincipalInfo.CurrentPrincipal.IsInRole("Administrators") || EPiServer.Security.PrincipalInfo.CurrentPrincipal.IsInRole("RelationAdmins") || EPiServer.Security.PrincipalInfo.CurrentPrincipal.IsInRole("CmsAdmins"));
        }
    }
}