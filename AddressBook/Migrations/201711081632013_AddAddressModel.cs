namespace AddressBook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAddressModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Address",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Street = c.String(maxLength: 30),
                        HouseNumber = c.String(maxLength: 8),
                        PostalCode = c.String(maxLength: 10),
                        City = c.String(maxLength: 20),
                        State = c.String(maxLength: 20),
                        AddressType = c.String(maxLength: 10),
                        ContactID = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        ModifiedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Contact", t => t.ContactID, cascadeDelete: true)
                .Index(t => t.ID)
                .Index(t => t.ContactID);
            
            AddColumn("dbo.EmailAddress", "EmailAddressType", c => c.String(maxLength: 10));
            DropColumn("dbo.EmailAddress", "AddressType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EmailAddress", "AddressType", c => c.String(maxLength: 10));
            DropForeignKey("dbo.Address", "ContactID", "dbo.Contact");
            DropIndex("dbo.Address", new[] { "ContactID" });
            DropIndex("dbo.Address", new[] { "ID" });
            DropColumn("dbo.EmailAddress", "EmailAddressType");
            DropTable("dbo.Address");
        }
    }
}
