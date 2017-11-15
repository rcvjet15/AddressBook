using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AddressBook.Helpers
{
    /// <summary>
    /// Static class that will hold app global parameters.
    /// </summary>
    public static class Params
    {
        public const string DefaultConnectionName = "DefaultConnection";
        
        public const string DefaultProfilePicPath = "/Content/ProfilePictures/person_profile.png";

        public static string[] GenderList = new string[] { "Male", "Female" };

        public static string[] AddressTypeList = new string[] { "Home", "Work", "Other" };

        public static string[] NumberTypeList = new string[] { "Home", "Work", "Other" };

        public static string[] EmailAddressTypeList = new string[] { "Home", "Work", "Other" };

    }
}