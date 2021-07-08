namespace NCSA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addlocationlatlng : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Locations", "Latitude", c => c.String());
            AddColumn("dbo.Locations", "Longitude", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Locations", "Longitude");
            DropColumn("dbo.Locations", "Latitude");
        }
    }
}
