namespace PetGrooming.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pethaspicfield : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pets", "HasPic", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pets", "HasPic");
        }
    }
}
