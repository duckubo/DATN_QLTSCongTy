using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using QuanLyTaiSan_UserManagement.Attribute;
using QuanLyTaiSan_UserManagement.Models;
namespace QuanLyTaiSan_UserManagement.Controllers
{

    public class DeviceTypeController : Controller
    {
        QuanLyTaiSanCtyEntities data = new QuanLyTaiSanCtyEntities();

        public ActionResult DeviceType()
        {
            return View(data.DeviceTypes.ToList());
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddDeviceType(FormCollection collection)
        {
            string TypeName = collection["TypeName"];
            string Notes = collection["Notes"];
            string TypeSymbol = collection["TypeSymbol"];
            data.AddDeviceType(TypeName,TypeSymbol, Notes);
            return RedirectToAction("DeviceType", "DeviceType");
        }
        [HttpGet]
        public JsonResult GetDetail(int id)
        {
            data.Configuration.ProxyCreationEnabled = false;
            var DeviceType = data.DeviceTypes.Find(id);
            return Json(new
            {
                data = DeviceType,
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult EditDeviceType(int Id, string TypeName,string TypeSymbol, string Notes)
        {
            bool result = true;
            data.UpdateDeviceType(Id, TypeName,TypeSymbol, Notes);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteDeviceType(int Id)
        {
            bool result = false;
            var charts = data.SearchDevice(null, Id, null, null,null).ToList().Count();
            if (charts== 0)
            {
                int checkdele = data.DeleteDeviceType(Id);
                result = true;
            }
            else result = false;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ExportStatisticalDevice()
        {
            data.Configuration.ProxyCreationEnabled = false;
            var charts = data.StatisticalDevice().Select(i => new { i.DeviceCode, i.DeviceName, i.PriceOne, i.TimeUse, i.TimeRepair, i.SumPrice });
            var model = charts.ToList();
            using (StringWriter sw = new StringWriter())
            {
                using (System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw))
                {
                    GridView grid = new GridView();
                    grid.DataSource = model;
                    grid.DataBind();
                    Response.ClearContent();
                    Response.Buffer = true;
                    string strDateFormat = string.Empty;
                    strDateFormat = string.Format("{0:yyyy-MMM-dd-hh-mm-ss}", DateTime.Now);
                    Response.AddHeader("content-disposition", "attachment; filename=UserDetails_" + strDateFormat + ".xlxs");
                    Response.ContentType = "application/ms-excel";
                    Response.Charset = "";
                    grid.RenderControl(htw);
                    Response.Output.Write(sw.ToString());
                    Response.End();
                    ViewBag.Sw = sw;
                }
            }
            return Json(new
            {
                ViewBag.Sw
            }, JsonRequestBehavior.AllowGet);
        }
        //  [AuthorizationViewHandler]
        public ActionResult StatisticalDeviceType()
        {
            return View(data.DeviceTypes.ToList());
        }
    }
}