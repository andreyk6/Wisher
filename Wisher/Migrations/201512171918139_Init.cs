namespace Wisher.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationUserCategoryInfoes", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserCategoryInfoes", "CategoryInfo_Id", "dbo.CategoryInfoes");
            DropIndex("dbo.ApplicationUserCategoryInfoes", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserCategoryInfoes", new[] { "CategoryInfo_Id" });
            AddColumn("dbo.AspNetUsers", "CatsToChose_SerializedValue", c => c.String());
            AddColumn("dbo.AspNetUsers", "SellectedCats_SerializedValue", c => c.String());
            DropTable("dbo.ApplicationUserCategoryInfoes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ApplicationUserCategoryInfoes",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        CategoryInfo_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.CategoryInfo_Id });
            
            DropColumn("dbo.AspNetUsers", "SellectedCats_SerializedValue");
            DropColumn("dbo.AspNetUsers", "CatsToChose_SerializedValue");
            CreateIndex("dbo.ApplicationUserCategoryInfoes", "CategoryInfo_Id");
            CreateIndex("dbo.ApplicationUserCategoryInfoes", "ApplicationUser_Id");
            AddForeignKey("dbo.ApplicationUserCategoryInfoes", "CategoryInfo_Id", "dbo.CategoryInfoes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ApplicationUserCategoryInfoes", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
