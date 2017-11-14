using AddressBook.Helpers;
using AddressBook.Models;
using AddressBook.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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
                Groups = Db.Groups.OrderBy(g => g.Name).ToList()
            };

            ViewBag.GenderList = CreateGenderSelectList(null);
            ViewBag.AddressTypeList = Params.AddressTypeList.AsEnumerable<string>();

            return PartialView("_Create", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(ContactCreateViewModel model, FormCollection formCollection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!DateTime.TryParse(model.Birthdate, out DateTime birthDate))
                    {
                        throw new InvalidDataException("Birthdate is has invalid format.");
                    }

                    Contact contact = new Contact
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Birthdate = birthDate,
                        Relationship = model.Relationship,
                        Title = model.Title,
                        Note = model.Note,
                        Gender = model.Gender,
                        ProfilePicPath = StoreUploadedPicture("~/Content/ProfilePictures") ?? Params.DefaultProfilePicPath, // Store picture 
                        ApplicationUserID = User.Identity.GetUserId<int>(),
                        Groups = new List<Group>(),
                    };
                        
                    AddGroupsToContact(contact, formCollection);

                    Db.Contacts.Add(contact);

                    Db.SaveChanges();

                    return Json(new { Message = $"Successfully created {model.FirstName} {model.LastName}." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (InvalidDataException ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex.Message);
            }

            return new JsonBadRequest(new { Errors = GetModelStateErrorMessages() });
        }

        /// <summary>
        /// Method that assignes groups to contact. All groups from contact are deleted then new are added.
        /// </summary>
        /// <param name="contact">Contact to which groups are assigned</param>
        /// <param name="formCollection">FormCollection that contains list of groups.</param>
        /// <param name="collectionKey">Key that for formCollection that contains list of values..</param>
        private void AddGroupsToContact(Contact contact, FormCollection formCollection, string collectionKey = "AllGroups")
        {
            // Group ids are submitted as array. Convert it oto
            var groupIds = Array.ConvertAll(formCollection.GetValues(collectionKey), int.Parse);
            
            List<Group> groups = Db.Groups
                .Where(g => groupIds.Contains(g.ID))
                .ToList();

            // First remove all groups 
            foreach (var group in contact.Groups)
            {
                contact.Groups.Remove(group);
            }

            // Add new groups
            groups.ForEach(g => contact.Groups.Add(g));
        }

        private SelectList CreateGenderSelectList(string selectedValue)
        {
            string[] genders = Params.GenderList;

            return new SelectList(genders, selectedValue);
        }
    }
}