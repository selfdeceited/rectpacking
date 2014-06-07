namespace RectPacking.UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Client",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FullName = c.String(),
                        Phone = c.String(),
                        Address = c.String(),
                        LoyaltyIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.COA",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProductId = c.Long(nullable: false),
                        FrameId = c.Long(nullable: false),
                        ContainerId = c.Long(nullable: false),
                        Left = c.Int(nullable: false),
                        Top = c.Int(nullable: false),
                        IsRotated = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Frame", t => t.FrameId, cascadeDelete: true)
                .ForeignKey("dbo.Container", t => t.ContainerId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.FrameId)
                .Index(t => t.ContainerId);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ImageId = c.Long(nullable: false),
                        Width = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                        StandardName = c.String(),
                        FreezeTime = c.Time(nullable: false),
                        ConcreteTypeId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Image", t => t.ImageId, cascadeDelete: true)
                .ForeignKey("dbo.ConcreteType", t => t.ConcreteTypeId, cascadeDelete: true)
                .Index(t => t.ImageId)
                .Index(t => t.ConcreteTypeId);
            
            CreateTable(
                "dbo.Image",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ImageUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConcreteType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductSet",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        OrderId = c.Long(nullable: false),
                        ProductId = c.Long(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Order", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        StartDate = c.String(),
                        EndDate = c.String(),
                        ClientId = c.Long(nullable: false),
                        Rating = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Client", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IsManager = c.Boolean(nullable: false),
                        IsAdmin = c.Boolean(nullable: false),
                        UserName = c.String(),
                        SaltedPassword = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        Enabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Frame",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FreezeTime = c.Time(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        PlacementId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Placement", t => t.PlacementId, cascadeDelete: true)
                .Index(t => t.PlacementId);
            
            CreateTable(
                "dbo.Placement",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ContainerId = c.Long(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Container", t => t.ContainerId, cascadeDelete: false)
                .Index(t => t.ContainerId);

            CreateTable(
                "dbo.Container",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    //IsRoom = c.Boolean(nullable: false),
                    //RoomId = c.Long(nullable: false),
                    Width = c.Int(nullable: false),
                    Height = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);
            //.ForeignKey("dbo.Container", t => t.RoomId)
            //.Index(t => t.RoomId);

        }
        
        public override void Down()
        {
           // DropIndex("dbo.Container", new[] { "RoomId" });
            DropIndex("dbo.Placement", new[] { "ContainerId" });
            DropIndex("dbo.Frame", new[] { "PlacementId" });
            DropIndex("dbo.Order", new[] { "ClientId" });
            DropIndex("dbo.Order", new[] { "UserId" });
            DropIndex("dbo.ProductSet", new[] { "ProductId" });
            DropIndex("dbo.ProductSet", new[] { "OrderId" });
            DropIndex("dbo.Product", new[] { "ConcreteTypeId" });
            DropIndex("dbo.Product", new[] { "ImageId" });
            DropIndex("dbo.COA", new[] { "ContainerId" });
            DropIndex("dbo.COA", new[] { "FrameId" });
            DropIndex("dbo.COA", new[] { "ProductId" });
            //DropForeignKey("dbo.Container", "RoomId", "dbo.Container");
            DropForeignKey("dbo.Placement", "ContainerId", "dbo.Container");
            DropForeignKey("dbo.Frame", "PlacementId", "dbo.Placement");
            DropForeignKey("dbo.Order", "ClientId", "dbo.Client");
            DropForeignKey("dbo.Order", "UserId", "dbo.User");
            DropForeignKey("dbo.ProductSet", "ProductId", "dbo.Product");
            DropForeignKey("dbo.ProductSet", "OrderId", "dbo.Order");
            DropForeignKey("dbo.Product", "ConcreteTypeId", "dbo.ConcreteType");
            DropForeignKey("dbo.Product", "ImageId", "dbo.Image");
            DropForeignKey("dbo.COA", "ContainerId", "dbo.Container");
            DropForeignKey("dbo.COA", "FrameId", "dbo.Frame");
            DropForeignKey("dbo.COA", "ProductId", "dbo.Product");
            DropTable("dbo.Container");
            DropTable("dbo.Placement");
            DropTable("dbo.Frame");
            DropTable("dbo.User");
            DropTable("dbo.Order");
            DropTable("dbo.ProductSet");
            DropTable("dbo.ConcreteType");
            DropTable("dbo.Image");
            DropTable("dbo.Product");
            DropTable("dbo.COA");
            DropTable("dbo.Client");
        }
    }
}
