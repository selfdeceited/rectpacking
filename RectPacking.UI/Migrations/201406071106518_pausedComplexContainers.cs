namespace RectPacking.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pausedComplexContainers : DbMigration
    {
        public override void Up()
        {
           /* DropForeignKey("dbo.Container", "RoomId", "dbo.Container");
            DropIndex("dbo.Container", new[] { "RoomId" });
            DropColumn("dbo.Container", "IsRoom");
            DropColumn("dbo.Container", "RoomId");*/
        }
        
        public override void Down()
        {/*
            AddColumn("dbo.Container", "RoomId", c => c.Long(nullable: false));
            AddColumn("dbo.Container", "IsRoom", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Container", "RoomId");
            AddForeignKey("dbo.Container", "RoomId", "dbo.Container", "Id");*/
        }
    }
}
