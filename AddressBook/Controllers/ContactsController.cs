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
                    PhoneNumber = contact.PhoneNumbers.FirstOrDefault(p => p.IsDefault == true)?.Number,
                    Email = contact.EmailAddresses.FirstOrDefault(e => e.IsDefault == true)?.Address,
                    Address = contact.Addresses.FirstOrDefault(a => a.Default == true)?.FullAddressWithoutState,
                    ProfileImagePath = contact.ProfilePicPath ?? Params.DefaultProfilePicPath,
                    Groups = contact.Groups.Select(g => g.Name),
                });
            }

            return Json(new { Contacts = viewModels }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Details(int id)
        {
            List<string> errors = new List<string>();

            try
            {
                Contact contact = Db.Contacts
                    .Find(id);

                if (contact == null)
                {
                    throw new InvalidOperationException("Unable to find requested contact.");
                }

                return PartialView("_Details", contact);
            }
            catch (InvalidOperationException ex)
            {
                errors.Add(ex.Message);
            }

            return new JsonBadRequest(new { Errors = errors.ToArray() }, JsonRequestBehavior.AllowGet); // return as array
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
                    IsDefault = true,
                }
            );

            // Add default email address to display one input
            viewModel.Emails.Add(
                new EmailAddress
                {
                    EmailAddressType = Params.EmailAddressTypeList.First(),
                    IsDefault = true,
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

                    UpdateContactPhoneNumbers(contact, model.PhoneNumbers);
                    UpdateContactEmailAddresses(contact, model.Emails);
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

        [HttpGet]
        public ActionResult Edit(int id)
        {
            List<string> errors = new List<string>();

            try
            {
                Contact contact = Db.Contacts
                    .Find(id);

                if (contact == null)
                {
                    throw new InvalidOperationException("Unable to find requested contact.");
                }

                ContactEditViewModel viewModel = new ContactEditViewModel
                {
                    ID = contact.ID,
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    Birthdate = contact.Birthdate.HasValue ? contact.Birthdate.ToString() : String.Empty,
                    Gender = contact.Gender,
                    Note = contact.Note,
                    Organization = contact.Organization,
                    Relationship = contact.Relationship,
                    Title = contact.Title,
                    ProfileImagePath = contact.ProfilePicPath ?? Params.DefaultProfilePicPath,
                    PhoneNumbers = contact.PhoneNumbers.ToList(),
                    Emails = contact.EmailAddresses.ToList(),
                    Address = contact.Addresses.FirstOrDefault(),
                    Groups = contact.Groups.ToList(),                    
                };

                if (viewModel.PhoneNumbers.Count == 0)
                {
                    // Add default phone number to display one input
                    viewModel.PhoneNumbers.Add(
                        new PhoneNumber
                        {
                            NumberType = Params.NumberTypeList.First(),
                            IsDefault = true,
                        }
                    );
                }           

                if (viewModel.Emails.Count == 0)
                {
                    // Add default email address to display one input
                    viewModel.Emails.Add(
                        new EmailAddress
                        {
                            EmailAddressType = Params.EmailAddressTypeList.First(),
                            IsDefault = true,
                        }
                    );
                }

                ViewBag.AllGroups = Db.Groups.ToList();
                ViewBag.GenderList = CreateGenderSelectList(contact.Gender);
                ViewBag.AddressTypeList = Params.AddressTypeList.AsEnumerable<string>();
                ViewBag.NumberTypeList = Params.NumberTypeList.AsEnumerable<string>();
                ViewBag.EmailAddressTypeList = Params.EmailAddressTypeList.AsEnumerable<string>();

                return PartialView("_Edit", viewModel);
            }
            catch (InvalidOperationException ex)
            {
                errors.Add(ex.Message);
            }            

            return new JsonBadRequest(new { Errors = errors.ToArray() }, JsonRequestBehavior.AllowGet); // return as array
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(int id, ContactEditViewModel model, FormCollection formCollection)
        {
            try
            {
                Contact contact = Db.Contacts
                  .Find(id);

                if (contact == null)
                {
                    throw new InvalidOperationException("Unable to find requested contact.");
                }

                if (ModelState.IsValid)
                {
                    if (!DateTime.TryParse(model.Birthdate, out DateTime birthDate))
                    {
                        throw new InvalidDataException("Birthdate is has invalid format.");
                    }
                                        
                    contact.FirstName = model.FirstName;
                    contact.LastName = model.LastName;
                    contact.Birthdate = birthDate;
                    contact.Relationship = model.Relationship;
                    contact.Title = model.Title;
                    contact.Note = model.Note;
                    contact.Gender = model.Gender;
                    // If nothing uploaded, check if contact already has profile picture. If not, assign default.
                    contact.ProfilePicPath = StoreUploadedPicture("~/Content/ProfilePictures") ?? (contact.ProfilePicPath ?? Params.DefaultProfilePicPath);
                    
                    UpdateContactPhoneNumbers(contact, model.PhoneNumbers);
                    UpdateContactEmailAddresses(contact, model.Emails);
                    UpdateContactAddress(contact, model.Address);
                    AddGroupsToContact(contact, formCollection);

                    Db.Entry(contact).State = EntityState.Modified;                    

                    Db.SaveChanges();

                    return Json(new { Message = $"Successfully updated {model.FirstName} {model.LastName}." }, JsonRequestBehavior.AllowGet);
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

        [HttpGet]
        public ActionResult Delete(int id)
        {
            List<string> errors = new List<string>();

            try
            {
                Contact contact = Db.Contacts
                    .Find(id);

                if (contact == null)
                {
                    throw new InvalidOperationException("Unable to find requested contact.");
                }

                return PartialView("_Delete", contact);
            }
            catch (InvalidOperationException ex)
            {
                errors.Add(ex.Message);
            }

            return new JsonBadRequest(new { Errors = errors.ToArray() }, JsonRequestBehavior.AllowGet); // return as array
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            List<string> errors = new List<string>();

            try
            {
                Contact contact = Db.Contacts
                    .Find(id);

                if (contact == null)
                {
                    throw new InvalidOperationException("Unable to find requested contact.");
                }

                Db.Entry(contact).State = EntityState.Deleted;
                Db.SaveChanges();

                return Json(new { Message = $"Successfully deleted {contact.FirstName} {contact.LastName}." }, JsonRequestBehavior.AllowGet);
            }
            catch (InvalidOperationException ex)
            {
                errors.Add(ex.Message);
            }

            return new JsonBadRequest(new { Errors = errors.ToArray() }, JsonRequestBehavior.AllowGet); // return as array
        }

        /// <summary>
        /// Method that assigns phone numbers to contact. First all its numbers are deleted then new one are added.
        /// </summary>
        /// <param name="contact">Contact whom numbers are assigned.</param>
        /// <param name="phoneNumbers">Phone numbers that are going to be assigned.</param>
        /// // <exception cref="InvalidDataException">Will be thworn if count of phone numbers is more than 6.</exception>
        private void UpdateContactPhoneNumbers(Contact contact, List<PhoneNumber> phoneNumbers)
        {
            if (phoneNumbers.Count > 6)
            {
                throw new InvalidDataException("Contact can have maximum of 6 numbers!");
            }

            // Check if contact has ID
            if (contact.ID != default(int))
            {
                // Check if contacts phone number list is retrieved. If not retrieve it
                if (contact.PhoneNumbers == null || contact.PhoneNumbers.Count == 0)
                {
                    contact.PhoneNumbers.ToList();
                }

                // Remove all contact phone numbers
                Db.PhoneNumbers
                    .RemoveRange(contact.PhoneNumbers);
            }

            foreach (var phoneNumber in phoneNumbers.Where(p => !String.IsNullOrEmpty(p.Number)))
            {
                phoneNumber.IsDefault = phoneNumber.IsDefault ?? false; // If value is null then set it to false
                // No need for adding phone numbere object to DbContext, EF will automatically take care of it            
                contact.PhoneNumbers.Add(phoneNumber);
            }
        }

        /// <summary>
        /// Method that assigns phone numbers to contact. First all its numbers are deleted then new one are added.
        /// </summary>
        /// <param name="contact">Contact whom numbers are assigned.</param>
        /// <param name="phoneNumbers">Phone numbers that are going to be assigned.</param>
        /// // <exception cref="InvalidDataException">Will be thworn if count of phone numbers is more than 6.</exception>
        private void UpdateContactEmailAddresses(Contact contact, List<EmailAddress> emailAddresses)
        {
            if (emailAddresses.Count > 6)
            {
                throw new InvalidDataException("Contact can have maximum of 6 numbers!");
            }

            // Check if contact has ID
            if (contact.ID != default(int))
            {
                // Check if contacts phone number list is retrieved. If not retrieve it
                if (contact.EmailAddresses == null || contact.EmailAddresses.Count == 0)
                {
                    contact.EmailAddresses.ToList();
                }

                // Remove all contact phone numbers
                Db.EmailAddresses
                    .RemoveRange(contact.EmailAddresses);
            }
            
            foreach (var emailAddress in emailAddresses.Where(e => !String.IsNullOrEmpty(e.Address)))
            {
                emailAddress.IsDefault = emailAddress.IsDefault ?? false; // If value is null then set it to false
                // No need for adding email address object to DbContext, EF will automatically take care of it            
                contact.EmailAddresses.Add(emailAddress);
            }
        }

        /// <summary>
        /// Updates contact address. If contact doesn't have any, creates new.
        /// </summary>
        /// <param name="contact">Contact for to whom address is assigned.</param>
        /// <param name="address">Address object that contains address data.</param>
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

            // First remove all contact groups 
            if (contact.ID != default(int) && contact.Groups.Count > 0)
            {
                string sql = "DELETE FROM dbo.ContactGroup " +
                    "WHERE ContactID = (@p0)";

                Db.Database.ExecuteSqlCommand(sql, contact.ID);
            }
            
            // If non of the groups were selected, return
            if (submitIds == null || submitIds.Length == 0)
                return;

            // Group ids are submitted as string array. Convert it to int array
            var groupIds = Array.ConvertAll(submitIds, int.Parse);

            List<Group> groups = Db.Groups
                .Where(g => groupIds.Contains(g.ID))
                .ToList();

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