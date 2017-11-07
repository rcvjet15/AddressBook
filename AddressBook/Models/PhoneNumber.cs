using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AddressBook.Models
{
    public class PhoneNumber : EntityBaseModel
    {
        [Display(Name = "Number")]
        public string Number { get; set; }

        [Display(Name = "Number Type")]
        public string NumberType { get; set; }
        
        /// <summary>
        /// Indicates if phonenubmer entity is default number for contact 
        /// because contact and phonenumber entity have one-to-many relationship.
        /// </summary>
        [Display(Name = "Primary")]
        public bool Default { get; set; }

        // Foreign key
        public int ContactID { get; set; }

        // Navigation property
        public Contact Contact { get; set; }
    }
}