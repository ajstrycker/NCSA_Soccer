namespace NCSA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changestoplayerandteammodel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Teams", "TeamId", "dbo.Players");
            DropIndex("dbo.Teams", new[] { "TeamId" });
            DropPrimaryKey("dbo.Teams");
            AddColumn("dbo.Players", "Gender", c => c.String());
            AddColumn("dbo.Players", "ShirtSize", c => c.String());
            AddColumn("dbo.Teams", "GenderOfTeam", c => c.String());
            AddColumn("dbo.Teams", "Coach1", c => c.String());
            AddColumn("dbo.Teams", "Coach2", c => c.String());
            AlterColumn("dbo.Teams", "TeamId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Teams", "TeamId");
            DropColumn("dbo.Teams", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Teams", "Id", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.Teams");
            AlterColumn("dbo.Teams", "TeamId", c => c.Int(nullable: false));
            DropColumn("dbo.Teams", "Coach2");
            DropColumn("dbo.Teams", "Coach1");
            DropColumn("dbo.Teams", "GenderOfTeam");
            DropColumn("dbo.Players", "ShirtSize");
            DropColumn("dbo.Players", "Gender");
            AddPrimaryKey("dbo.Teams", "Id");
            CreateIndex("dbo.Teams", "TeamId");
            AddForeignKey("dbo.Teams", "TeamId", "dbo.Players", "ID", cascadeDelete: true);
        }
    }
}
