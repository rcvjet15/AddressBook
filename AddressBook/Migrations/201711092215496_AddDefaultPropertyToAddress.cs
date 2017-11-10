namespace AddressBook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDefaultPropertyToAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Address", "Default", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Address", "Default");
        }
    }
}
