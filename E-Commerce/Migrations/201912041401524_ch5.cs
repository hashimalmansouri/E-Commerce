namespace E_Commerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ch5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "TempQuantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "TempQuantity", c => c.Int());
        }
    }
}
