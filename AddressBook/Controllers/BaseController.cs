using AddressBook.DataAccessLayer;
using AddressBook.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AddressBook.Controllers
{
    /// <summary>
    /// Base controller from which all app controllers will be inherited.
    /// </summary>
    public class BaseController : Controller
    {
        protected AddressBookDbContext Db { get; set; }
        
        public BaseController()
        {
            Db = new AddressBookDbContext(Params.DefaultConnectionName);
        }
    }
}