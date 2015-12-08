using EPiServer.Shell;
using EPiServer.Shell.ViewComposition;

namespace EPiCode.Relations.Gadgets
{
    [Component(
        WidgetType = "relations.RelationEditor",
        PlugInAreas = PlugInArea.AssetsDefaultGroup,
        SortOrder= 900000,
        Title = "Relation editor", 
        Categories="CMS",
        Description = "Add or remove relations to current content.")]
    public class RelationEditorGadget : ComponentBase
    {
        public RelationEditorGadget()
            : base("relations.RelationEditor")
        {
            this.Heading = "Relations";
            this.SortOrder = 10000;

        }
    }
    /*
    [Component(
        //Auto-plugs in the component to the assets panel of cms
        //(See EPiServer.Cms.Shell.PlugInArea
        //in the EPiServer.Cms.Shell.UI assembly for CMS constants)
        WidgetType = "relations.RelationOverview", 
        IsAvailableForUserSelection=true,
        PlugInAreas = PlugInArea.DefaultAssetsGroup,
        //Define language path to translate Title/Description.
        //LanguagePath = "/customtranslations/components/customcomponent";
        Title = "Relation editor",
        Description = "Overview of current relations.")]
    public class RelationOverviewGadget : ComponentBase
    {
        public RelationOverviewGadget()
            : base("relations.RelationOverview")
        {
            this.Heading = "Overview";
        }
    }
     */ 
    /*
    [Component]
    public class RelationEditorGroup : ComponentDefinitionBase
    {
        public RelationEditorGroup()
            : base("")
        {
            Categories = new[] { "cms" };

            PlugInAreas = new[] { "/episerver/cms/DefaultAssetsGroup" };
            base.Title = "Relations";

        }

        public override IComponent CreateComponent()
        {

            var container = new ComponentGroup("Relations");

            container.ContainerType = ContainerType.User;

            var listComponent = new RelationEditorGadget
            {
                Heading = "Relation editor"
            };
            listComponent.Settings["sortOptions"] = "test";
            container.Add(listComponent);

            var listComponent2 = new RelationOverviewGadget
            {
                Heading = "Overview"
            };
            container.Add(listComponent2);

            return container;
        }
    }*/
}