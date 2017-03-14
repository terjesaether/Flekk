namespace SpotzWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedImages : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Images", "SpotzId", "dbo.Spotzs");
            DropIndex("dbo.Images", new[] { "SpotzId" });
            RenameColumn(table: "dbo.Images", name: "SpotzId", newName: "Spotz_SpotzId");
            AlterColumn("dbo.Images", "Spotz_SpotzId", c => c.Guid());
            CreateIndex("dbo.Images", "Spotz_SpotzId");
            AddForeignKey("dbo.Images", "Spotz_SpotzId", "dbo.Spotzs", "SpotzId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Images", "Spotz_SpotzId", "dbo.Spotzs");
            DropIndex("dbo.Images", new[] { "Spotz_SpotzId" });
            AlterColumn("dbo.Images", "Spotz_SpotzId", c => c.Guid(nullable: false));
            RenameColumn(table: "dbo.Images", name: "Spotz_SpotzId", newName: "SpotzId");
            CreateIndex("dbo.Images", "SpotzId");
            AddForeignKey("dbo.Images", "SpotzId", "dbo.Spotzs", "SpotzId", cascadeDelete: true);
        }
    }
}
