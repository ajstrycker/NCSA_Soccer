namespace NCSA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changetoplayers : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Players", "BirthDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Players", "BirthDate", c => c.Int(nullable: false));
        }
    }
}
