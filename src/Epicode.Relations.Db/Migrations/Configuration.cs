using System.Data.Entity.Migrations;

namespace EPiCode.Relations.Db.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<RelationsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = RelationsConfiguration.Instance.AutomaticMigrationsEnabled;
            AutomaticMigrationDataLossAllowed = false;          
        }

        protected override void Seed(RelationsContext context)
        {
            //  This method will be called after migrating to the latest version.
        }
    }
}
