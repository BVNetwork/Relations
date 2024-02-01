using EPiServer;
using System.Collections.Generic;

namespace EPiCode.Relations.Core
{
    public class PageEvents : EPiServer.PlugIn.PlugInAttribute
    {
        // TODO NETCORE: Check this
        // public static void Start()
        // {
        //     DataFactory.Instance.DeletedContent += Instance_DeletedContent;
        // }

        private static void Instance_DeletedContent(object sender, DeleteContentEventArgs e)
        {
            if (e.DeletedDescendents != null)
            {
                foreach (var deletedDescendents in e.DeletedDescendents)
                {
                    List<Relation> relationsToDelete = RelationEngine.Instance.GetRelationsForPage(deletedDescendents.ID);
                    foreach (Relation relation in relationsToDelete)
                    {
                        RelationEngine.Instance.DeleteRelation(relation);
                    }
                }
            }
        }
    }
}