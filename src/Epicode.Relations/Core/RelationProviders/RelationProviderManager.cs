using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Linq;
using EPiCode.Relations.Helpers;

namespace EPiCode.Relations.Core.RelationProviders
{
    public class RelationProviderManager
    {
        private static RelationProviderBase _defaultProvider;

        public static void Initialize()
        {
            const string fallbackProviderSetting = "EPiCode.Relations.Core.RelationProviders.DynamicDataStoreProvider.DDSRelationProvider";
            string defaultProviderSetting = Settings.GetSettingValue("DefaultRelationProviderString");
            if (string.IsNullOrEmpty(defaultProviderSetting))
                defaultProviderSetting = fallbackProviderSetting;

            _defaultProvider = GetRelationProvider(defaultProviderSetting) as RelationProviderBase ??
                GetRelationProvider(fallbackProviderSetting) as RelationProviderBase;           
        }


        public static Type[] GetRelationProviders()
        {
            var assemblyStartsWith = ConfigurationHelper.GetAppSettingsList("Relations.RelationProviderAssembliesStartsWith");

            var type = typeof(RelationProviderBase);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .Where(ass => ass.IsDynamic == false)
                .Where(ass => !assemblyStartsWith.Any() || assemblyStartsWith.Any(x => ass.FullName.StartsWith(x)))
                .SelectMany(ass => ass.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p != type);
            return types.ToArray<Type>();

        }

        public static RelationProviderBase Provider { 
            get {
                if (_defaultProvider == null)
                    Initialize();
                return _defaultProvider;
            }
        } 

        private static ProviderBase GetRelationProvider(string providerType)
        {
            try
            {
                RelationProviderBase base2 = null;
                if (string.IsNullOrEmpty(providerType))
                {
                    throw new ArgumentException("Provider type is invalid");
                }
                Type c = Type.GetType(providerType, true, true);
                if (!typeof (RelationProviderBase).IsAssignableFrom(c))
                {
                    throw new ArgumentException(String.Format("Provider must implement type {0}.",
                        typeof (RelationProviderBase).ToString()));
                }
                base2 = (RelationProviderBase) Activator.CreateInstance(c);
                var config = new NameValueCollection();
                config.Add("name", providerType);
                base2.ProviderName = providerType;

                base2.Initialize(providerType, config);
                return base2;
            }
            catch (Exception e)
            {
                throw new Exception("Error loading RelationProvider.", e);
            }

        }

    }
}