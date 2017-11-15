using AddressBook.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AddressBook.ViewModels
{
    public class ContactIndexViewModel
    {
        public int ID { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string ProfileImageBase64 { get; set; }

        public string ProfileImagePath { get; set; }

        public IEnumerable<string> Groups { get; set; }
    }

    public class ContactCreateViewModel
    {
        public ContactCreateViewModel()
        {
            Address = new Address();
            PhoneNumbers = new List<PhoneNumber>();
            Emails = new List<EmailAddress>();
        }

        [Required]
        [StringLength(20)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        
        [Display(Name = "Birthdate")]
        public string Birthdate { get; set; } // Set Birthdate as string type because dd/mm/yyyy format is not acceptable in jquery.validate

        [Display(Name = "Gender")]
        public string Gender { get; set; }

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

        public string ProfileImagePath { get; set; }
                
        public Address Address { get; set; }
                
        public List<PhoneNumber> PhoneNumbers { get; set; }

        public List<EmailAddress> Emails { get; set; }
                
        public List<Group> Groups { get; set; }        
    }
}