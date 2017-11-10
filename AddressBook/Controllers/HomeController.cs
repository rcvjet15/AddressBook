using AddressBook.DataAccessLayer;
using AddressBook.Helpers;
using AddressBook.Models;
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

            var c = new Contact()
            {
                FirstName = "Mom",
                LastName = "Perić",
                Gender = "Male",
                Birthdate = DateTime.Now,
                Relationship = "Family",
                ProfilePicPath = Params.DefaultProfilePicPath,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
            };

            _db.Contacts.Add(c);
            _db.SaveChanges();

            //var query = _db.Contacts
            //    .Include(c => c.PhoneNumbers.FirstOrDefault(p => p.Default == true))
            //    .Include(c => c.EmailAddresses.FirstOrDefault(e => e.Default == true))
            //    .Include(c => c.Addresses.FirstOrDefault(a => a.Default == true))

            return View();
        }

        // GET: Contacts/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Contacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Contacts/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Contacts/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Contacts/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Contacts/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
