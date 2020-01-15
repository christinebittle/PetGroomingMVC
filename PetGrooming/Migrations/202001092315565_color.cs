namespace PetGrooming.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class color : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pets", "color", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pets", "color");
        }
    }
}
