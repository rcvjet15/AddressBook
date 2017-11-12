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
                        throw new InvalidDataException("Birthdate is not in valid format.");
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
                        ProfilePicPath = StoreProfilePicture() ?? Params.DefaultProfilePicPath, // Store picture 
                        ApplicationUserID = User.Identity.GetUserId<int>(),
                    };

                    Db.Contacts.Add(contact);

                    if (Db.SaveChanges() == 1)
                    {
                        return Json(new { Message = $"Successfully created {model.FirstName} {model.LastName}." }, JsonRequestBehavior.AllowGet);
                    }
                    ModelState.AddModelError(String.Empty, $"Unable to save contact {model.FirstName} {model.LastName} into database. PLease try again.");
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
        /// Method that checks if file is uploaded, validates it and stores it into content
        /// </summary>
        /// <returns>Returns path of stored image if file exists else returns null.</returns>
        private string StoreProfilePicture()
        {
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string storePath = Path.Combine(Server.MapPath("~/Content/ProfilePictures"), fileName);

                    if (!AppMethods.IsValidImageType(new FileInfo(storePath)))
                    {
                        throw new InvalidDataException($"Allowed image extensions are: {String.Join(", ", AppMethods.AllowedImageExtensions)}.");
                    }

                    file.SaveAs(storePath);                    
                    return ConvertToServerRelativePath(storePath);
                }
            }

            return null;
        }
    }
}