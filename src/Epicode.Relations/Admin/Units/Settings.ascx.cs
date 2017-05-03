using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiCode.Relations.Core;
using EPiServer.Data.Dynamic;
using EPiServer.Data;

namespace EPiCode.Relations.Plugins.Admin.Units
{
    public partial class Settings : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        
        private DynamicDataStore SettingsDataStore
        {
            get
            {
                return typeof(Setting).GetStore();
            }
        }
        
        public  Rule GetSetting(Identity id)
        {
            var settings = from r in SettingsDataStore.Items<Setting>()
                        where r.I == id
                        select r;
            return settings.First<Setting>();
        }


        protected void SaveProvider_Click(object sender, EventArgs e)
        {
            
            //ProviderTextBox.Text

        }

        public sealed class Setting {
            string DefaultProvider;
        }

    }


}