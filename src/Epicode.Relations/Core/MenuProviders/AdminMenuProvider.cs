using System;
using System.Collections.Generic;
using EPiServer.Framework.Localization;
using EPiServer.Shell.Navigation;
using EPiServer.Shell;

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


        public IEnumerable<MenuItem> GetMenuItems()
        {
            
            var url = Paths.ToResource(GetType(), "admin");

            var link = new UrlMenuItem("Relations", MenuPaths.Global + "/relations", url)
            {
                SortIndex = 100,
                AuthorizationPolicy = Constants.PolicyName
            };

            return new List<MenuItem>
            {
                link,
                new UrlMenuItem("Overview", MenuPaths.Global + "/relations/overview",
                    Paths.ToResource(GetType(), "admin"))
                {
                    SortIndex = 200,
                    AuthorizationPolicy = Constants.PolicyName
                },
                new UrlMenuItem("Rule editor", MenuPaths.Global + "/relations/edit",
                    Paths.ToResource(GetType(), "admin/edit"))
                {
                    SortIndex = 300,
                    AuthorizationPolicy = Constants.PolicyName
                },
                new UrlMenuItem("Validator", MenuPaths.Global + "/relations/validator/",
                    Paths.ToResource(GetType(), "admin/relationvalidator"))
                {
                    SortIndex = 400,
                    AuthorizationPolicy = Constants.PolicyName
                },
                new UrlMenuItem(string.Empty, MenuPaths.Global + "/relations/validator/validate",
                    Paths.ToResource(GetType(), "admin/validate"))
                {
                    SortIndex = 405,
                    IsAvailable = _ => false
                },
                new UrlMenuItem("Import and export", MenuPaths.Global + "/relations/query",
                    Paths.ToResource(GetType(), "admin/importexport"))
                {
                    SortIndex = 500,
                    AuthorizationPolicy = Constants.PolicyName
                },
                new UrlMenuItem(string.Empty, MenuPaths.Global + "/relations/query/import",
                    Paths.ToResource(GetType(), "admin/importorexport"))
                {
                    SortIndex = 505,
                    IsAvailable = _ => false
                },
                new UrlMenuItem("Settings", MenuPaths.Global + "/relations/settings",
                    Paths.ToResource(GetType(), "admin/settings"))
                {
                    SortIndex = 600,
                    AuthorizationPolicy = Constants.PolicyName
                }
            };
        }
    }
}