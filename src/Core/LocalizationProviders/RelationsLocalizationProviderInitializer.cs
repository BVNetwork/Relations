using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Framework.Localization;
using EPiServer.Framework.Localization.XmlResources;

namespace EPiCode.Relations.Core.LocalizationProviders
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class RelationsLocalizationProviderInitializer : IInitializableModule
    {
        private const string ProviderName = "RelationsProvider";

        public void Initialize(InitializationEngine context)
        {

            ProviderBasedLocalizationService localizationService = context.Locate.Advanced.GetInstance<LocalizationService>() as ProviderBasedLocalizationService;
            if (localizationService != null)
            {
                string langFolder = System.Web.HttpContext.Current.Server.MapPath(@"~/Modules/EPiCode.Relations/Resources/LanguageFiles/");
                if (Directory.Exists(langFolder))
                {
                    NameValueCollection configValues = new NameValueCollection();
                    configValues.Add(FileXmlLocalizationProvider.PhysicalPathKey, langFolder);
                    FileXmlLocalizationProvider localizationProvider = new FileXmlLocalizationProvider();
                    localizationProvider.Initialize(ProviderName, configValues);
                    localizationService.Providers.Add(localizationProvider);
                }
            }

        }

        public void Uninitialize(InitializationEngine context)
        {
            ProviderBasedLocalizationService localizationService = context.Locate.Advanced.GetInstance<LocalizationService>() as ProviderBasedLocalizationService;
            if (localizationService != null)
            {
                LocalizationProvider localizationProvider = localizationService.Providers.FirstOrDefault(p => p.Name.Equals(ProviderName, StringComparison.Ordinal));
                if (localizationProvider != null)
                {
                    localizationService.Providers.Remove(localizationProvider);
                }
            }
        }

        public void Preload(string[] parameters)
        {
        }
    }
}