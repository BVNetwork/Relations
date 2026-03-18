using System;
using EPiCode.Relations.Db.PageSearch;
using EPiServer.ServiceLocation;
using Microsoft.Extensions.Options;

namespace EPiCode.Relations.Db
{
    public class RelationsConfiguration
    {
        public string ConnectionStringName => "EPiServerDB";

        public bool RemoveDeletedPageTypesFromRules =>
            !ServiceLocator.Current.TryGetExistingInstance<IOptions<RelationsDbOptions>>(out var opts) 
            || opts.Value.RemoveDeletedPageTypesFromRules;

        public bool AutomaticMigrationsEnabled =>
            !ServiceLocator.Current.TryGetExistingInstance<IOptions<RelationsDbOptions>>(out var opts) 
            || opts.Value.AutomaticMigrationsEnabled;

        public IPageSearch PageSearch => ServiceLocator.Current.GetInstance<IPageSearch>();

        private static readonly Lazy<RelationsConfiguration> Lazy = new(() => new RelationsConfiguration());

        public static RelationsConfiguration Instance => Lazy.Value;

        private RelationsConfiguration()
        {
        }
    }
}