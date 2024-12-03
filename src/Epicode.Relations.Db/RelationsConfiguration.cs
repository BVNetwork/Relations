using System;
using EPiCode.Relations.Db.PageSearch;
using EPiServer.ServiceLocation;

namespace EPiCode.Relations.Db
{
    public class RelationsConfiguration
    {
        public string ConnectionStringName => "EPiServerDB";

        public bool RemoveDeletedPageTypesFromRules => true;

        public bool AutomaticMigrationsEnabled => true;

        public IPageSearch PageSearch => ServiceLocator.Current.GetInstance<IPageSearch>();

        private static readonly Lazy<RelationsConfiguration> Lazy = new(() => new RelationsConfiguration());

        public static RelationsConfiguration Instance => Lazy.Value;

        private RelationsConfiguration()
        {
        }
    }
}