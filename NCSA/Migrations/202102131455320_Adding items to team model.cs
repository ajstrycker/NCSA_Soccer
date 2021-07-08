namespace NCSA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addingitemstoteammodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Teams", "Coach1Phone", c => c.String());
            AddColumn("dbo.Teams", "PracticeTimes", c => c.String());
            AddColumn("dbo.Teams", "WhatToBring", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Teams", "WhatToBring");
            DropColumn("dbo.Teams", "PracticeTimes");
            DropColumn("dbo.Teams", "Coach1Phone");
        }
    }
}
