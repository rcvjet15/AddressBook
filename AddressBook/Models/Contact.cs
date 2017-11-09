using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AddressBook.Models
{
    public class Contact : EntityBaseModel
    {
        public Contact()
        {           
        }

        [Required]
        [StringLength(20)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [ScaffoldColumn(false)]
        public string ProfilePicPath { get; set; }

        [StringLength(6)]
        public string Gender { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d/M/yyyy}")]
        [Display(Name = "Birthdate")]
        public DateTime? Birthdate { get; set; }

        [Display(Name = "Note")]
        public string Note { get; set; }

        [Display(Name = "Relationship")]
        public string Relationship { get; set; }

        [StringLength(30)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [StringLength(30)]
        [Display(Name = "Organization")]
        public string Organization { get; set; }
        
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