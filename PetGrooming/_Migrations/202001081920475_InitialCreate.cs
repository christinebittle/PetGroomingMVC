namespace PetGrooming.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pets",
                c => new
                    {
                        PetID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Weight = c.Double(nullable: false),
                        Species = c.String(),
                    })
                .PrimaryKey(t => t.PetID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Pets");
        }
    }
}
