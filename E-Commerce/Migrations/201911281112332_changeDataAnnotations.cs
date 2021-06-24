namespace E_Commerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeDataAnnotations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "ProductName", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Products", "ImageUrl", c => c.String(nullable: false, maxLength: 1024));
            AlterColumn("dbo.Products", "Quantity", c => c.Int(nullable: false));
            AlterColumn("dbo.Genres", "GenreName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Genres", "GenreName", c => c.String());
            AlterColumn("dbo.Products", "Quantity", c => c.Int());
            AlterColumn("dbo.Products", "ImageUrl", c => c.String(maxLength: 1024));
            AlterColumn("dbo.Products", "ProductName", c => c.String(maxLength: 200));
        }
    }
}
