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
    public class ContactsController : BaseController
    {
        [HttpGet]
        public JsonResult GetUserContacts()
        {
            int userId = User.Identity.GetUserId<int>();

            // Get all contacts that belongs to logged in user
            var query = Db.Contacts
                .Where(c => c.ApplicationUserID == userId)
                .Include(c => c.PhoneNumbers)
                .Include(c => c.EmailAddresses)
                .Include(c => c.Addresses)
                .Include(c => c.Groups);

            List<ContactIndexViewModel> viewModels = new List<ContactIndexViewModel>();

            for (int i = 0; i < 100; i++)
            {
                foreach (var contact in query)
                {
                    viewModels.Add(new ContactIndexViewModel()
                    {
                        ID = contact.ID,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        PhoneNumber = contact.PhoneNumbers.FirstOrDefault(p => p.Default == true)?.Number,
                        Email = contact.EmailAddresses.FirstOrDefault(e => e.Default == true)?.Address,
                        Address = contact.Addresses.FirstOrDefault(a => a.Default == true)?.FullAddressWithoutState,
                        ProfileImagePath = contact.ProfilePicPath ?? Params.DefaultProfilePicPath,
                        Groups = contact.Groups.Select(g => g.Name),
                    });
                }
            }
            

            return Json(new { Contacts = viewModels }, JsonRequestBehavior.AllowGet);           
        }

        [HttpGet]
        public ActionResult Create()
        {
            ContactCreateViewModel viewModel = new ContactCreateViewModel
            {
                ProfileImagePath = Params.DefaultProfilePicPath,
                Groups = Db.Groups.ToList()
            };

            return PartialView("_Create", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(ContactCreateViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return Json(new { Message = $"Successfully created {model.FirstName} {model.LastName}." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }

            return new JsonBadRequest(new { Message = "Successfully created " });
        }
    }
}