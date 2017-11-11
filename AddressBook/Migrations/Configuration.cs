namespace AddressBook.Migrations
{
    using AddressBook.Helpers;
    using AddressBook.Models;
    using Microsoft.AspNet.Identity;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Text;
    using AddressBook.DataAccessLayer;

    internal sealed class Configuration : DbMigrationsConfiguration<AddressBook.DataAccessLayer.AddressBookDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AddressBook.DataAccessLayer.AddressBookDbContext context)
        {
            AddDefaultUser(context);
            AddContacts(context);
            AddGroups(context);

            // Save at the end
            context.SaveChanges();
        }

        private void AddGroups(DataAccessLayer.AddressBookDbContext context)
        {
            var groups = new List<Group>
            {
                new Group
                {
                    Name = "Work"
                },
                new Group
                {
                    Name = "Training"
                },
                new Group
                {
                    Name = "Cooking"
                },
                new Group
                {
                    Name = "Friends"
                },
                new Group
                {
                    Name = "Guests"
                },
                new Group
                {
                    Name = "Family"
                },
            };

            groups.ForEach(g => context.Groups.AddOrUpdate(x => x.Name, g));            
        }

        private void AddContacts(AddressBook.DataAccessLayer.AddressBookDbContext context)
        {
            int userId = context.Users
                .Single(u => u.UserName == "admin@test.com")
                .Id;

            var contacts = new List<Contact>()
            {
                new Contact()
                {
                    FirstName = "Ivan",
                    LastName = "Perić",
                    Gender = "Male",
                    Birthdate = DateTime.Now,
                    Title = "Colleague",
                    Organization = "Google",
                    ProfilePicPath = Params.DefaultProfilePicPath,
                    ApplicationUserID = userId,
                },
                new Contact()
                {
                    FirstName = "Marija",
                    LastName = "Ivušić",
                    Gender = "Female",
                    Birthdate = DateTime.Now,
                    Relationship = "Colleague",
                    Organization = "Facebook",
                    ProfilePicPath = Params.DefaultProfilePicPath,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    ApplicationUserID = userId,
                },
                new Contact()
                {
                    FirstName = "Kristina",
                    LastName = "Gorić",
                    Gender = "Female",
                    Birthdate = DateTime.Now,
                    Title = "Friend",
                    ProfilePicPath = Params.DefaultProfilePicPath,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    ApplicationUserID = userId,
                },
                new Contact()
                {
                    FirstName = "Domagoj",
                    LastName = "Marinić",
                    Gender = "Male",
                    Birthdate = DateTime.Now,
                    Title = "Boss",
                    Organization = "Google",
                    ProfilePicPath = Params.DefaultProfilePicPath,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    ApplicationUserID = userId,
                },
                new Contact()
                {
                    FirstName = "Marin",
                    LastName = "Dorić",
                    Gender = "Male",
                    Birthdate = DateTime.Now,
                    Relationship = "Friend",
                    Title = "Driver",
                    Organization = "Taxi",
                    ProfilePicPath = Params.DefaultProfilePicPath,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    ApplicationUserID = userId,
                },
                new Contact()
                {
                    FirstName = "Mom",
                    LastName = "Perić",
                    Gender = "Male",
                    Birthdate = DateTime.Now,
                    Relationship = "Family",
                    ProfilePicPath = Params.DefaultProfilePicPath,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    ApplicationUserID = userId,
                },
                new Contact()
                {
                    FirstName = "Home",
                    LastName = "Home",
                    ProfilePicPath = Params.DefaultProfilePicPath,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    ApplicationUserID = userId,
                },
            };

            // Add default contacts, but only those whose LastName is not in DB
            contacts.ForEach(c => context.Contacts.AddOrUpdate(x => x.LastName, c));
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

            if (!context.Users.Any(u => u.UserName == "admin@test.com"))
            {
                CustomUserStore store = new CustomUserStore(context);
                CustomUserManager manager = new CustomUserManager(store);
                ApplicationUser user = new ApplicationUser()
                {
                    FirstName = "admin",
                    LastName = "admin",
                    UserName = "admin@test.com",
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
