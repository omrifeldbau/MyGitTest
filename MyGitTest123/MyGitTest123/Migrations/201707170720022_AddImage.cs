namespace MyGitTest123.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "Image", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "Image");
        }
    }
}
