namespace NCSA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingSchedulesandLocations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StreetName = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Zip = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        HomeTeamID = c.Int(nullable: false),
                        AwayTeam2ID = c.Int(nullable: false),
                        GameDateTime = c.DateTime(nullable: false),
                        CenterRef = c.String(),
                        AR1 = c.String(),
                        AR2 = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Teams", "LocationId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Teams", "LocationId");
            DropTable("dbo.Schedules");
            DropTable("dbo.Locations");
        }
    }
}
