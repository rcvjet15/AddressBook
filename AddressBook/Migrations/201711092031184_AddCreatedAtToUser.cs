namespace AddressBook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreatedAtToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contact", "Birthdate", c => c.DateTime());
            AddColumn("dbo.User", "CreatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.User", "FirstName", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.User", "LastName", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.User", "BirthDate", c => c.DateTime());
            DropColumn("dbo.Contact", "Birthday");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Contact", "Birthday", c => c.DateTime(nullable: false));
            AlterColumn("dbo.User", "BirthDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.User", "LastName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.User", "FirstName", c => c.String(nullable: false, maxLength: 30));
            DropColumn("dbo.User", "CreatedAt");
            DropColumn("dbo.Contact", "Birthdate");
        }
    }
}
