using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AddressBook.Models
{
    public class EmailAddress : EntityBaseModel
    {
        [Display(Name = "Email")]
        [RegularExpression(@"^[A-Za-z]+[A-Za-z0-9\!\#\$\%\&\'\*\+\-\/\=\?\^\_\`\{\|\}\~\.]*@[A-Za-z0-9\.\-]+[\.][a-z]+$", ErrorMessage = "Invalid Email address format.")]
        public string Address { get; set; }

        public string AddressType { get; set; }

        [Display(Name = "Primary")]
        public bool Default { get; set; }

        // Foreign key
        public int ContactID { get; set; }

        // Navigation property
        public Contact Contact { get; set; }
    }
}