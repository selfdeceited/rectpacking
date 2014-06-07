namespace RectPacking.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNameToContainer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Container", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Container", "Name");
        }
    }
}
