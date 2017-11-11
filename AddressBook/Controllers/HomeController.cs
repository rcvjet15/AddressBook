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
    }
}
