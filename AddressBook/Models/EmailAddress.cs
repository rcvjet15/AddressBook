using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AddressBook.Models
{
    public class EmailAddress : EntityBaseModel
    {
        [StringLength(30)]
        [Display(Name = "Email")]
        [RegularExpression(@"^[A-Za-z]+[A-Za-z0-9\!\#\$\%\&\'\*\+\-\/\=\?\^_\`\{\|\}\~\.]*@[A-Za-z0-9\.\-]+[\.][a-z]+$", ErrorMessage = "Invalid Email address format.")]
        public string Address { get; set; }

        [StringLength(10)]
        [Display(Name = "Email Address Type")]
        public string EmailAddressType { get; set; }
        
        public bool? IsDefault { get; set; }

        // Foreign key
        public int ContactID { get; set; }

        // Navigation property
        public Contact Contact { get; set; }
    }
}