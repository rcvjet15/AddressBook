namespace AddressBook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddContactModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contact",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 20),
                        LastName = c.String(nullable: false, maxLength: 30),
                        ProfilePicPath = c.String(),
                        Gender = c.String(maxLength: 6),
                        Birthday = c.DateTime(nullable: false),
                        Note = c.String(),
                        Title = c.String(maxLength: 30),
                        Organization = c.String(maxLength: 30),
                        ApplicationUserID = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        ModifiedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.ApplicationUserID, cascadeDelete: true)
                .Index(t => t.ID)
                .Index(t => t.ApplicationUserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contact", "ApplicationUserID", "dbo.User");
            DropIndex("dbo.Contact", new[] { "ApplicationUserID" });
            DropIndex("dbo.Contact", new[] { "ID" });
            DropTable("dbo.Contact");
        }
    }
}
