using System.Collections.Generic;
using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;
using EPiServer.Shell.Navigation;
using EPiCode.Relations.Helpers;

namespace EPiCode.Relations.Core.MenuProviders
{
    [MenuProvider]
    public class AdminMenuProvider : IMenuProvider
    {
        private readonly LocalizationService _localizationService;

        public AdminMenuProvider(LocalizationService localizationService)
        {
            _localizationService = localizationService;
        }


        public IEnumerable<MenuItem> GetMenuItems() {
                              
            if (SecurityHelper.IsRelationsAdmin() == false)
            {
                return new List<MenuItem>();
            }

            return new List<MenuItem>
            {
                new UrlMenuItem(
                    _localizationService.GetString("/relations/admin/relations"), 
                    "/global/Relations/",
                    "/Modules/EPiCode.Relations/Admin/RelationsAdmin.aspx")
            };
        }
    }
}