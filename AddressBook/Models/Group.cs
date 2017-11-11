using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AddressBook.Models
{
    public class Group : EntityBaseModel
    {
        public Group()
        {
            Contacts = new List<Contact>();
        }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }        

        // Navigation property - many-to-many
        public virtual ICollection<Contact> Contacts { get; set; }
    }
}