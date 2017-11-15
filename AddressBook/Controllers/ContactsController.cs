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

            return Json(new { Contacts = viewModels }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ContactCreateViewModel viewModel = new ContactCreateViewModel
            {
                ProfileImagePath = Params.DefaultProfilePicPath,
                Groups = Db.Groups.OrderBy(g => g.Name).ToList(),
            };

            // Add default phone number to display one input
            viewModel.PhoneNumbers.Add(
                new PhoneNumber
                {
                    NumberType = Params.NumberTypeList.First(),
                    Default = true,
                }
            );

            // Add default email address to display one input
            viewModel.Emails.Add(
                new EmailAddress
                {
                    EmailAddressType = Params.EmailAddressTypeList.First(),
                    Default = true,
                }
            );
            
            ViewBag.GenderList = CreateGenderSelectList(null);
            ViewBag.AddressTypeList = Params.AddressTypeList.AsEnumerable<string>();
            ViewBag.NumberTypeList = Params.NumberTypeList.AsEnumerable<string>();
            ViewBag.EmailAddressTypeList = Params.EmailAddressTypeList.AsEnumerable<string>();

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
                        ProfilePicPath = StoreUploadedPicture("~/Content/ProfilePictures") ?? Params.DefaultProfilePicPath, // Store picture on disk                        
                        ApplicationUserID = User.Identity.GetUserId<int>(),
                    };

                    UpdateContactAddress(contact, model.Address);
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
        /// Updates contact address. If contact doesn't have any, creates new.
        /// </summary>
        /// <param name="contact">Contact for to whom address is assigned.</param>
        /// <param name="address">Address object that contains address data</param>
        private void UpdateContactAddress(Contact contact, Address address)
        {
            // Check if contact has ID
            if (contact.ID != default(int))
            {
                // Check if contacts address list is retrieved. If not retrieve it
                if (contact.Addresses == null || contact.Addresses.Count == 0)
                {
                    contact.Addresses.ToList();
                }

                // Remove all contact addresses
                Db.Addresses
                    .RemoveRange(contact.Addresses);
            }

            // No need for adding addres object to DbContext, EF will automatically take care of it
            contact.Addresses.Add(address);
        }

        /// <summary>
        /// Method that assignes groups to contact. All groups from contact are deleted then new are added.
        /// </summary>
        /// <param name="contact">Contact to which groups are assigned</param>
        /// <param name="formCollection">FormCollection that contains list of groups.</param>
        /// <param name="collectionKey">Key that for formCollection that contains list of values..</param>
        private void AddGroupsToContact(Contact contact, FormCollection formCollection, string collectionKey = "AllGroups")
        {
            string[] submitIds = formCollection.GetValues(collectionKey);

            if (submitIds == null || submitIds.Length == 0)
                return;

            // Group ids are submitted as string array. Convert it to int array
            var groupIds = Array.ConvertAll(submitIds, int.Parse);

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