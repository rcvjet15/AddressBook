using AddressBook.Helpers;
using AddressBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AddressBook.Controllers
{
    public class GroupsController : BaseController
    {
        [HttpPost]
        public JsonResult Create(string groupName)
        {
            if (!String.IsNullOrEmpty(groupName))
            {
                // Check if group name exists
                Group group = Db.Groups
                    .FirstOrDefault(g => g.Name == groupName);

                // If group doesn't exist
                if (group == null)
                {
                    group = new Group
                    {
                        Name = groupName
                    };

                    Db.Groups.Add(group);

                    if (Db.SaveChanges() == 1)
                    {
                        return Json(new { Message = String.Format("Successfully created {0}", group.Name), Id = group.ID }, JsonRequestBehavior.AllowGet);
                    }

                    ModelState.AddModelError(String.Empty, $"Unable to save to database group named: {groupName}. Please try again.");
                }
                else
                {
                    ModelState.AddModelError(String.Empty, $"Group named: {groupName} already exists.");
                }
            }
            else
            {
                ModelState.AddModelError(String.Empty, $"Group name cannot be empty.");
            }

            return new JsonBadRequest(new { Errors = GetModelStateErrorMessages() });
        }
    }
}