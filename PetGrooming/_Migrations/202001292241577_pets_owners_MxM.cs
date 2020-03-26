namespace PetGrooming.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pets_owners_MxM : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PetOwners",
                c => new
                    {
                        Pet_PetID = c.Int(nullable: false),
                        Owner_OwnerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Pet_PetID, t.Owner_OwnerID })
                .ForeignKey("dbo.Pets", t => t.Pet_PetID, cascadeDelete: true)
                .ForeignKey("dbo.Owners", t => t.Owner_OwnerID, cascadeDelete: true)
                .Index(t => t.Pet_PetID)
                .Index(t => t.Owner_OwnerID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PetOwners", "Owner_OwnerID", "dbo.Owners");
            DropForeignKey("dbo.PetOwners", "Pet_PetID", "dbo.Pets");
            DropIndex("dbo.PetOwners", new[] { "Owner_OwnerID" });
            DropIndex("dbo.PetOwners", new[] { "Pet_PetID" });
            DropTable("dbo.PetOwners");
        }
    }
}
