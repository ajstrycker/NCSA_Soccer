namespace NCSA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addinglogtable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Controller = c.String(),
                        Message = c.String(),
                        Stacktrace = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Logs");
        }
    }
}
