using AddressBook.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Text;

namespace AddressBook.DataAccessLayer
{
    /// <summary>
    /// Extended from IdentityDbContext so that one DB context holds all entitites and application user entity.
    /// </summary>
    public sealed class AddressBookDbContext : IdentityDbContext<ApplicationUser, Role, int, UserLogin, UserRole, UserClaim>
    {
        public AddressBookDbContext()
             : base("UsersConnection")
        {
        }

        public static AddressBookDbContext Create()
        {
            return new AddressBookDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // There is no need to keep contacts after app user is deleted 
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            // Override so that when tables from Identity entitities are generated, there won't be prefix in table names "AspNet"
            modelBuilder.Entity<ApplicationUser>().ToTable("Users").HasKey(x => x.Id);
            modelBuilder.Entity<UserRole>().ToTable("UserRoles");
            modelBuilder.Entity<UserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<UserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<Role>().ToTable("Roles");
        }
    }
}
