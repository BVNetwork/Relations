using System;
using System.Web.UI.WebControls;
using EPiCode.Relations.Core.RelationProviders;
using EPiCode.Relations.Core;

namespace EPiCode.Relations.Plugins.Admin.Units
{
    public partial class SettingsControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ProviderOverview.DataBind();
            if (!IsPostBack)
            {

                //"EPiCode.Relations.Core.RelationProviders.DynamicDataStoreProvider.DDSRelationProvider,EPiCode.Relations;DDSRelationProvider";

            }
        }

        protected void SearchProviders_Click(object sender, EventArgs e)
        {
            ProviderDropDowns.Visible = true;
            ProviderOverview.Visible = false;
            RelationProvidersDropDown.Items.Clear();
            string defaultRelationProvider = Settings.GetSettingValue("DefaultRelationProviderString");
            Type[] RelationProviders = RelationProviderManager.GetRelationProviders();
            foreach (Type t in RelationProviders)
            {
                ListItem li = new ListItem(t.Name, t.FullName + ", "+t.Assembly.FullName.Substring(0,t.Assembly.FullName.IndexOf(',')));
                li.Selected = li.Value == defaultRelationProvider;
                RelationProvidersDropDown.Items.Add(li);
            }

            RuleProvidersDropDown.Items.Clear();
            string defaultRuleProvider = Settings.GetSettingValue("DefaultRuleProviderString");
            Type[] RuleProviders = RuleProviderManager.GetRuleProviders();
            foreach (Type t in RuleProviders)
            {
                ListItem li = new ListItem(t.Name, t.FullName + ", " + t.Assembly.FullName.Substring(0, t.Assembly.FullName.IndexOf(',')));
                li.Selected = li.Value == defaultRuleProvider;
                RuleProvidersDropDown.Items.Add(li);
            }
            SaveButton.Visible = true;
        }

        protected void SaveSettings_Click(object sender, EventArgs e) {
            Settings.SaveSetting("DefaultRelationProviderString", RelationProvidersDropDown.SelectedValue);
            RelationProviderManager.Initialize();
            Settings.SaveSetting("DefaultRuleProviderString", RuleProvidersDropDown.SelectedValue);
            RuleProviderManager.Initialize();
            ProviderDropDowns.Visible = false;
            SaveButton.Visible = false;
            ProviderOverview.Visible = true;
            ProviderOverview.DataBind();
        }



    }


}