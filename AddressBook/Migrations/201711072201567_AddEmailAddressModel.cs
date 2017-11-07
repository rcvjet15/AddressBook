namespace AddressBook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmailAddressModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmailAddress",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Address = c.String(nullable: false, maxLength: 30),
                        AddressType = c.String(maxLength: 10),
                        Default = c.Boolean(nullable: false),
                        ContactID = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        ModifiedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Contact", t => t.ContactID, cascadeDelete: true)
                .Index(t => t.ID)
                .Index(t => t.ContactID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmailAddress", "ContactID", "dbo.Contact");
            DropIndex("dbo.EmailAddress", new[] { "ContactID" });
            DropIndex("dbo.EmailAddress", new[] { "ID" });
            DropTable("dbo.EmailAddress");
        }
    }
}
