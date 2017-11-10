using AddressBook.DataAccessLayer;
using AddressBook.Helpers;
using AddressBook.Models;
using AddressBook.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AddressBook.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController()
        {
        }

        // GET: Contacts
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public JsonResult GetUserContacts()
        {
            int userId = User.Identity.GetUserId<int>();

            // Get all contacts that belongs to logged in user
            var query = Db.Contacts
                .Where(c => c.ApplicationUserID == userId)
                .Include(c => c.PhoneNumbers)
                .Include(c => c.EmailAddresses)
                .Include(c => c.Addresses);

            List<ContactIndexViewModel> viewModels = new List<ContactIndexViewModel>();

            foreach (var contact in query)
            {
                viewModels.Add(new ContactIndexViewModel()
                {
                    ID = contact.ID,
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    PhoneNumber = contact.PhoneNumbers.FirstOrDefault(p => p.Default == true)?.Number,
                    Email = contact.EmailAddresses.FirstOrDefault(e => e.Default == true)?.Address,
                    Address = contact.Addresses.FirstOrDefault(a => a.Default == true)?.FullAddress,
                    ProfileImagePath = contact.ProfilePicPath ?? Params.DefaultProfilePicPath,
                });
            }

            return Json(new { Contacts = viewModels }, JsonRequestBehavior.AllowGet);
        }
    }
}
