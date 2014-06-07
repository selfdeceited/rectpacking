namespace RectPacking.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedOrderDatesToDateTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Order", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Order", "EndDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Order", "EndDate", c => c.String());
            AlterColumn("dbo.Order", "StartDate", c => c.String());
        }
    }
}
