namespace NCSA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addingrequiredfieldsforteam : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Teams", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Teams", "TownName", c => c.String(nullable: false));
            AlterColumn("dbo.Teams", "GenderOfTeam", c => c.String(nullable: false));
            AlterColumn("dbo.Teams", "Coach1", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Teams", "Coach1", c => c.String());
            AlterColumn("dbo.Teams", "GenderOfTeam", c => c.String());
            AlterColumn("dbo.Teams", "TownName", c => c.String());
            AlterColumn("dbo.Teams", "Description", c => c.String());
        }
    }
}
