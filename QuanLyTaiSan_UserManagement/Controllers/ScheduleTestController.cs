using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyTaiSan_UserManagement.Attribute;
using QuanLyTaiSan_UserManagement.Models;

namespace QuanLyTaiSan_UserManagement.Controllers
{

    public class ScheduleTestController : Controller
    {

        QuanLyTaiSanCtyEntities Ql = new QuanLyTaiSanCtyEntities();


        public ActionResult ScheduleTest()
        {
            ViewData["User"] = Ql.Users.ToList();
            ViewData["ScheduleTests"] = Ql.ScheduleTests.ToList();
            var lstScheduleTests = Ql.SearchScheduleTest(null, null).ToList();
            return View(lstScheduleTests);
        }

        [HttpPost]
        public ActionResult SeachScheduleTest(FormCollection colection, RepairDetail RepairDetails)
        {
            ViewData["User"] = Ql.Users.ToList();
            ViewData["ScheduleTests"] = Ql.ScheduleTests.ToList();
            int? Users = colection["User"].Equals("0") ? (int?)null : Convert.ToInt32(colection["User"]);
            int? Status = colection["Status"].Equals("") ? (int?)null : Convert.ToInt32(colection["Status"]);          
            var lstScheduleTest = Ql.SearchScheduleTest(Users, Status).ToList();
            var ViewScheduleTest = lstScheduleTest;
            ViewBag.Users = Users;
            ViewBag.Status = Status;
            return View("ScheduleTest", lstScheduleTest);
        }

        public ActionResult AddScheduleTest()
        {
            ViewData["Devices"] = Ql.Devices.Where(x=> x.IsDeleted != true).ToList();
            ViewData["User"] = Ql.Users.Where(x => x.Status != 1 && x.IsDeleted != true).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult AddScheduleTest(FormCollection colection, ScheduleTest ScheduleTest)
        {
            int? DeviceId = colection["DeviceId"].Equals("") ? (int?)null : Convert.ToInt32(colection["DeviceId"]);
            DateTime? DateOfTest = colection["DateOfTest"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["DateOfTest"]);
            DateTime? NextDateOfTest = colection["NextDateOfTest"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["NextDateOfTest"]);
            String CategoryTest = colection["CategoryTest"];
            int? UserTest = colection["UserTest"].Equals("0") ? (int?)null : Convert.ToInt32(colection["UserTest"]);
            String Notes = colection["Notes"] ;
            Ql.AddScheduleTest(DeviceId, DateOfTest, NextDateOfTest, CategoryTest, UserTest, Notes);
            return RedirectToAction("ScheduleTest", "ScheduleTest");
        }
      //  [AuthorizationViewHandler]
        public ActionResult EditScheduleTest(int Id)
        {
            var dvId = Ql.ScheduleTests.Find(Id).DeviceId;
            ViewData["historyScheduleTests"] = Ql.HistoryScheduleTestById(dvId).Where(x=>x.Status==1).ToList();
            ViewData["User"] = Ql.Users.Where(x => x.Status != 1 && x.IsDeleted != true).ToList();
            var lstSchedule = Ql.ScheduleTestById(Id).Single();
            return View(lstSchedule);
        }
        [HttpPost]
        public ActionResult EditScheduleTest(FormCollection colection, ScheduleTest ScheduleTest)
        {
            int? Id = colection["Id"].Equals("") ? (int?)null : Convert.ToInt32(colection["Id"]);
            int? DeviceId = colection["DeviceId"].Equals("") ? (int?)null : Convert.ToInt32(colection["DeviceId"]);
            //var dateot = colection["DateOfTest"];
            DateTime? DateOfTest = colection["DateOfTest"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["DateOfTest"]);
            DateTime? NextDateOfTest = colection["NextDateOfTest"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["NextDateOfTest"]);
            String CategoryTest = colection["CategoryTest"];
            int? UserTest = colection["UserTest"].Equals("0") ? (int?)null : Convert.ToInt32(colection["UserTest"]);
            String Notes = colection["Notes"];
            int? Status = colection["Status"].Equals("") ? (int?)null : Convert.ToInt32(colection["Status"]);
            Ql.UpdateScheduleTest(Id, DeviceId, DateOfTest, NextDateOfTest, CategoryTest, UserTest, Notes, Status);
            return RedirectToAction("EditScheduleTest", "ScheduleTest");

        }

        public JsonResult DeleteScheduleTest(string Id)
        {
            string a = "," + Id + ",";
            bool result = false;
            int checkdele = Ql.DeleteScheduleTest(a);
            if (checkdele > 0)
                result = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}