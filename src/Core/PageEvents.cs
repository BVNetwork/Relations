using EPiServer;
using System.Collections.Generic;

namespace EPiCode.Relations.Core
{
    public class PageEvents : EPiServer.PlugIn.PlugInAttribute
    {
        public static void Start() {
            EPiServer.DataFactory.Instance.DeletedContent += Instance_DeletedContent;
        }

        private static void Instance_DeletedContent(object sender, DeleteContentEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        static void Instance_DeletedPage(object sender, DeleteContentEventArgs e)
        {
            if(e.Content != null && e.ContentLink != null) {
                List<Relation> relationsToDelete = RelationEngine.Instance.GetRelationsForPage(e.ContentLink.ID);
                foreach (Relation relation in relationsToDelete)
                {
                    RelationEngine.Instance.DeleteRelation(relation);
                }
            }
        }
    }
}