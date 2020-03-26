namespace PetGrooming.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class booking_services_MxM : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GroomServiceGroomBookings",
                c => new
                    {
                        GroomService_GroomServiceID = c.Int(nullable: false),
                        GroomBooking_GroomBookingID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.GroomService_GroomServiceID, t.GroomBooking_GroomBookingID })
                .ForeignKey("dbo.GroomServices", t => t.GroomService_GroomServiceID, cascadeDelete: true)
                .ForeignKey("dbo.GroomBookings", t => t.GroomBooking_GroomBookingID, cascadeDelete: true)
                .Index(t => t.GroomService_GroomServiceID)
                .Index(t => t.GroomBooking_GroomBookingID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GroomServiceGroomBookings", "GroomBooking_GroomBookingID", "dbo.GroomBookings");
            DropForeignKey("dbo.GroomServiceGroomBookings", "GroomService_GroomServiceID", "dbo.GroomServices");
            DropIndex("dbo.GroomServiceGroomBookings", new[] { "GroomBooking_GroomBookingID" });
            DropIndex("dbo.GroomServiceGroomBookings", new[] { "GroomService_GroomServiceID" });
            DropTable("dbo.GroomServiceGroomBookings");
        }
    }
}
