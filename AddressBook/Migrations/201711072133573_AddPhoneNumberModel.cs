namespace AddressBook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPhoneNumberModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PhoneNumber",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Number = c.String(nullable: false, maxLength: 30),
                        NumberType = c.String(maxLength: 10),
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
            DropForeignKey("dbo.PhoneNumber", "ContactID", "dbo.Contact");
            DropIndex("dbo.PhoneNumber", new[] { "ContactID" });
            DropIndex("dbo.PhoneNumber", new[] { "ID" });
            DropTable("dbo.PhoneNumber");
        }
    }
}
