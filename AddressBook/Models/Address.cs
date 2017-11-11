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
        public string Street { get; set; }

        public string HouseNumber { get; set; }
        
        public string PostalCode { get; set; }
        
        public string City { get; set; }
                
        public string State { get; set; }
                
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