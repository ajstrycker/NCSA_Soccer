namespace NCSA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingImagePathtoteam : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Teams", "ImagePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Teams", "ImagePath");
        }
    }
}
