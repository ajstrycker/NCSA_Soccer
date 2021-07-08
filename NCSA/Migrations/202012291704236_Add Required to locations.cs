namespace NCSA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequiredtolocations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Locations", "StreetName", c => c.String(nullable: false));
            AlterColumn("dbo.Locations", "City", c => c.String(nullable: false));
            AlterColumn("dbo.Locations", "State", c => c.String(nullable: false));
            AlterColumn("dbo.Locations", "Zip", c => c.String(nullable: false));
            AlterColumn("dbo.Locations", "Latitude", c => c.String(nullable: false));
            AlterColumn("dbo.Locations", "Longitude", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Locations", "Longitude", c => c.String());
            AlterColumn("dbo.Locations", "Latitude", c => c.String());
            AlterColumn("dbo.Locations", "Zip", c => c.String());
            AlterColumn("dbo.Locations", "State", c => c.String());
            AlterColumn("dbo.Locations", "City", c => c.String());
            AlterColumn("dbo.Locations", "StreetName", c => c.String());
        }
    }
}
