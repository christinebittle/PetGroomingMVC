namespace PetGrooming.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Core_Concepts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Species",
                c => new
                    {
                        SpeciesID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.SpeciesID);
            
            AddColumn("dbo.Pets", "PetName", c => c.String());
            AddColumn("dbo.Pets", "Notes", c => c.String());
            AddColumn("dbo.Pets", "SpeciesID", c => c.Int(nullable: false));
            CreateIndex("dbo.Pets", "SpeciesID");
            AddForeignKey("dbo.Pets", "SpeciesID", "dbo.Species", "SpeciesID", cascadeDelete: true);
            DropColumn("dbo.Pets", "Name");
            DropColumn("dbo.Pets", "Species");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pets", "Species", c => c.String());
            AddColumn("dbo.Pets", "Name", c => c.String());
            DropForeignKey("dbo.Pets", "SpeciesID", "dbo.Species");
            DropIndex("dbo.Pets", new[] { "SpeciesID" });
            DropColumn("dbo.Pets", "SpeciesID");
            DropColumn("dbo.Pets", "Notes");
            DropColumn("dbo.Pets", "PetName");
            DropTable("dbo.Species");
        }
    }
}
