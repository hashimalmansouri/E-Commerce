namespace E_Commerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletePropToUsersTB : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Created");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Created", c => c.DateTime(nullable: false));
        }
    }
}
