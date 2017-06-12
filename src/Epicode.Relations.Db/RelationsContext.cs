using System.Data.Entity;
using EPiCode.Relations.Db.Data;

namespace EPiCode.Relations.Db
{
    public class RelationsContext : DbContext
    {
        public RelationsContext(string connectonString) : base(connectonString)
        {
        }

        public DbSet<Rule> Rules { get; set; }
        public DbSet<Relation> Relations { get; set; }
        public DbSet<RulePageType> RulePageType { get; set; }
       

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Relation>().ToTable("EPiCodeRelations_Relation");
            modelBuilder.Entity<Rule>().ToTable("EPiCodeRelations_Rule");
            modelBuilder.Entity<RulePageType>().ToTable("EPiCodeRelations_RulePageType");

            modelBuilder.Entity<RulePageType>()
                        .HasRequired(rpt => rpt.Rule)
                        .WithMany(r => r.PageTypeList)
                        .Map(m => m.MapKey("RuleId"));

        }


        public static RelationsContext CreateContext()
        {
            return new RelationsContext(RelationsConfiguration.Instance.ConnectionStringName);
        }
    }
}
