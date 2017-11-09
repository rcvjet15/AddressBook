using AddressBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AddressBook.ViewModels
{
    public class ContactListViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public PhoneNumber PhoneNumber { get; set; }

        public EmailAddress Email { get; set; }

        public Address Address { get; set; }

        public List<Group> Groups { get; set; }
    }

    public class ContactCreateViewModel
    { 
        
    }
}