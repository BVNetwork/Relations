using System;
using System.Linq;
using EPiCode.Relations.Core.RelationProviders;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;

namespace EPiCode.Relations.Core.InitializationModules
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class ProviderInitialization : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            RelationProviderManager.Initialize();
            RuleProviderManager.Initialize();
        }

        public void Preload(string[] parameters) { }

        public void Uninitialize(InitializationEngine context)
        {
            //Add uninitialization logic
        }
    }
}