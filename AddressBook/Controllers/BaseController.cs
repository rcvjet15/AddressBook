using AddressBook.DataAccessLayer;
using AddressBook.Helpers;
using AddressBook.Models;
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
        private AddressBookDbContext _db;
        private CustomRoleStore _roleStore { get; }
        private CustomRoleManager _roleManager { get; }
        private CustomUserStore _userStore { get; }
        private CustomUserManager _userManager { get; }
        
        /// <summary>
        /// Database context that is responsible for communicating with AddressBook database. It is initialized in BaseController's constructor. 
        /// </summary>
        protected AddressBookDbContext Db { get => _db; }
        
        protected CustomRoleStore RoleStore { get => _roleStore; }
        protected CustomUserStore UserStore { get => _userStore; }
        protected CustomUserManager UserManager { get => _userManager; }
        protected CustomRoleManager RoleManager { get => _roleManager; }

        public BaseController()
        {
            _db = new AddressBookDbContext();

            _roleStore = new CustomRoleStore(_db);
            _roleManager = new CustomRoleManager(_roleStore);
            _userStore = new CustomUserStore(_db);
            _userManager = new CustomUserManager(_userStore);
        }
    }
}