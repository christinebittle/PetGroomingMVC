namespace PetGrooming.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Booking_1_Many : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GroomBookings", "PetID", c => c.Int(nullable: false));
            AddColumn("dbo.GroomBookings", "GroomerID", c => c.Int(nullable: false));
            AddColumn("dbo.GroomBookings", "OwnerID", c => c.Int(nullable: false));
            CreateIndex("dbo.GroomBookings", "PetID");
            CreateIndex("dbo.GroomBookings", "GroomerID");
            CreateIndex("dbo.GroomBookings", "OwnerID");
            AddForeignKey("dbo.GroomBookings", "GroomerID", "dbo.Groomers", "GroomerID", cascadeDelete: true);
            AddForeignKey("dbo.GroomBookings", "OwnerID", "dbo.Owners", "OwnerID", cascadeDelete: true);
            AddForeignKey("dbo.GroomBookings", "PetID", "dbo.Pets", "PetID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GroomBookings", "PetID", "dbo.Pets");
            DropForeignKey("dbo.GroomBookings", "OwnerID", "dbo.Owners");
            DropForeignKey("dbo.GroomBookings", "GroomerID", "dbo.Groomers");
            DropIndex("dbo.GroomBookings", new[] { "OwnerID" });
            DropIndex("dbo.GroomBookings", new[] { "GroomerID" });
            DropIndex("dbo.GroomBookings", new[] { "PetID" });
            DropColumn("dbo.GroomBookings", "OwnerID");
            DropColumn("dbo.GroomBookings", "GroomerID");
            DropColumn("dbo.GroomBookings", "PetID");
        }
    }
}
