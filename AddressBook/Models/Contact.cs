using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AddressBook.Models
{
    public class Contact : EntityBaseModel
    {
        public Contact()
        {
            Groups = new List<Group>();
            Addresses = new List<Address>();
            PhoneNumbers = new List<PhoneNumber>();
            EmailAddresses = new List<EmailAddress>();
        }       

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "Last Name")]
        public DateTime? Birthdate { get; set; }

        [Display(Name = "Note")]
        public string Note { get; set; }

        [Display(Name = "Relationship")]
        public string Relationship { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Organization")]
        public string Organization { get; set; }

        public string ProfilePicPath { get; set; }

        [NotMapped]
        [ScaffoldColumn(false)]
        public string FullName { get => $"{LastName} {FirstName}"; }

        // Foreign key
        public int ApplicationUserID { get; set; }

        // Navigation property
        public ApplicationUser ApplicationUser { get; set; }

        // Navigation property
        public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }

        // Navigation property
        public virtual ICollection<EmailAddress> EmailAddresses { get; set; }

        // Navigation property
        public virtual ICollection<Address> Addresses { get; set; }

        // Navigation property - many-to-many
        public virtual ICollection<Group> Groups { get; set; }
    }
}