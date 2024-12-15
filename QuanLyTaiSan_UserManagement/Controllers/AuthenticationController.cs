using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using QuanLyTaiSan_UserManagement.Helper;
using QuanLyTaiSan_UserManagement.Migrations;
using QuanLyTaiSan_UserManagement.Models;
using QuanLyTaiSan_UserManagement.Attribute;

namespace QuanLyTaiSan_UserManagement.Controllers
{
    [Authorize]
    [AuthorizationHandler]
    public class AuthenticationController : Controller
    {
        [AuthorizationViewHandler]
        // GET: Authentication
        public ActionResult Role()
        {
            UserAuthorizationDatabseAction dbContext = new UserAuthorizationDatabseAction();
            ViewData["Role"] = dbContext.GetAllRole();
            var feature = dbContext.GetAllFeatureRecords();
            return View(feature);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Adduserauthorization(FormCollection collection)
        {
            UserAuthorizationDatabseAction _dbContext = new UserAuthorizationDatabseAction();
            SystemFeature feature = new SystemFeature();
            feature.Name = collection["Name"];
            feature.ControllerName = collection["ControllerName"];
            feature.ActionName = collection["ActionName"];
            feature.RoleName = "";
            _dbContext.AddNewFeature(feature);
            return RedirectToAction("Role", "Authentication");
        }
        public JsonResult DeleteAuthentication(List<int> Id)
        {
            UserAuthorizationDatabseAction _dbContext = new UserAuthorizationDatabseAction();
            UserAuthorizationContext context = new UserAuthorizationContext();

            _dbContext.DeleteListFeature(Id);
            var result = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetDetail(int id)
        {
            UserAuthorizationDatabseAction _dbContext = new UserAuthorizationDatabseAction();
            UserAuthorizationContext context = new UserAuthorizationContext();
            var userauthorization = context.SystemFeature.Find(id);
            return Json(new
            {
                data = userauthorization,
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Editauthorization(int Id, string Name, string RoleName, string ControllerName, string ActionName)
        {
            bool result = true;
            UserAuthorizationDatabseAction dbContext = new UserAuthorizationDatabseAction();
            SystemFeature feature = new SystemFeature();
            feature.Id = Id;
            feature.Name = Name;
            feature.ControllerName = ControllerName;
            feature.ActionName = ActionName;
            result = dbContext.UpdateFeature(feature) > 0;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [AuthorizationViewHandler]
        public ActionResult RoleAuthentication(string roleId, int controllerId = -1, string actionText = "")
        {
            UserAuthorizationDatabseAction dbContext = new UserAuthorizationDatabseAction();
            var roleName = dbContext.GetRoleById(roleId).Name;
            ViewData["RoleId"] = roleId;
            ViewData["RoleName"] = roleName;
            ViewData["ActionText"] = actionText;
            ViewData["Controller"] = controllerId;
            var records = dbContext.GetAllFeatureRecords();
            if (controllerId > 0)
            {
                string controllerName = Enum.GetName(typeof(eStatus.ControllerId), controllerId);
                records = records.Where(k => k.ControllerName.EqualsIgnoringCase(controllerName)).ToList();
            }
            if (!String.IsNullOrEmpty(actionText.Trim()))
            {
                records = records
                    .Where(k => k.ActionName.Contains(actionText) || k.Name.Contains(actionText))
                    .ToList();
            }
            ViewData["FeatureList"] = records;
            var dict = new Dictionary<int, string>();
            foreach (var name in Enum.GetNames(typeof(eStatus.ControllerId)))
            {
                var value = (int)Enum.Parse(typeof(eStatus.ControllerId), name);
                dict.Add(value, value.GetEnumDescription(typeof(eStatus.ControllerId)));
            }
            ViewData["ControllerList"] = dict;
            var lstAvailableFeatureId = dbContext.GetFeaturesByRoleName(roleName).Select(k => k.Id);
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(lstAvailableFeatureId.ToList());
            ViewData["AvailableFeatureId"] = json;
            return View();
        }

        [HttpPost]
        public JsonResult ChangeRole(int[] lstId, string roleName)
        {
            UserAuthorizationDatabseAction dbContext = new UserAuthorizationDatabseAction();
            var deleteResult = dbContext.DeleteUserAuthorizationByRoleName(roleName);
            if (!deleteResult) return Json(new { status = false });
            var result = dbContext.AddRangeUserAuthorization(lstId, roleName);
            return Json(!result ? new { status = false } : new { status = true });
        }

        [HttpPost]
        public JsonResult AddNewRole(string id, string name)
        {
            var db = new ApplicationDbContext();
            if (id.Equals("-1"))
            {
                Guid guidId = Guid.NewGuid();
                var sqlInsert = @"INSERT INTO [AspNetRoles] VALUES(@Id, @Name)";
                db.Database.ExecuteSqlCommand(sqlInsert
                    , new SqlParameter("@Id", guidId.ToString().ToLower())
                    , new SqlParameter("@Name", name));
            }
            else
            {
                var existName = db.Database.SqlQuery<string>(@"SELECT Name From [AspNetRoles] WHERE Id = @Id", new SqlParameter("@Id", id)).ToList();
                if (string.IsNullOrEmpty(existName.FirstOrDefault()))
                {
                    return Json(new { status = false });
                }
                var sqlUpdate = @"UPDATE [AspNetRoles] SET Name = @Name WHERE Id = @Id";
                db.Database.ExecuteSqlCommand(sqlUpdate
                    , new SqlParameter("@Id", id)
                    , new SqlParameter("@Name", name));
                UserAuthorizationDatabseAction dbContext = new UserAuthorizationDatabseAction();
                dbContext.UpdateUserAuthorizationByRoleName(existName.FirstOrDefault(), name);
            }
            return Json(new { status = true });
        }
    }
}