namespace SpotzWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedGravatarField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "GravatarUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "GravatarUrl");
        }
    }
}
