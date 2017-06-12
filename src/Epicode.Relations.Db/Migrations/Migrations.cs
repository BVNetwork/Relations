using System.Data.Entity.Migrations;

namespace EPiCode.Relations.Db.Migrations
{
    public partial class AddPostClass : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "Relation",
            //    c => new
            //        {
            //            PostId = c.Int(nullable: false, identity: true),
            //            Title = c.String(maxLength: 200),
            //            Content = c.String(),
            //            BlogId = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.PostId)
            //    .ForeignKey("Blogs", t => t.BlogId, cascadeDelete: true)
            //    .Index(t => t.BlogId)
            //    .Index(p => p.Title, unique: true);

            //AddColumn("Blogs", "Rating", c => c.Int(nullable: false, defaultValue: 3));
        }

        public override void Down()
        {
            //DropIndex("Posts", new[] { "Title" });
            //DropIndex("Posts", new[] { "BlogId" });
            //DropForeignKey("Posts", "BlogId", "Blogs");
            //DropColumn("Blogs", "Rating");
            //DropTable("Posts");
        }
    }
}