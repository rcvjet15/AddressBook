namespace AddressBook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeColumnDefaultToIsDefault : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmailAddress", "IsDefault", c => c.Boolean());
            AddColumn("dbo.PhoneNumber", "IsDefault", c => c.Boolean());
            DropColumn("dbo.EmailAddress", "Default");
            DropColumn("dbo.PhoneNumber", "Default");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PhoneNumber", "Default", c => c.Boolean(nullable: false));
            AddColumn("dbo.EmailAddress", "Default", c => c.Boolean(nullable: false));
            DropColumn("dbo.PhoneNumber", "IsDefault");
            DropColumn("dbo.EmailAddress", "IsDefault");
        }
    }
}
