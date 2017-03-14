namespace SpotzWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDbSets : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Guid(nullable: false),
                        Text = c.String(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        Spotz_SpotzId = c.Guid(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Spotzs", t => t.Spotz_SpotzId)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Spotz_SpotzId)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Spotzs",
                c => new
                    {
                        SpotzId = c.Guid(nullable: false),
                        Title = c.String(nullable: false),
                        Description = c.String(),
                        Longitude = c.String(),
                        Latitude = c.String(),
                        Timestamp = c.DateTime(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.SpotzId)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.CommentToSpotzs",
                c => new
                    {
                        CommentToSpotzId = c.Int(nullable: false, identity: true),
                        SpotzId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.CommentToSpotzId)
                .ForeignKey("dbo.Spotzs", t => t.SpotzId, cascadeDelete: true)
                .Index(t => t.SpotzId);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        ImageId = c.Guid(nullable: false),
                        ImageUrl = c.String(),
                        Filename = c.String(),
                        Datestamp = c.DateTime(nullable: false),
                        Data = c.Binary(),
                        Spotz_SpotzId = c.Guid(),
                    })
                .PrimaryKey(t => t.ImageId)
                .ForeignKey("dbo.Spotzs", t => t.Spotz_SpotzId)
                .Index(t => t.Spotz_SpotzId);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        TagId = c.Guid(nullable: false),
                        TagName = c.String(),
                    })
                .PrimaryKey(t => t.TagId);
            
            CreateTable(
                "dbo.TagsToSpotzs",
                c => new
                    {
                        TagsToSpotzId = c.Int(nullable: false, identity: true),
                        Spotz_SpotzId = c.Guid(),
                    })
                .PrimaryKey(t => t.TagsToSpotzId)
                .ForeignKey("dbo.Spotzs", t => t.Spotz_SpotzId)
                .Index(t => t.Spotz_SpotzId);
            
            CreateTable(
                "dbo.TagSpotzs",
                c => new
                    {
                        Tag_TagId = c.Guid(nullable: false),
                        Spotz_SpotzId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_TagId, t.Spotz_SpotzId })
                .ForeignKey("dbo.Tags", t => t.Tag_TagId, cascadeDelete: true)
                .ForeignKey("dbo.Spotzs", t => t.Spotz_SpotzId, cascadeDelete: true)
                .Index(t => t.Tag_TagId)
                .Index(t => t.Spotz_SpotzId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Spotzs", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.TagsToSpotzs", "Spotz_SpotzId", "dbo.Spotzs");
            DropForeignKey("dbo.TagSpotzs", "Spotz_SpotzId", "dbo.Spotzs");
            DropForeignKey("dbo.TagSpotzs", "Tag_TagId", "dbo.Tags");
            DropForeignKey("dbo.Images", "Spotz_SpotzId", "dbo.Spotzs");
            DropForeignKey("dbo.CommentToSpotzs", "SpotzId", "dbo.Spotzs");
            DropForeignKey("dbo.Comments", "Spotz_SpotzId", "dbo.Spotzs");
            DropIndex("dbo.TagSpotzs", new[] { "Spotz_SpotzId" });
            DropIndex("dbo.TagSpotzs", new[] { "Tag_TagId" });
            DropIndex("dbo.TagsToSpotzs", new[] { "Spotz_SpotzId" });
            DropIndex("dbo.Images", new[] { "Spotz_SpotzId" });
            DropIndex("dbo.CommentToSpotzs", new[] { "SpotzId" });
            DropIndex("dbo.Spotzs", new[] { "User_Id" });
            DropIndex("dbo.Comments", new[] { "User_Id" });
            DropIndex("dbo.Comments", new[] { "Spotz_SpotzId" });
            DropTable("dbo.TagSpotzs");
            DropTable("dbo.TagsToSpotzs");
            DropTable("dbo.Tags");
            DropTable("dbo.Images");
            DropTable("dbo.CommentToSpotzs");
            DropTable("dbo.Spotzs");
            DropTable("dbo.Comments");
        }
    }
}
