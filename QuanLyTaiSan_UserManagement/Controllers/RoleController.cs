using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using QuanLyTaiSan_UserManagement.Attribute;
using QuanLyTaiSan_UserManagement.Models;
using QuanLyTaiSan_UserManagement.Common;

namespace QuanLyTaiSan_UserManagement.Controllers
{
    public class RoleController : Controller
    {
        QuanLyTaiSanCtyEntities data = new QuanLyTaiSanCtyEntities();
        // GET: Role

        [HasCredential(RoleID = "VIEW_GROUP_ROLE")]
        public ActionResult RoleIndex()
        {
            ViewData["ListUserGroup"] = data.UserGroups.ToList();
            ViewData["ListRoleForGroup"] = data.Roles.ToList();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [HasCredential(RoleID = "ADD_GROUP_ROLE")]
        public ActionResult AddUserGroup(string ID, string Name)
        {
            var dao = new UserDao();
            bool result = dao.AddUserGroup(ID.Trim(), Name.Trim());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        [HasCredential(RoleID = "DELETE_GROUP_ROLE")]
        public ActionResult ConfirmDelete(string ID)
        {
            var dao = new UserDao();
            bool result = dao.DeleteUserGroup(ID.Trim());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_ROLE_FOR_GROUP")]
        [ValidateInput(false)]
        public ActionResult GetRoleForeGroup(string GroupID)
        {
            //  var dao = new UserDao();
            var lstRole = data.Credentials.Where(x => x.UserGroupID == GroupID).Select(x => x.RoleID).ToList();
            // bool result = dao.DeleteUserGroup(RoleID.Trim());
            // bool Result = true; 
            var result = new { lstRole };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        [HasCredential(RoleID = "ADD_ROLE_FOR_GROUP")]
        public ActionResult AddRoleForGroup(string RoleId, string GroupId)
        {
            data.DeleteAllRole(GroupId);
            var dao = new UserDao();
            bool result = dao.AddRoleForGroup(RoleId, GroupId.Trim());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}