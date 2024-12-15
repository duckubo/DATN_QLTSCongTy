using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyTaiSan_UserManagement.Attribute;
using QuanLyTaiSan_UserManagement.Models;
using System.Text;
using System.Net.Mail;

namespace QuanLyTaiSan_UserManagement.Controllers
{

    public class RequestDeviceController : Controller
    {
        QuanLyTaiSanCtyEntities Ql = new QuanLyTaiSanCtyEntities();
        
        [HasCredential(RoleID = "VIEW_REQUEST_DEVICE")]
        public ActionResult RequestDevice()
        {
            ViewData["User"] = Ql.Users.ToList();
            ViewData["RequestDevices"] = Ql.RequestDevices.ToList();
            var x = Ql.RequestDevices.ToList();
            var lstRequestDevices = Ql.SearchRequestDeviceNew(null, null).ToList();
            return View(lstRequestDevices);
        }

        [HttpPost]
        public ActionResult SeachRequestDevices(FormCollection colection, RequestDevice RequestDevice)
        {
            ViewData["User"] = Ql.Users.ToList();
            ViewData["RequestDevices"] = Ql.RequestDevices.ToList();
            int? Status = colection["Status"].Equals("") ? (int?)null : Convert.ToInt32(colection["Status"]);
            var lstRequestDevices = Ql.SearchRequestDeviceNew(Status, false).ToList();
            var ViewRequestDevices = lstRequestDevices;
            ViewBag.Status = Status;
            return View("RequestDevice", ViewRequestDevices);
        }

        [HasCredential(RoleID = "ADD_REQUEST_DEVICE")]
        public ActionResult AddRequestDevice()
        {
            ViewData["User"] = Ql.Users.Where(x => x.Status != 1 && x.IsDeleted != true).ToList();
            ViewData["DeviceTypes"] = Ql.DeviceTypes.ToList();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [HasCredential(RoleID = "ADD_REQUEST_DEVICE")]
        public ActionResult AddRequestDevice(FormCollection colection, RequestDevice RequestDevice)
        {
            int? UserRequest = colection["UserRequest"].Equals("") ? (int?)null : Convert.ToInt32(colection["UserRequest"]);
            DateTime? DateOfRequest = colection["DateOfRequest"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["DateOfRequest"]);
            DateTime? DateOfUse = colection["DateOfUse"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["DateOfUse"]);
            String DeviceName = colection["DeviceName"];
            int? TypeOfDevice = colection["TypeOfDevice"].Equals("") ? (int?)null : Convert.ToInt32(colection["TypeOfDevice"]);
            String Configuration = colection["Configuration"];
            String Notes = colection["Notes"];
            int? Status = colection["Status"].Equals("-1") ? (int?)null : Convert.ToInt32(colection["Status"]);
            int? NumDevice = colection["NumDevice"].Equals("") ? (int?)null : Convert.ToInt32(colection["NumDevice"]);
            //int? UserApproved = colection["UserApproved"].Equals("") ? (int?)null : Convert.ToInt32(colection["UserApproved"]);
            Ql.AddRequestDevice(UserRequest, DateOfRequest, DateOfUse, DeviceName, TypeOfDevice, Configuration, Notes, Status, NumDevice, null);
            //String Name = Ql.Users.Where(x => x.Id == UserRequest).First().FullName;
            //String MailFrom = Ql.Users.Where(x => x.Id == UserRequest).First().Email.Trim();
            //String MailTo = Ql.Users.Where(x => x.Id == null).First().Email.Trim();
            //String TypeDevice = Ql.DeviceTypes.Where(x => x.Id == TypeOfDevice).First().TypeName.Trim();
            //if (MailFrom != "" & MailTo != "")
            //{
            //    StringBuilder Body = new StringBuilder();
            //    Body.Append("<table>");
            //    Body.Append("<tr><td colspan='2'><h4>Thông Tin Yêu Cầu</h4></td></tr>");
            //    Body.Append("<tr><td>Người Yêu Cầu:</td><td>" + Name + "</td></tr>");
            //    Body.Append("<tr><td>Tên Thiết Bị:</td><td>" + DeviceName + "</td></tr>");
            //    Body.Append("<tr><td>Ngày Sử Dụng:</td><td>" + DateOfUse + "</td></tr>");
            //    Body.Append("<tr><td>Số Lượng Thiết Bị:</td><td>" + NumDevice + "</td></tr>");
            //    Body.Append("<tr><td>Loại Thiết Bị:</td><td>" + TypeDevice + "</td></tr>");
            //    Body.Append("<tr><td>Cấu Hình:</td><td>" + Configuration + "</td></tr>");
            //    Body.Append("<tr><td>Chú Thích:</td><td>" + Notes + "</td></tr>");
            //    Body.Append("</table>");
            //    MailMessage mail = new MailMessage();
            //    mail.To.Add(MailTo);
            //    mail.From = new MailAddress(MailFrom);
            //    mail.Subject = "Yêu Cầu Sử Dụng Thiết Bị";
            //    mail.Body = Body.ToString();// phần thân của mail ở trên
            //    mail.IsBodyHtml = true;
            //    SmtpClient smtp = new SmtpClient();
            //    smtp.Host = "smtp.gmail.com";
            //    smtp.Port = 587;
            //    smtp.UseDefaultCredentials = true;
            //    smtp.Credentials = new System.Net.NetworkCredential(MailFrom, "11111111");// tài khoản Gmail của bạn
            //    smtp.EnableSsl = true;
            //    try
            //    {
            //        smtp.Send(mail);
            //    }
            //    catch (Exception)
            //    {
            //    }
            //}
            return RedirectToAction("RequestDevice", "RequestDevice");
        }

        [HasCredential(RoleID = "EDIT_REQUEST_DEVICE")]
        public ActionResult EditRequestDevice(int Id)
        {
            ViewData["DeviceTypes"] = Ql.DeviceTypes.ToList();
            ViewData["User"] = Ql.Users.Where(x => x.Status != 1 && x.IsDeleted != true).ToList();
            return View(Ql.RequestDevices.Find(Id));
        }
        [HttpPost]
        [ValidateInput(false)]
        [HasCredential(RoleID = "EDIT_REQUEST_DEVICE")]
        public ActionResult EditRequestDevice(FormCollection colection, RequestDevice RequestDevice)
        {
            int? IdRequest = colection["IdRequest"].Equals("-1") ? (int?)null : Convert.ToInt32(colection["IdRequest"]);
            int? UserRequest = colection["UserRequest"].Equals("0") ? (int?)null : Convert.ToInt32(colection["UserRequest"]);
            DateTime? DateOfRequest = colection["DateOfRequest"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["DateOfRequest"]);
            DateTime? DateOfUse = colection["DateOfUse"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["DateOfUse"]);
            String DeviceName = colection["DeviceName"];
            int? TypeOfDevice = colection["TypeOfDevice"].Equals("0") ? (int?)null : Convert.ToInt32(colection["TypeOfDevice"]);
            String Configuration = colection["Configuration"];
            String Notes = colection["Notes"];
            int? Status = colection["Status"].Equals("") ? (int?)null : Convert.ToInt32(colection["Status"]);
            bool? Approved = Convert.ToBoolean(colection["Approved"]);
            int? NumDevice = colection["NumDevice"].Equals("") ? (int?)null : Convert.ToInt32(colection["NumDevice"]);
            String NoteProcess = colection["NoteProcess"];
            String NoteReasonRefuse = colection["NoteReasonRefuse"];
            String NameUserApproved = colection["NameUserApproved"];
            Ql.UpdateRequestDevice(IdRequest, UserRequest, DateOfRequest, DateOfUse, DeviceName, TypeOfDevice, Configuration, Notes, Approved, null, Status, NumDevice, NoteProcess, NoteReasonRefuse, NameUserApproved);
            ViewData["DeviceTypes"] = Ql.DeviceTypes.ToList();
            ViewData["User"] = Ql.Users.Where(x => x.Status != 1 && x.IsDeleted != true).ToList();
            return View(Ql.RequestDevices.Find(IdRequest));

        }

        [HasCredential(RoleID = "DELETE_REQUEST_DEVICE")]
        public JsonResult DeleteRequestDevice(string Id)
        {
            string a = "," + Id + ",";
            bool result = false;
            int checkdele = Ql.DeleteRequestDevice(a);
            if (checkdele > 0)
                result = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateInput(false)]
        [HasCredential(RoleID = "ADD_DEVICE_TYPE")]
        public JsonResult AddDeviceType(string TypeName, string TypeSymbol, string Notes)
        {
            Ql.AddDeviceType(TypeName, TypeSymbol, Notes);
            bool result = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}