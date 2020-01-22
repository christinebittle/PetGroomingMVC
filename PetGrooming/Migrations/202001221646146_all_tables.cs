namespace PetGrooming.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class all_tables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GroomBookings",
                c => new
                    {
                        GroomBookingID = c.Int(nullable: false, identity: true),
                        GroomBookingDate = c.DateTime(nullable: false),
                        GroomBookingPrice = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GroomBookingID);
            
            CreateTable(
                "dbo.GroomServices",
                c => new
                    {
                        GroomServiceID = c.Int(nullable: false, identity: true),
                        ServiceName = c.String(),
                        ServiceCost = c.Int(nullable: false),
                        ServiceDuration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GroomServiceID);
            
            CreateTable(
                "dbo.Owners",
                c => new
                    {
                        OwnerID = c.Int(nullable: false, identity: true),
                        OwnerFname = c.String(),
                        OwnerLname = c.String(),
                        OwnerAddress = c.String(),
                        OwnerWorkPhone = c.String(),
                        OwnerHomePhone = c.String(),
                    })
                .PrimaryKey(t => t.OwnerID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Owners");
            DropTable("dbo.GroomServices");
            DropTable("dbo.GroomBookings");
        }
    }
}
