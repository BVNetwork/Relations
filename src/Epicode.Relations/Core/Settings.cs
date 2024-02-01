using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Data.Dynamic;
using EPiServer.Data;

namespace EPiCode.Relations.Core
{
    [EPiServerDataStore(AutomaticallyCreateStore = true, AutomaticallyRemapStore = true)]
    public class Settings
    {
        public Identity Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        protected void Initialize()
        {
            Id = Identity.NewIdentity(Guid.NewGuid());
        }

        public Settings(string key, string value)
        {
            Initialize();
        }

        public Settings()
        {
            Initialize();
        }

        private static DynamicDataStore SettingsDataStore
        {
            get
            {
                return typeof(Settings).GetStore();
            }
        }

        public static string GetSettingValue(string key)
        {
            var items = SettingsDataStore.Items<Settings>();
            
            var settings = from r in items
                           where r.Key == key
                           select r;
            List<Settings> currentSettings = settings.ToList<Settings>();

            if (currentSettings.Count() > 0)
                return currentSettings.ElementAt<Settings>(0).Value;
            return null;
        }

        public static void SaveSetting(string key, string value)
        {
            Settings currentSetting = null;
            IEnumerable<Settings> setting = SettingsDataStore.Find<Settings>("Key", key);
            if (setting.Any<Settings>())
            {
                currentSetting = setting.First<Settings>();
                if (currentSetting != null)
                    currentSetting.Value = value;
            }
            if(currentSetting == null)
                currentSetting = new Settings { Key = key, Value = value };
            SettingsDataStore.Save(currentSetting);
        }

    }
}