namespace NCSA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addtownnametoteam : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Teams", "TownName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Teams", "TownName");
        }
    }
}
