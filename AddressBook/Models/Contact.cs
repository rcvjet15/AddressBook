using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AddressBook.Models
{
    public class Contact : EntityBaseModel
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [ScaffoldColumn(false)]
        public string ProfilePicPath { get; set; }

        [StringLength(6)]
        public string Gender { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d/M/yyyy}")]
        [Display(Name = "Birthday")]
        public DateTime Birthday { get; set; }

        [Display(Name = "Note")]
        public string Note { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Organization")]
        public string Organization { get; set; }
        
        // Foreign key
        public int ApplicationUserID { get; set; }

        // Navigation property
        public ApplicationUser ApplicationUser { get; set; }

        // Navigation property
        public virtual ICollection<PhoneNumber> PhoneNumber { get; set; }

        // Navigation property
        public virtual ICollection<EmailAddress> EmailAddress { get; set; }

        // Navigation property
        public virtual ICollection<Address> Address { get; set; }
    }
}