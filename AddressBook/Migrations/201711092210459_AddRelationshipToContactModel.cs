namespace AddressBook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRelationshipToContactModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contact", "Relationship", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contact", "Relationship");
        }
    }
}
