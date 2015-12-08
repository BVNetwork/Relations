using System.Collections.Generic;
using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;
using EPiServer.Shell.Navigation;

namespace EPiCode.Relations.Core.MenuProviders
{
    [MenuProvider]
    public class AdminMenuProvider : IMenuProvider
    {
        public IEnumerable<MenuItem> GetMenuItems() {
            var localizationService = ServiceLocator.Current.GetInstance<LocalizationService>();
            if ((System.Web.Security.Roles.IsUserInRole("Administrators") || System.Web.Security.Roles.IsUserInRole("WebAdmins") ||
                 System.Web.Security.Roles.IsUserInRole("RelationAdmins")) == false)
                return new List<MenuItem>();
            return new List<MenuItem> {
                new UrlMenuItem(localizationService.GetString("/relations/admin/relations"), "/global/Relations/","/Modules/EPiCode.Relations/Admin/RelationsAdmin.aspx")
                    };
        }
    }
}