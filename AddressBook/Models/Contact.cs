using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AddressBook.Models
{
    public class Contact : EntityBaseClass
    {
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
        [Display(Name = "Birthday")]
        public DateTime Birthday { get; set; }

        [Display(Name = "Note")]
        public string Note { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Organization")]
        public string Organization { get; set; }
    }
}