using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AddressBook.Models
{
    public class PhoneNumber : EntityBaseModel
    {
        [StringLength(30)]
        [Display(Name = "Number")]
        public string Number { get; set; }

        [StringLength(10)]
        [Display(Name = "Number Type")]
        public string NumberType { get; set; }
        
        /// <summary>
        /// Indicates if phonenubmer entity is default number for contact 
        /// because contact and phone number entity have one-to-many relationship.
        /// </summary>
        public bool? IsDefault { get; set; }

        // Foreign key
        public int ContactID { get; set; }

        // Navigation property
        public Contact Contact { get; set; }
    }
}