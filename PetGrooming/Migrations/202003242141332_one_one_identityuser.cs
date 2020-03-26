namespace PetGrooming.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class one_one_identityuser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GroomBookings", "GroomerID", "dbo.Groomers");
            DropForeignKey("dbo.GroomBookings", "OwnerID", "dbo.Owners");
            DropForeignKey("dbo.PetOwners", "Owner_OwnerID", "dbo.Owners");
            DropIndex("dbo.GroomBookings", new[] { "GroomerID" });
            DropIndex("dbo.GroomBookings", new[] { "OwnerID" });
            DropIndex("dbo.PetOwners", new[] { "Owner_OwnerID" });
            DropPrimaryKey("dbo.Groomers");
            DropPrimaryKey("dbo.Owners");
            DropPrimaryKey("dbo.PetOwners");
            DropColumn("dbo.Groomers", "GroomerID");
            DropColumn("dbo.Owners", "OwnerID");
            DropColumn("dbo.PetOwners","Owner_OwnerID");
            DropColumn("dbo.GroomBookings", "GroomerID");
            DropColumn("dbo.GroomBookings", "OwnerID");
            AddColumn("dbo.GroomBookings", "GroomerID", c => c.String(maxLength: 128));
            AddColumn("dbo.GroomBookings", "OwnerID", c => c.String(maxLength: 128));
            AddColumn("dbo.Groomers", "GroomerID", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Owners", "OwnerID", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.PetOwners", "Owner_OwnerID", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Groomers", "GroomerID");
            AddPrimaryKey("dbo.Owners", "OwnerID");
            AddPrimaryKey("dbo.PetOwners", new[] { "Pet_PetID", "Owner_OwnerID" });
            CreateIndex("dbo.GroomBookings", "GroomerID");
            CreateIndex("dbo.GroomBookings", "OwnerID");
            CreateIndex("dbo.Groomers", "GroomerID");
            CreateIndex("dbo.Owners", "OwnerID");
            CreateIndex("dbo.PetOwners", "Owner_OwnerID");
            AddForeignKey("dbo.Owners", "OwnerID", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Groomers", "GroomerID", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.GroomBookings", "GroomerID", "dbo.Groomers", "GroomerID");
            AddForeignKey("dbo.GroomBookings", "OwnerID", "dbo.Owners", "OwnerID");
            AddForeignKey("dbo.PetOwners", "Owner_OwnerID", "dbo.Owners", "OwnerID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PetOwners", "Owner_OwnerID", "dbo.Owners");
            DropForeignKey("dbo.GroomBookings", "OwnerID", "dbo.Owners");
            DropForeignKey("dbo.GroomBookings", "GroomerID", "dbo.Groomers");
            DropForeignKey("dbo.Groomers", "GroomerID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Owners", "OwnerID", "dbo.AspNetUsers");
            DropIndex("dbo.PetOwners", new[] { "Owner_OwnerID" });
            DropIndex("dbo.Owners", new[] { "OwnerID" });
            DropIndex("dbo.Groomers", new[] { "GroomerID" });
            DropIndex("dbo.GroomBookings", new[] { "OwnerID" });
            DropIndex("dbo.GroomBookings", new[] { "GroomerID" });
            DropPrimaryKey("dbo.PetOwners");
            DropPrimaryKey("dbo.Owners");
            DropPrimaryKey("dbo.Groomers");
            AlterColumn("dbo.PetOwners", "Owner_OwnerID", c => c.Int(nullable: false));
            AlterColumn("dbo.Owners", "OwnerID", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Groomers", "GroomerID", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.GroomBookings", "OwnerID", c => c.Int(nullable: false));
            AlterColumn("dbo.GroomBookings", "GroomerID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.PetOwners", new[] { "Pet_PetID", "Owner_OwnerID" });
            AddPrimaryKey("dbo.Owners", "OwnerID");
            AddPrimaryKey("dbo.Groomers", "GroomerID");
            CreateIndex("dbo.PetOwners", "Owner_OwnerID");
            CreateIndex("dbo.GroomBookings", "OwnerID");
            CreateIndex("dbo.GroomBookings", "GroomerID");
            AddForeignKey("dbo.PetOwners", "Owner_OwnerID", "dbo.Owners", "OwnerID", cascadeDelete: true);
            AddForeignKey("dbo.GroomBookings", "OwnerID", "dbo.Owners", "OwnerID", cascadeDelete: true);
            AddForeignKey("dbo.GroomBookings", "GroomerID", "dbo.Groomers", "GroomerID", cascadeDelete: true);
        }
    }
}
