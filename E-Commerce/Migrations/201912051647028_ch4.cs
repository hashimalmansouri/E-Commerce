namespace E_Commerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ch4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SlideShows",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImageUrl = c.String(),
                        ImageAlt = c.String(),
                        Title = c.String(),
                        Caption = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SlideShows");
        }
    }
}
