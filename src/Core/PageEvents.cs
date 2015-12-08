using System.Collections.Generic;

namespace EPiCode.Relations.Core
{
    public class PageEvents : EPiServer.PlugIn.PlugInAttribute
    {
        public static void Start() {
            EPiServer.DataFactory.Instance.DeletedPage += new EPiServer.PageEventHandler(Instance_DeletedPage);
        }

        static void Instance_DeletedPage(object sender, EPiServer.PageEventArgs e)
        {
            if(e.Page != null && e.Page.PageLink != null) {
                List<Relation> relationsToDelete = RelationEngine.Instance.GetRelationsForPage(e.Page.PageLink.ID);
                foreach (Relation relation in relationsToDelete)
                {
                    RelationEngine.Instance.DeleteRelation(relation);
                }
            }
        }
    }
}