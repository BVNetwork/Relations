<%@ Page Language="C#" AutoEventWireup="true" Inherits="System.Web.UI.Page" %>
<%@ Import Namespace="EPiCode.Relations.Core" %>
<%@ Import Namespace="EPiCode.Relations.Core.RelationProviders" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
        <script type="text/C#" runat="server">
        protected override void OnLoad(EventArgs e)
{
    EPiCode.Relations.Core.Settings.SaveSetting("DefaultRelationProviderString", "EPiCode.Relations.Core.RelationProviders.DDSInMemoryProvider.DDSInMemoryRelationProvider, EPiCode.Relations");
    RelationProviderManager.Initialize();
    EPiCode.Relations.Core.Settings.SaveSetting("DefaultRuleProviderString", "EPiCode.Relations.Core.RuleProviders.DDSInMemoryProvider.DDSInMemoryRuleProvider, EPiCode.Relations");
    RuleProviderManager.Initialize();

            base.OnLoad(e);
}
        
    
    </script>

<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        
        <div>
    
    </div>
    </form>
</body>
</html>
