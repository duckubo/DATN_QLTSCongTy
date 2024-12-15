namespace QuanLyTaiSan_UserManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserAuthorization : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                    "dbo.UserAuthorization",
                    c => new
                    {
                        Id = c.Int(nullable: false),
                        UserId = c.String(nullable: true, maxLength: 50),
                        ControllerName = c.String(nullable: true, maxLength: 50),
                        ActionName = c.String(nullable: true, maxLength: 50)
                    })
                .PrimaryKey(t => t.Id);

        }
        
        public override void Down()
        {
            DropTable("dbo.UserAuthorization");
        }
    }
}
