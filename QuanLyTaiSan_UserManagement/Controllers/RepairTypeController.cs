using QuanLyTaiSan_UserManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyTaiSan_UserManagement.Attribute;

namespace QuanLyTaiSan_UserManagement.Controllers
{
    public class RepairTypeController : Controller
    {
        QuanLyTaiSanCtyEntities Ql = new QuanLyTaiSanCtyEntities();

        public ActionResult RepairType()
        {
            ViewData["RepairTypes"] = Ql.RepairTypes.ToList();
            return View();
        }

        [HttpGet]
        public JsonResult GetDetail(int id)
        {
            Ql.Configuration.ProxyCreationEnabled = false;
            var RepairTypes = Ql.RepairTypes.Find(id);
            return Json(new
            {
                data = RepairTypes,
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddRepairType()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddRepairType(FormCollection colection, RepairType RepairType)
        {
            String Notes = colection["Notes"];
            String TypeName = colection["TypeName"];
            Ql.AddRepairType(TypeName, Notes);
            return RedirectToAction("RepairType", "RepairType");
        }

        public ActionResult EditRepairType(int Id)
        {
            return View(Ql.RepairTypes.Find(Id));
        }
        [HttpPost]
        public ActionResult EditRepairType(int Id, String TypeName,String Notes)
        {
            bool result = false;
            int checkdele = Ql.EditRepairType(Id, TypeName, Notes);
            if (checkdele > 0)
                result = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteRepairType(int Id)
        {
            bool result = false;
            var dv = Ql.RepairDetails.Where(x => x.TypeOfRepair == Id);
            if (dv.Count() > 0)
            {
                result = false;
            }
            else
            {
                string a = "," + Id + ",";
                int checkdele = Ql.DeleteRepairType(a);
                if (checkdele > 0)
                    result = true;
            }
                return Json(result, JsonRequestBehavior.AllowGet);
        }
    
    }
}