namespace AddressBook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveGroupTypeFromGroupModel : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Group", "GroupType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Group", "GroupType", c => c.String(maxLength: 10));
        }
    }
}
