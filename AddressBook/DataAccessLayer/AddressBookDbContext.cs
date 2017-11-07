using AddressBook.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
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
                
        public override int SaveChanges()
        {
            try
            {
                foreach (var entry in ChangeTracker.Entries())
                {
                    if (entry.Entity is EntityBaseModel)
                    {
                        EntityBaseModel entity = (EntityBaseModel)entry.Entity;

                        switch (entry.State)
                        {
                            case EntityState.Added:
                                entity.CreatedAt = DateTime.Now;
                                entity.ModifiedAt = DateTime.Now;
                                break;                         
                            case EntityState.Modified:
                                entity.ModifiedAt = DateTime.Now;
                                break;
                        }
                    }
                }

                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Wrapper for SaveChanges that is used for adding Validation meesages to the generated excpetion
                // for easier error validation reading in database migration
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

            // Link User and Contact
            modelBuilder.Entity<ApplicationUser>()
                .ToTable("User")
                .HasKey(x => x.Id)
                .HasMany(x => x.Contacts)
                .WithRequired(x => x.ApplicationUser) // Not null
                .HasForeignKey<int>(x => x.ApplicationUserID)
                .WillCascadeOnDelete();

            modelBuilder.Entity<UserRole>().ToTable("UserRole");
            modelBuilder.Entity<UserLogin>().ToTable("UserLogin");
            modelBuilder.Entity<UserClaim>().ToTable("UserClaim");
            modelBuilder.Entity<Role>().ToTable("Role");

            // Contact model
            // TPC - mapping (Table-Per-Concrete)               
            modelBuilder.Entity<Contact>().Map(u =>
            {
                // This table will have columns with inherited and its own properties. On hover for more information.
                u.MapInheritedProperties();
                u.ToTable("Contact");
            }).HasKey(c => c.ID)                
                .Property(c => c.ID)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)
                    .HasColumnAnnotation("Index", new IndexAnnotation(new System.ComponentModel.DataAnnotations.Schema.IndexAttribute())); // Add index

            modelBuilder.Entity<Contact>()
                .Property(c => c.FirstName)
                    .IsRequired()
                    .HasMaxLength(20);

            modelBuilder.Entity<Contact>()
                .Property(c => c.LastName)
                    .IsRequired()
                    .HasMaxLength(30);
            
            modelBuilder.Entity<Contact>()
               .Property(c => c.Title)
                   .HasMaxLength(30);

            modelBuilder.Entity<Contact>()
               .Property(c => c.Organization)
                   .HasMaxLength(30);

            modelBuilder.Entity<Contact>()
                .Property(c => c.CreatedAt)
                    .IsRequired();

            modelBuilder.Entity<Contact>()
                .Property(c => c.ModifiedAt)
                    .IsRequired();

            // Link Contact and PhoneNumber
            modelBuilder.Entity<Contact>()
                .HasMany(c => c.PhoneNumber)
                .WithRequired(p => p.Contact)
                .HasForeignKey<int>(p => p.ContactID)
                .WillCascadeOnDelete();

            // Link Contact and EmailAddress
            modelBuilder.Entity<Contact>()
                .HasMany(c => c.EmailAddress)
                .WithRequired(e => e.Contact)
                .HasForeignKey<int>(e => e.ContactID)
                .WillCascadeOnDelete();

            // PhoneNumber model
            modelBuilder.Entity<PhoneNumber>().Map(u =>
            {
                u.MapInheritedProperties();
                u.ToTable("PhoneNumber");
            }).HasKey(c => c.ID)
                .Property(c => c.ID)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)
                    .HasColumnAnnotation("Index", new IndexAnnotation(new System.ComponentModel.DataAnnotations.Schema.IndexAttribute())); // Add index

            modelBuilder.Entity<PhoneNumber>()
                .Property(p => p.Number)
                .IsRequired()
                .HasMaxLength(30);

            modelBuilder.Entity<PhoneNumber>()
                .Property(p => p.NumberType)
                .HasMaxLength(10);

            // EmailAddress model
            modelBuilder.Entity<EmailAddress>().Map(u =>
            {
                u.MapInheritedProperties();
                u.ToTable("EmailAddress");
            })
            .HasKey(e => e.ID)
                .Property(e => e.ID)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)
                    .HasColumnAnnotation("Index", new IndexAnnotation(new System.ComponentModel.DataAnnotations.Schema.IndexAttribute())); // Add index

            modelBuilder.Entity<EmailAddress>()
                .Property(p => p.Address)
                .IsRequired()
                .HasMaxLength(30);

            modelBuilder.Entity<EmailAddress>()
                .Property(p => p.AddressType)
                .HasMaxLength(10);
        }
    }
}
