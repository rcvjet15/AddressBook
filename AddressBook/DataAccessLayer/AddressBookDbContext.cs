using AddressBook.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.IO;
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

        /// <summary>
        /// Wrapper for SaveChanges that is used for adding Validation meesages to the generated excpetion
        /// for easier error validation reading in database migration.
        /// </summary>
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (DbEntityValidationResult failure in ex.EntityValidationErrors)
                {
                    sb.Append($"{failure.Entry.Entity.GetType()} failed validation\n");

                    foreach (DbValidationError error in failure.ValidationErrors)
                    {
                        sb.Append($"- {error.PropertyName} : {error.ErrorMessage}");
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException("Entity Validation Failed - errors follow:\n" + sb.ToString(), ex); // Add the original exception as inner exception                
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Override so that when tables from Identity entitities are generated, there won't be prefix in table names "AspNet"
            modelBuilder.Entity<ApplicationUser>().ToTable("User").HasKey(x => x.Id);
            modelBuilder.Entity<UserRole>().ToTable("UserRole");
            modelBuilder.Entity<UserLogin>().ToTable("UserLogin");
            modelBuilder.Entity<UserClaim>().ToTable("UserClaim");
            modelBuilder.Entity<Role>().ToTable("Role");
        }
    }
}
