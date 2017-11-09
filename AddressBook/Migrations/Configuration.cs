namespace AddressBook.Migrations
{
    using AddressBook.Models;
    using Microsoft.AspNet.Identity;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Text;

    internal sealed class Configuration : DbMigrationsConfiguration<AddressBook.DataAccessLayer.AddressBookDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AddressBook.DataAccessLayer.AddressBookDbContext context)
        {
            AddDefaultUser(context);

            context.SaveChanges();
        }
        
        /// <summary>
        /// Adds default user. This user must be removed for production.
        /// </summary>
        /// <param name="context"></param>
        private void AddDefaultUser(AddressBook.DataAccessLayer.AddressBookDbContext context)
        {
            if (!context.Roles.Any(r => r.Name == "administrator"))
            {
                CustomRoleStore store = new CustomRoleStore(context);
                CustomRoleManager manager = new CustomRoleManager(store);
                Role role = new Role { Name = "administrator" };

                manager.Create(role);
            }

            if (!context.Users.Any(u => u.UserName == "admin"))
            {
                CustomUserStore store = new CustomUserStore(context);
                CustomUserManager manager = new CustomUserManager(store);
                ApplicationUser user = new ApplicationUser()
                {
                    FirstName = "admin",
                    LastName = "admin@test.com",
                    UserName = "admin",
                    BirthDate = DateTime.Today,
                    Email = "admin@test.com",
                    CreatedAt = DateTime.Now
                };

                manager.Create(user, "Lozinka1$");
                manager.AddToRole(user.Id, "administrator");
            }
        }
    }
}
