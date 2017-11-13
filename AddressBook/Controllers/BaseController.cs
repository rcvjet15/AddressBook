using AddressBook.DataAccessLayer;
using AddressBook.Helpers;
using AddressBook.Models;
using System;
using System.Collections.Generic;
using System.IO;
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

        /// <summary>
        /// Method that collects messages from ModelState and returns them as list.
        /// </summary>
        /// <returns>List of error messages for current ModelState.</returns>
        protected List<string> GetModelStateErrorMessages()
        {
            List<string> messages =
                (
                    from state in ModelState.Values
                    from error in state.Errors
                    select error.ErrorMessage
                ).ToList();

            return messages;
        }

        /// <summary>
        /// Method that takes absolute path to file and converts it to relative path
        /// e.g. C:\Users\User\App\Content\Pictures\pic.jpg into /Content/Pictures/pic.jpg
        /// </summary>
        /// <param name="absolutePath">Absolute path.</param>
        /// <returns>Relative path or null if given argumtn is null or empty.</returns>
        protected string ConvertToServerRelativePath(string absolutePath)
        {
            if (String.IsNullOrEmpty(absolutePath))
            {
                return null;
            }

            return absolutePath.Replace(HttpContext.Server.MapPath("~/"), "/").Replace(@"\", "/");
        }

        /// <summary>
        /// Method that checks if file is uploaded, validates it and stores it into given relative path
        /// </summary>
        /// <param name="relativePath">Relative path e.g. "~/Content/ProfilePictures"</param>
        /// <returns>Returns path of stored image if file exists else returns null.</returns>
        /// <exception cref="InvalidDataException">Throws exception if image has invalid extension,</exception>
        protected string StoreUploadedPicture(string relativePath)
        {
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string storePath = Path.Combine(Server.MapPath(relativePath), fileName);

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