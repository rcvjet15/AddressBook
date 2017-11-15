using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AddressBook.Models
{
    public class Address : EntityBaseModel
    {
        [StringLength(30)]
        [Display(Name = "Street Name")]
        public string Street { get; set; }

        [StringLength(8)]
        [Display(Name = "House Number")]
        public string HouseNumber { get; set; }

        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Postal code can contain only numbers.")]
        [StringLength(10)]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [StringLength(20)]
        [Display(Name = "City")]
        public string City { get; set; }

        [StringLength(20)]
        [Display(Name = "State")]
        public string State { get; set; }

        [StringLength(10)]
        [Display(Name = "Address Type")]
        public string AddressType { get; set; }

        public bool Default { get; set; }

        [NotMapped]
        [ScaffoldColumn(false)]
        public string FullAddress { get => $"{Street} {HouseNumber}, {City} {PostalCode}, {State}"; }

        [NotMapped]
        [ScaffoldColumn(false)]
        public string FullAddressWithoutState { get => $"{Street} {HouseNumber}, {City} {PostalCode}"; }

        // Foreign key
        public int ContactID { get; set; }

        // Navigation property
        public Contact Contact { get; set; }
    }
}