using System.Collections.Generic;
using EPiServer.ServiceLocation;
using Microsoft.Extensions.Configuration;

namespace EPiCode.Relations.Helpers
{
    public class ConfigurationHelper
    {
        /// <summary>
        /// Gets the value of an appSettings key as an bool. If not defined
        /// or not parsable, the defaultValue parameter will be returned.
        /// </summary>
        /// <param name="key">The key in the appSettings section whose value to return.</param>
        /// <param name="defaultValue">The default value to returned if the setting is null or not parsable.</param>
        /// <returns>The appSettings value if parsable, defaultValue if not</returns>
        public static bool GetAppSettingsConfigValueBool(string key, bool defaultValue)
        {
            return ServiceLocator.Current.GetInstance<IConfiguration>().GetValue(key, defaultValue);
        }

        private static string GetAppSettingsConfig(string key, string defaultValue)
        {
            return ServiceLocator.Current.GetInstance<IConfiguration>().GetValue(key, defaultValue);
        }

        public static IList<string> GetAppSettingsList(string key)
        {
            var stringValue = GetAppSettingsConfig(key, null);

            if (stringValue != null)
            {
                return stringValue.Split('|');
            }

            return new List<string>();
        }
    }
}