using System;
using EPiCode.Relations.Db.PageSearch;
using EPiCode.Relations.Db.Properties;

namespace EPiCode.Relations.Db
{
    public class RelationsConfiguration
    {
        public string ConnectionStringName
        {
            get { return Settings.Default.ConnectionStringName; }
        }

        public bool RemoveDeletedPageTypesFromRules
        {
            get { return Settings.Default.RemoveDeletedPageTypesFromRules; }
        }

        public IPageSearch PageSearch
        {
            get { return (IPageSearch)Activator.CreateInstance(PageSearchType); }
        }

        private Type _pageSearchType;
        private Type PageSearchType
        {
            get { return _pageSearchType ?? (_pageSearchType = GetConfiguredOrDefaultType()); }
        }

        private Type GetConfiguredOrDefaultType()
        {
            return !string.IsNullOrWhiteSpace(Settings.Default.PageSearch) ? Type.GetType(Settings.Default.PageSearch) : typeof (PageSearchFindPagesWithCriteria);
        }

        private static readonly Lazy<RelationsConfiguration> Lazy = new Lazy<RelationsConfiguration>(() => new RelationsConfiguration());

        public static RelationsConfiguration Instance
        {
            get { return Lazy.Value; }
        }

        private RelationsConfiguration()
        {
        }
    }
}