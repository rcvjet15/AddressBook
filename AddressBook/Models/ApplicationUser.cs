using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using AddressBook.DataAccessLayer;

namespace AddressBook.Models
{
    // Inherit from identity user and set that primary key will be type of int (ID), not type of string (username)
    public class ApplicationUser : IdentityUser<int, UserLogin, UserRole, UserClaim>
    {
        public ApplicationUser()
        {
            Contacts = new List<Contact>();
        }

        [Required]
        [StringLength(30)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d/M/yyyy}")]
        public DateTime BirthDate { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Profile Picture")]
        public string ProfilePicturePath { get; set; }
        
        [ScaffoldColumn(false)]
        [Display(Name = "Full Name")]
        public string FullName
        {
            get => String.Format("{0} {1}", FirstName, LastName);
        }

        // Navigation property
        public virtual ICollection<Contact> Contacts { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    // Override so that when tables from Identity entitities are generated, there won't be prefix in table names "AspNet"
    public class UserRole : IdentityUserRole<int> { }
    public class UserLogin : IdentityUserLogin<int> { }
    public class UserClaim : IdentityUserClaim<int> { }
    public class Role : IdentityRole<int, UserRole>
    {
        public Role() { }
        public Role(string name) { Name = name; }
    }

    public class CustomUserStore : UserStore<ApplicationUser, Role, int, UserLogin, UserRole, UserClaim>
    {
        public CustomUserStore(AddressBookDbContext context)
            : base(context)
        {
        }
    }

    public class CustomRoleStore : RoleStore<Role, int, UserRole>
    {
        public CustomRoleStore(AddressBookDbContext context)
            : base(context)
        {
        }
    }

    public class CustomUserManager : UserManager<ApplicationUser, int>
    {
        public CustomUserManager(CustomUserStore store)
            : base(store)
        {

        }
    }

    public class CustomRoleManager : RoleManager<Role, int>
    {
        public CustomRoleManager(CustomRoleStore store)
            : base(store)
        {

        }
    }
}