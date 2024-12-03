using EPiServer;
using System.Collections.Generic;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;

namespace EPiCode.Relations.Core
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class PageEventsInitialization : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            var contentEvents = context.Locate.ContentEvents();
            contentEvents.DeletedContent += Instance_DeletedContent;
        }

        public void Uninitialize(InitializationEngine context)
        {
            
        }
        
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