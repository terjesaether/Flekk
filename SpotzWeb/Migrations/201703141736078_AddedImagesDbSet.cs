namespace SpotzWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedImagesDbSet : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Images", "Spotz_SpotzId", "dbo.Spotzs");
            DropIndex("dbo.Images", new[] { "Spotz_SpotzId" });
            RenameColumn(table: "dbo.Images", name: "Spotz_SpotzId", newName: "SpotzId");
            AddColumn("dbo.Images", "Timestamp", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Images", "SpotzId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Images", "SpotzId");
            AddForeignKey("dbo.Images", "SpotzId", "dbo.Spotzs", "SpotzId", cascadeDelete: true);
            DropColumn("dbo.Images", "Datestamp");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Images", "Datestamp", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.Images", "SpotzId", "dbo.Spotzs");
            DropIndex("dbo.Images", new[] { "SpotzId" });
            AlterColumn("dbo.Images", "SpotzId", c => c.Guid());
            DropColumn("dbo.Images", "Timestamp");
            RenameColumn(table: "dbo.Images", name: "SpotzId", newName: "Spotz_SpotzId");
            CreateIndex("dbo.Images", "Spotz_SpotzId");
            AddForeignKey("dbo.Images", "Spotz_SpotzId", "dbo.Spotzs", "SpotzId");
        }
    }
}
