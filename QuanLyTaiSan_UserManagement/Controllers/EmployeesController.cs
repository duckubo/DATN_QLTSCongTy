using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyTaiSan_UserManagement.Attribute;
using QuanLyTaiSan_UserManagement.Models;

namespace QuanLyTaiSan_UserManagement.Controllers
{

    public class EmployeesController : Controller
    {
        QuanLyTaiSanCtyEntities data = new QuanLyTaiSanCtyEntities();

        public ActionResult UserManagement()
        {
            var lstUser = data.SearchUser(null).ToList();
            return View(lstUser);
        }
        [HttpPost]
        public ActionResult SearchUser(FormCollection collection)
        {
            int Status = Convert.ToInt32(collection["Status"]);
            ViewBag.status = Status;
            var charts = data.SearchUser(Status).ToList();
            var model = charts.ToList();
            return View("UserManagement", model);
        }

        public ActionResult CreateUser()
        {
            ViewData["Department"] = data.ProjectDKCs. Where(x => x.Status != 2 && x.IsDeleted == false).ToList();
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateUser(FormCollection collection)
        {
            string UserName = collection["UserName"];
            string FullName = collection["FullName"];
            string Email = collection["Email"];
            string PhoneNumber = collection["PhoneNumber"];
            string Address = collection["Address"];
            string Department = collection["Department"];
            string Position = collection["Position"];
            data.AddUser(UserName, null, FullName, Email, PhoneNumber, Address, Department, Position, null, 0);
            return RedirectToAction("UserManagement", "Employees");
        }

        public ActionResult DetailUser(int Id)
        {
            ViewData["Department"] = data.ProjectDKCs.Where(x => x.Status != 2 && x.IsDeleted == false).ToList();
            var ID = data.Users.Where(x => x.Id == Id).Select(x => x.Department).FirstOrDefault();
            int ID1 = Convert.ToInt32(ID);
            ViewBag.ID = ID1;
           
            return View(data.Users.Find(Id));
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DetailUser(FormCollection collection)
        {
            int Id = Convert.ToInt32(collection["Id"]);
            string UserName = collection["UserName"];
            string FullName = collection["FullName"];
            string Email = collection["Email"];
            string PhoneNumber = collection["PhoneNumber"];
            string Address = collection["Address"];
            string Department = collection["Department"];
            string Position = collection["Position"];
            int Status = Convert.ToInt32(collection["Status"]);
            data.UpdateUser(Id, UserName, null, FullName, Email, PhoneNumber, Address, Department, Position, null, Status);
            return RedirectToAction("UserManagement", "Employees");
        }
        public JsonResult DeleteUser(int Id)
        {
            bool result = false;
            var charts = data.SearchDevice(null, null, null, null,null).Where(x => x.UserId == Id).ToList().Count();
            charts += data.SearchProject(Id, null,0, null).ToList().Count();
            charts += data.SearchRepairDetails(null, Id, null, null).ToList().Count();
            charts += data.RequestDevices.Where(x => x.UserApproved == Id).ToList().Count();
            charts += data.RequestDevices.Where(x => x.UserRequest == Id).ToList().Count();
            charts += data.ScheduleTests.Where(x => x.UserTest == Id).ToList().Count();
            if (charts == 0)
            {
                data.DeleteUser(Id);
                result = true;
            }
            else result = false;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Role()
        {
            ViewData["Role"] = data.Roles.ToList();
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddRole(FormCollection collection)
        {
            string RoleName = collection["RoleName"];
            string Notes = collection["Notes"];
            data.AddRole(RoleName, Notes);
            return RedirectToAction("Role", "Employees");
        }
        [HttpGet]
        public JsonResult GetDetail(int id)
        {
            data.Configuration.ProxyCreationEnabled = false;
            var Role = data.Roles.Find(id);
            return Json(new
            {
                data = Role,
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult EditRole(int Id, string RoleName, string Notes)
        {
            bool result = true;
            data.UpdateRole(Id, RoleName, Notes);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteRole(int Id)
        {
            bool result = false;
            int checkdele = data.DeleteRole(Id);
            if (checkdele > 0)
                result = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}