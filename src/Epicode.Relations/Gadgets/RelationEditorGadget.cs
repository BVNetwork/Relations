using EPiCode.Relations.Helpers;
using EPiServer.Shell;
using EPiServer.Shell.ViewComposition;

namespace EPiCode.Relations.Gadgets
{
    [Component(
        WidgetType = "relations/RelationEditor",
        PlugInAreas = PlugInArea.AssetsDefaultGroup,
        SortOrder= 900000,
        Title = "Relation editor", 
        Categories="CMS",
        Description = "Add or remove relations to current content.")]
    public class RelationEditorGadget : ComponentBase
    {
        public RelationEditorGadget()
            : base("relations/RelationEditor")
        {
            Heading = TranslationHelper.Translate("/relations/admin/relations",  "Relations");
            SortOrder = 10000;
            Settings.Add("dblclick", ConfigurationHelper.GetAppSettingsConfigValueBool("Relations.EnableDoubleClick", false));
            Settings.Add("useDynamicHeight", ConfigurationHelper.GetAppSettingsConfigValueBool("Relations.UseDynamicHeight", false));
        }
    }
}