using EPiServer.PlugIn;

namespace EPiCode.Relations.Plugins.Edit.EnsureScriptManager
{
    [GuiPlugIn(Area = PlugInArea.EditPanel)]
    public class EnsureScriptManagerPlugin 
    {
        /*         public PlugInDescriptor[] List()
                {
                   var editPanel = HttpContext.Current.Handler as EditPanel;
            
                    if (editPanel != null)
                    {
                        editPanel.Page.Init += delegate
                        {
                            var scriptManager = ScriptManager.GetCurrent(editPanel.Page);
                            if (scriptManager == null)
                            {
                                scriptManager = new ScriptManager
                                {
                                    ID = "scriptManager",
                                    EnablePartialRendering = true
                                };
                                editPanel.Page.Form.Controls.AddAt(0, scriptManager);
                            }
                        };
                    }

                    return new PlugInDescriptor[] { };
  
                }*/
    }
}