using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Linq;
using EPiCode.Relations.Helpers;

namespace EPiCode.Relations.Core.RelationProviders
{
    public class RuleProviderManager
    {
        private static RuleProviderBase defaultProvider;

        public static void Initialize() {
            const string fallbackProviderSetting = "EPiCode.Relations.Core.RelationProviders.DynamicDataStoreProvider.DDSRuleProvider";
            string defaultProviderSetting = Settings.GetSettingValue("DefaultRuleProviderString");
            if (string.IsNullOrEmpty(defaultProviderSetting))
                defaultProviderSetting = fallbackProviderSetting;

            defaultProvider = GetRuleProvider(defaultProviderSetting) as RuleProviderBase ??
                GetRuleProvider(fallbackProviderSetting) as RuleProviderBase;
        }

        public static Type[] GetRuleProviders() {

            var assemblyStartsWith = ConfigurationHelper.GetAppSettingsList("Relations.RuleProviderAssembliesStartsWith");

            var type = typeof(RuleProviderBase);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .Where(ass => ass.IsDynamic == false)
                .Where(ass => !assemblyStartsWith.Any() || assemblyStartsWith.Any(x => ass.FullName.StartsWith(x)))
                .SelectMany(ass => ass.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p != type);
            return types.ToArray<Type>();
            
        }

        public static RuleProviderBase Provider { 
            get {
                if (defaultProvider == null)
                    Initialize();
                return defaultProvider;
            }
        } 

        private static ProviderBase GetRuleProvider(string providerType)
        {
            try
            {
                RuleProviderBase base2 = null;
                if (string.IsNullOrEmpty(providerType))
                {
                    throw new ArgumentException("Provider type is invalid");
                }
                Type c = Type.GetType(providerType, true, true);
                if (!typeof (RuleProviderBase).IsAssignableFrom(c))
                {
                    throw new ArgumentException(String.Format("Provider must implement type {0}.",
                        typeof (RuleProviderBase).ToString()));
                }
                base2 = (RuleProviderBase) Activator.CreateInstance(c);
                var config = new NameValueCollection();
                config.Add("name", providerType);
                base2.ProviderName = providerType;

                base2.Initialize(providerType, config);
                return base2;
            }
            catch (Exception e)
            {
                throw new Exception("Error loading RuleProvider.", e);
            }

        }


    }
}