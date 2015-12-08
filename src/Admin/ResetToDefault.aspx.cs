using System;
using EPiCode.Relations.Core.RelationProviders;
using EPiCode.Relations.Core;

namespace EPiCode.Relations.Plugins.Admin
{
    public partial class ResetToDefault : EPiServer.UI.SystemPageBase
    {

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            Settings.SaveSetting("DefaultRelationProviderString", "");
            RelationProviderManager.Initialize();
            Settings.SaveSetting("DefaultRuleProviderString", "");
            RuleProviderManager.Initialize();
        }


    }



}