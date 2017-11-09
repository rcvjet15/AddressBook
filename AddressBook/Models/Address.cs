using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AddressBook.Models
{
    public class Address : EntityBaseModel
    {
        [StringLength(30)]
        public string Street { get; set; }

        [StringLength(8)]
        public string HouseNumber { get; set; }

        [StringLength(10)]
        public string PostalCode { get; set; }

        [StringLength(20)]
        public string City { get; set; }

        [StringLength(20)]
        public string State { get; set; }

        [StringLength(10)]
        public string AddressType { get; set; }

        // Foreign key
        public int ContactID { get; set; }

        // Navigation property
        public Contact Contact { get; set; }
    }
}