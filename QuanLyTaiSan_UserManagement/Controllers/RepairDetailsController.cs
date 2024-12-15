using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyTaiSan_UserManagement.Attribute;
using QuanLyTaiSan_UserManagement.Models;
namespace QuanLyTaiSan_UserManagement.Controllers
{

    public class RepairDetailsController : Controller
    {

        QuanLyTaiSanCtyEntities Ql = new QuanLyTaiSanCtyEntities();

        public ActionResult RepairDetails()
        {
            ViewData["RepairTypes"] = Ql.RepairTypes.ToList();
            ViewData["User"] = Ql.Users.Where(x => x.IsDeleted == false).ToList();
            var lstRepairDetails = Ql.SearchRepairDetails(null, null, null, null).ToList();
            return View(lstRepairDetails);
        }

        [HttpPost]
        public ActionResult SeachRepairDetails(FormCollection colection, RepairDetail RepairDetails)
        {
            ViewData["User"] = Ql.Users.ToList();
            ViewData["RepairTypes"] = Ql.RepairTypes.ToList();
            int? RepairType1 = colection["RepairType1"].Equals("0") ? (int?)null : Convert.ToInt32(colection["RepairType1"]);
            int? Users = colection["User"].Equals("0") ? (int?)null : Convert.ToInt32(colection["User"]);
            int? Status = colection["Status"].Equals("-1") ? (int?)null : Convert.ToInt32(colection["Status"]);
            int? IdDevice = colection["IdDevice"].Equals("") ? (int?)null : Convert.ToInt32(colection["IdDevice"]);
            var lstRepairDetails = Ql.SearchRepairDetails(RepairType1, Users, IdDevice, Status).ToList();
            var ViewRepairDetails = lstRepairDetails;
            ViewBag.RepairType1 = RepairType1;
            ViewBag.Users = Users;
            ViewBag.Status = Status;
            return View("RepairDetails", ViewRepairDetails);
        }

        [HttpGet]
        public JsonResult GetDetail(int id)
        {
            Ql.Configuration.ProxyCreationEnabled = false;
            var getDevices = Ql.SearchRepairDevice(id).FirstOrDefault();
            return Json(new
            {
                data = getDevices,
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddRepairDetails()
        {
            ViewData["Devices"] = Ql.SearchDevice(null, null, null, null, null).Where(x => x.Status != 2 && x.StatusRepair != 1).ToList();
            ViewData["User"] = Ql.Users.Where(x => x.Status != 1 && x.IsDeleted == false).ToList();
            ViewData["RepairTypes"] = Ql.RepairTypes.ToList();
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddRepairDetails(FormCollection colection, RepairDetail RepairDetail)
        {
            int? DeviceId = colection["DeviceId"].Equals("") ? (int?)null : Convert.ToInt32(colection["DeviceId"]);
            DateTime? DateOfRepair = colection["DateOfRepair"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["DateOfRepair"]);
            string AddressOfRepair = colection["AddressOfRepair"];
            int? TimeOrder = colection["TimeOrder"].Equals("") ? (int?)null : Convert.ToInt32(colection["TimeOrder"]);
            int? TypeOfRepair = colection["TypeOfRepair"].Equals("0") ? (int?)null : Convert.ToInt32(colection["TypeOfRepair"]);
            int? UserId = colection["UserId"].Equals("") ? (int?)null : Convert.ToInt32(colection["UserId"]);
            string Notes = colection["Notes"];
            DateTime? NextDateOfRepair = colection["NextDateOfRepair"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["NextDateOfRepair"]);
            Ql.AddRepairDetails(DeviceId, DateOfRepair, NextDateOfRepair, TimeOrder, TypeOfRepair, AddressOfRepair, UserId, Notes);
            return RedirectToAction("RepairDetails", "RepairDetails");
        }

        [ValidateInput(false)]
        public ActionResult EditRepairDetails(int Id)
        {
            var his = Ql.RepairDetails.Find(Id).DeviceId;

            ViewData["RepairHistory"] = Ql.HistoryRepairDetails(his).Where(x => x.Status == 1).ToList();
            ViewData["User"] = Ql.Users.Where(x => x.Status != 1 && x.IsDeleted != true).ToList();
            ViewData["RepairTypes"] = Ql.RepairTypes.ToList();
            var repair = Ql.RepairDetailsById(Id).Single();
            return View(repair);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditRepairDetails(FormCollection colection, RepairDetail RepairDetail)
        {
            int? Id = colection["Id"].Equals("0") ? (int?)null : Convert.ToInt32(colection["Id"]);
            int? DeviceId = colection["DeviceId"].Equals("") ? (int?)null : Convert.ToInt32(colection["DeviceId"]);
            DateTime? DateOfRepair = colection["DateOfRepair"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["DateOfRepair"]);
            string AddressOfRepair = colection["AddressOfRepair"];
            int? TimeOrder = colection["TimeOrder"].Equals("") ? (int?)null : Convert.ToInt32(colection["TimeOrder"]);
            int? TypeOfRepair = colection["TypeOfRepair"].Equals("") ? (int?)null : Convert.ToInt32(colection["TypeOfRepair"]);
            int? UserId = colection["UserId"].Equals("0") ? (int?)null : Convert.ToInt32(colection["UserId"]);
            string NoteRepair = colection["NoteRepair"];
            DateTime? NextDateOfRepair = colection["NextDateOfRepair"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["NextDateOfRepair"]);
            int? Status = colection["Status"].Equals("") ? (int?)null : Convert.ToInt32(colection["Status"]);
            double Price = colection["Prices"].Equals("") ? 0 : Convert.ToDouble(colection["Prices"]);
            Ql.EditRepairDetails(Id, DeviceId, DateOfRepair, NextDateOfRepair, TimeOrder, TypeOfRepair, AddressOfRepair, UserId, NoteRepair, Status, Price);
            return RedirectToAction("EditRepairDetails", "RepairDetails");
        }

        public JsonResult DeleteRepairDetails(string Id)
        {
            string a = "," + Id + ",";
            bool result = false;
            int checkdele = Ql.DeleteRepairDetails(a);
            if (checkdele > 0)
                result = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteRepairDetailsOne(int Id)
        {
            bool result = false;
            var dv = Ql.RepairDetails.Find(Id).Status;
            if (dv == 1)
            {
                result = true;
                string a = "," + Id + ",";
                Ql.DeleteRepairDetails(a);
            }
            else
            {
                result = false;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }

}
