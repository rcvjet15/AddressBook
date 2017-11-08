namespace AddressBook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGroupModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Group",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        GroupType = c.String(maxLength: 10),
                        CreatedAt = c.DateTime(nullable: false),
                        ModifiedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.ID);
            
            CreateTable(
                "dbo.ContactGroup",
                c => new
                    {
                        ContactID = c.Int(nullable: false),
                        GroupID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ContactID, t.GroupID })
                .ForeignKey("dbo.Contact", t => t.ContactID, cascadeDelete: true)
                .ForeignKey("dbo.Group", t => t.GroupID, cascadeDelete: true)
                .Index(t => t.ContactID)
                .Index(t => t.GroupID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ContactGroup", "GroupID", "dbo.Group");
            DropForeignKey("dbo.ContactGroup", "ContactID", "dbo.Contact");
            DropIndex("dbo.ContactGroup", new[] { "GroupID" });
            DropIndex("dbo.ContactGroup", new[] { "ContactID" });
            DropIndex("dbo.Group", new[] { "ID" });
            DropTable("dbo.ContactGroup");
            DropTable("dbo.Group");
        }
    }
}
