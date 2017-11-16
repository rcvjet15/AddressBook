﻿using System;
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
        }

        public string FirstName { get; set; }
              
        public string LastName { get; set; }
        
        public string ProfilePicPath { get; set; }
        
        public string Gender { get; set; }
        
        public DateTime? Birthdate { get; set; }
                
        public string Note { get; set; }
                
        public string Relationship { get; set; }
                
        public string Title { get; set; }
                
        public string Organization { get; set; }

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