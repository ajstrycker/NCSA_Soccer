namespace NCSA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addingcontactmessagemodel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContactMessages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TimeOfMessage = c.DateTime(nullable: false),
                        Name = c.String(),
                        Email = c.String(),
                        Message = c.String(),
                        IsRead = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ContactMessages");
        }
    }
}
