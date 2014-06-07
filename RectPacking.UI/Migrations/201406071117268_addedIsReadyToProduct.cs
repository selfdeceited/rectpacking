namespace RectPacking.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedIsReadyToProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Client", "ClientName", c => c.String());
            AddColumn("dbo.Product", "IsReady", c => c.Boolean(nullable: false));
            DropColumn("dbo.Client", "FullName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Client", "FullName", c => c.String());
            DropColumn("dbo.Product", "IsReady");
            DropColumn("dbo.Client", "ClientName");
        }
    }
}
