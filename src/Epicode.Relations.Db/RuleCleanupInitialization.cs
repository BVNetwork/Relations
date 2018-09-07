using System.Data.Entity;
using System.Linq;
using EPiServer.DataAbstraction;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

namespace EPiCode.Relations.Db
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class RuleCleanupInitialization : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            if (RelationsConfiguration.Instance.RemoveDeletedPageTypesFromRules)
            {
                // Get all pagetypename
                var contentTypeRepository = ServiceLocator.Current.GetInstance<IContentTypeRepository>();

                var pageTypes = contentTypeRepository.List().Where(x => !(x.Name.EndsWith("Block") || x.Name.EndsWith("File")))
                    .OrderBy(x => x.DisplayName)
                    .Select(pagetype => pagetype.Name)
                    .ToList();

                using (var ctx = RelationsContext.CreateContext())
                {
                    var rules = ctx.Rules.Include(x => x.PageTypeList).ToList();
                    foreach (var rule in rules)
                    {
                        var rulePageTypeToRemove = rule.PageTypeList.Where(rulePageType => !pageTypes.Contains(rulePageType.PageTypeName)).ToList();

                        foreach (var rulePageType in rulePageTypeToRemove)
                        {
                            rule.PageTypeList.Remove(rulePageType);
                            ctx.RulePageType.Remove(rulePageType);
                        }
                    }

                    ctx.SaveChanges();
                }
            }
        }

        public void Uninitialize(InitializationEngine context)
        {

        }

        public void Preload(string[] parameters)
        {

        }
    }
}