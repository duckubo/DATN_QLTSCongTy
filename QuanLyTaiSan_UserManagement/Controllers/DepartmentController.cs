using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyTaiSan_UserManagement.Attribute;
using QuanLyTaiSan_UserManagement.Models;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Collections;
using System.Text.RegularExpressions;

namespace QuanLyTaiSan_UserManagement.Controllers
{
    // [Authorize]
    // [AuthorizationHandler]
    // [HasCredential(RoleID = "")]
    public class DepartmentController : Controller
    {
        QuanLyTaiSanCtyEntities Ql = new QuanLyTaiSanCtyEntities();
        [HasCredential(RoleID = "VIEW_DEPARTMENT")]
        public ActionResult Department()
        {
            ViewBag.ProjectNb = Ql.SearchProject(null, null, 1, null).Count();
            ViewData["User"] = Ql.Users.Where(x => x.Status != 1 && x.IsDeleted == false).ToList();
            var lstProject = Ql.SearchProject(null, null, 1, null).ToList();
            return View(lstProject);
        }
        [HttpPost]
        public ActionResult SeachDepartment(FormCollection colection, ProjectDKC Project)
        {
            ViewData["User"] = Ql.Users.Where(x => x.Status != 1 && x.IsDeleted == false).ToList();
            String ProjectSymbol = colection["ProjectSymbol"].Trim();
            //   int? Status = colection["Status"].Equals("0") ? (int?)null : Convert.ToInt32(colection["Status"]);
            int? ManagerProject = colection["ManagerProject"].Equals("0") ? (int?)null : Convert.ToInt32(colection["ManagerProject"]);
            var lstProject = Ql.SearchProject(ManagerProject, null, 1, ProjectSymbol).ToList();
            var ViewProject = lstProject;
            // ViewBag.Status = Status;
            ViewBag.ManagerProject = ManagerProject;
            ViewBag.ProjectSymbol = ProjectSymbol;
            ViewBag.ProjectNb = Ql.SearchProject(ManagerProject, null, 1, ProjectSymbol).Count();
            return View("Department", ViewProject);
        }
        //  [AuthorizationViewHandler]
        [HasCredential(RoleID = "ADD_DEPARTMENT")]
        public ActionResult AddDepartment()
        {
            ViewData["User"] = Ql.Users.Where(x => x.Status != 1 && x.IsDeleted == false).ToList();
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        [HasCredential(RoleID = "ADD_REPAIR_DEVICE")]
        public JsonResult AddRepairType(string TypeName, string Notes)
        {
            Ql.AddRepairType(TypeName, Notes);
            bool result = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_DEPARTMENT")]
        public ActionResult AddDepartment(FormCollection colection, ProjectDKC Project)
        {

            String ProjectSymbol = colection["ProjectSymbol"];
            String ProjectName = colection["ProjectName"];
            int? ManagerProject = colection["ManagerProject"].Equals("0") ? (int?)null : Convert.ToInt32(colection["ManagerProject"]);
            String Address = colection["Address"];
            // DateTime? FromDate = colection["FromDate"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["FromDate"]);
            // DateTime? ToDate = colection["ToDate"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["ToDate"]);
            int? Status = colection["Status"].Equals("0") ? (int?)null : Convert.ToInt32(colection["Status"]);
            var lstProjectSymbol = Ql.ProjectDKCs.Where(x => x.ProjectSymbol.Trim() == ProjectSymbol.Trim()).ToList();
            var CountPr = lstProjectSymbol.Count();
            if (CountPr > 0)
            {
                ViewBag.Tb = "Mã bị trùng";
                ViewBag.ProjectSymbol = ProjectSymbol;
                ViewBag.ProjectName = ProjectName;
                ViewBag.ManagerProject = ManagerProject;
                ViewBag.Address = Address;
                // ViewBag.FromDate = @String.Format("{0:yyyy-MM-dd}", FromDate);
                //  ViewBag.ToDate = @String.Format("{0:yyyy-MM-dd}", ToDate);
                //  ViewBag.Status = Status;
                ViewData["User"] = Ql.Users.Where(x => x.Status != 1 && x.IsDeleted == false).ToList();
                return View();
            }
            else
            {
                Ql.AddProject_(ProjectSymbol, ProjectName, ManagerProject, Address, null, null, Status);
                return RedirectToAction("Department", "Department");
            }
        }
        // [AuthorizationViewHandler]
        [HasCredential(RoleID = "EDIT_DEPARTMENT")]
        public ActionResult EditDepartment(int Id)
        {
            var lstType = Ql.DeviceTypes.ToList();
            ViewData["DeviceOfProjectAll"] = Ql.DeviceOfProjectAll(Id).ToList();
            var lstDeviceInProject = Ql.DeviceOfProjectAll(Id).ToList();
            ViewData["DeviceOfProjects"] = Ql.DeviceOfProjects.Where(x => x.ProjectId == Id).ToList();
            var tempList = lstDeviceInProject.GroupBy(k => k.TypeOfDevice).ToList();
            var map = new Dictionary<string, int>();
            foreach (var i in tempList)
            {
                var typeName = lstType.FirstOrDefault(k => k.Id == i.Key).TypeName;
                map.Add(typeName, i.Count());
            }
            ViewData["CountingDeviceType"] = map;
            ViewData["User"] = Ql.Users.Where(x => x.Status != 1 && x.IsDeleted == false).ToList();
            return View(Ql.ProjectDKCs.Find(Id));
        }

        [HttpPost]
        [HasCredential(RoleID = "EDIT_DEPARTMENT")]
        public ActionResult EditDepartment(FormCollection colection, ProjectDKC Project)
        {
            int? Id = colection["Id"].Equals("0") ? (int?)null : Convert.ToInt32(colection["Id"]);
            String ProjectSymbol = colection["ProjectSymbol"];
            String ProjectName = colection["ProjectName"];
            int? ManagerProject = colection["ManagerProject"].Equals("0") ? (int?)null : Convert.ToInt32(colection["ManagerProject"]);
            String Address = colection["Address"];
            //   DateTime? FromDate = colection["FromDate"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["FromDate"]);
            //  DateTime? ToDate = colection["ToDate"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["ToDate"]);
            DateTime? CreatedDate = colection["CreatedDate"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["CreatedDate"]);
            DateTime ModifiedDate = Convert.ToDateTime(colection["ModifiedDate"]);
            int? Status = colection["Status"].Equals("0") ? (int?)null : Convert.ToInt32(colection["Status"]);
            bool IsDeleted = Convert.ToBoolean(colection["IsDeleted"]);
            Ql.UpdateProject(Id, ProjectSymbol, ProjectName, ManagerProject, Address, null, null, CreatedDate, ModifiedDate, Status, IsDeleted);
            return RedirectToAction("EditDepartment", "Department");
        }

        //  [AuthorizationViewHandler]
        [HasCredential(RoleID = "DELETE_DEPARTMENT")]
        public JsonResult DeleteDepartment(string Id)
        {
            bool checkIsset = true;
            bool result = false;
            string[] arrId = Id.Split(',');
            foreach (string i in arrId)
            {
                int prjid = Convert.ToInt32(i);
                var dv = Ql.DeviceOfProjects.Where(x => x.ProjectId == prjid && x.Status == 1).ToList();
                if (dv.Count() > 0)
                {
                    checkIsset = false;
                    break;
                }
            }
            if (checkIsset)
            {
                string a = "," + Id + ",";
                int checkdele = Ql.DeleteProject1(a);
                if (checkdele > 0)
                    result = true;
            }
            else
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //   [AuthorizationViewHandler]
        [HasCredential(RoleID = "DELETE_DEPARTMENT")]
        public JsonResult DeleteDepartment1(int Id)
        {
            bool result = false;
            var dv = Ql.DeviceOfProjects.Where(x => x.ProjectId == Id && x.Status == 1);
            if (dv.Count() == 0)
            {
                result = true;
                Ql.DeleteProject(Id);
            }
            else
            {
                result = false;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //  [AuthorizationViewHandler]
        [HasCredential(RoleID = "ADD_DEVICEINPROJECT")]
        public ActionResult AddDeviceInDepartment(int Id, int DeviceType)
        {
            ViewData["DeviceType"] = Ql.DeviceTypes.ToList();
            if (DeviceType != 0)
            {
                ViewData["Device"] = Ql.SearchDevice(0, DeviceType, null, null, null).Where(x => x.StatusRepair != 1).ToList();
            }
            else
            {
                ViewData["Device"] = Ql.SearchDevice(0, null, null, null, null).Where(x => x.StatusRepair != 1).ToList();
            }

            ViewData["DeviceOfProjectAll"] = Ql.DeviceOfProjectAll(Id).ToList();
            var lstType = Ql.DeviceTypes.ToList();
            var lstDeviceInProject = Ql.DeviceOfProjectAll(Id).ToList();
            var tempList = lstDeviceInProject.GroupBy(k => k.TypeOfDevice).ToList();
            var map = new Dictionary<string, int>();
            foreach (var i in tempList)
            {
                var typeName = lstType.FirstOrDefault(k => k.Id == i.Key).TypeName;
                map.Add(typeName, i.Count());
            }
            ViewData["CountingDeviceType"] = map;
            return View(Ql.ProjectDKCs.Find(Id));
        }
        //   [AuthorizationViewHandler]
        [HasCredential(RoleID = "ADD_DEVICEINPROJECT")]
        public ActionResult AddDeviceInDepartment1(int Idpr, int Iddv, String Notes)
        {
            bool result = true;
            //Check xem thiết bị có phải là thiết bị con nằm trong
            var Check = Ql.DeviceDevices.Where(x => x.DeviceCodeChildren == Iddv & x.IsDeleted == false & x.TypeComponant == 1).Select(x => x.DeviceCodeChildren).Count();
            if (Check > 0)
            {
                result = false;
            }
            else
            {
                int checkdele = Ql.AddDeviceOfProject(Idpr, Iddv, Notes);
                if (checkdele > 0)
                    result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HasCredential(RoleID = "RETURN_DEVICETODEPOT")]
        public ActionResult ReturnDeviceInDepartment(int Idpr, int Iddv, String notes)
        {
            bool result = true;
            var check = 0;
            //Check xem có phải là thiết bị con nằm trong 
            check += Ql.DeviceDevices.Where(x => x.DeviceCodeChildren == Iddv & x.IsDeleted == false & x.TypeComponant == 1).Count();
            if (check > 0)
            {
                result = false;
            }
            else
            {
                int checkdele = Ql.ReturnDeviceOfProject(Idpr, Iddv, notes);
                if (checkdele > 0)
                    result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //   [AuthorizationViewHandler]
        [HasCredential(RoleID = "ADD_DEVICEINPROJECT")]
        public JsonResult AddDeviceDepartmentAll(string Id, int PJ, String Nt)
        {
            bool result = true;
            var IdParent = Id.Split(',');
            var check = 0;
            for (int i = 0; i < IdParent.Length; i++)
            {
                var IdP = Convert.ToInt32(IdParent[i]);
                //Check thiết bị xem có phải là thiết bị con nằm trong
                check += Ql.DeviceDevices.Where(x => x.DeviceCodeChildren == IdP & x.IsDeleted == false & x.TypeComponant == 1).Count();
            }
            if (check > 0)
            {
                result = false;
            }
            else
            {
                // Lấy danh sách thiết bị con từ danh sách thiết bị cha
                for (int i = 0; i < IdParent.Length; i++)
                {
                    var IdP = Convert.ToInt32(IdParent[i]);
                    var lstComponant = Ql.DeviceDevices.Where(x => x.DeviceCodeParents == IdP & x.IsDeleted == false & x.TypeComponant == 1).Select(x => x.DeviceCodeChildren).ToList();
                    foreach (var item in lstComponant)
                    {
                        // Thêm thiết bị con vào DeviceOfProject
                        Ql.AddDeviceOfProject(PJ, Convert.ToInt32(item), "");
                    }
                }
                //Thêm thiết bị cha vào DeviceOfProject
                var lstId = Id.Split(',');
                for (int i = 0; i < lstId.Length; i++)
                {
                    if (!lstId[i].Equals(""))
                        Ql.AddDeviceOfProject(PJ, Convert.ToInt32(lstId[i]), Nt);
                }
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // [AuthorizationViewHandler]
        [HasCredential(RoleID = "RETURN_DEVICETODEPOT")]
        public JsonResult ReturnDeviceDepartmentAll(string Id, int PJ, string notes)
        {
            bool result = true;
            var lstId = Id.Split(',');
            var check = 0;
            for (int i = 0; i < lstId.Length; i++)
            {
                var IdP = Convert.ToInt32(lstId[i]);
                //Check thiết bị xem có phải là thiết bị con nằm trong
                check += Ql.DeviceDevices.Where(x => x.DeviceCodeChildren == IdP & x.IsDeleted == false & x.TypeComponant == 1).Count();
            }
            if (check > 0)
            {
                result = false;
            }
            else
            {
                // Lấy danh sách thiết bị con từ danh sách thiết bị cha
                for (int i = 0; i < lstId.Length; i++)
                {
                    var IdP = Convert.ToInt32(lstId[i]);
                    var lstComponant = Ql.DeviceDevices.Where(x => x.DeviceCodeParents == IdP & x.IsDeleted == false & x.TypeComponant == 1).Select(x => x.DeviceCodeChildren).ToList();
                    foreach (var item in lstComponant)
                    {
                        // Trả thiết bị con về kho                      
                        Ql.ReturnDeviceOfProject(PJ, Convert.ToInt32(item), notes);
                    }
                }
                // var lstId = Id.Split(',');
                for (int i = 0; i < lstId.Length; i++)
                {
                    if (!lstId[i].Equals(""))
                        Ql.ReturnDeviceOfProject(PJ, Convert.ToInt32(lstId[i]), notes);
                }
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult StatisticsDevicesInDepartment()
        {
            return View();
        }

        public static string HtmlToPlainText(string html)
        {
            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

            var text = html;
            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);
            //Remove tag whitespace/line breaks
            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br /> with line breaks
            text = lineBreakRegex.Replace(text, Environment.NewLine);
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);
            return text;
        }


        public class NewConfig
        {
            // Auto-Initialized properties  
            public string DeviceCode { get; set; }
            public string DeviceName { get; set; }
            public string TypeName { get; set; }
            public string Configuration { get; set; }
            public string DateOfDelivery { get; set; }
            public string Notes { get; set; }
            public string StatusRepair { get; set; }
        }

        public string Get_Status(int? status)
        {
            var st = "";
            if (status == 1)
            {
                st = "Đang sửa";
            }
            return st;
        }
        [HasCredential(RoleID = "EXPORT_DV_IN_DEPARTMENT")]
        public JsonResult ExportToExcel(int? IdProject)
        {
            Ql.Configuration.ProxyCreationEnabled = false;
            var charts = Ql.DeviceOfProjectAll(IdProject).Select(i => new { i.DeviceCode, i.DeviceName, i.TypeName, i.Configuration, i.DateOfDelivery, i.StatusRepair, i.Notes }).ToList();
            var model = charts.ToList();
            var a = "";
            List<NewConfig> numbers = new List<NewConfig>();
            for (int i = 0; i < model.Count; ++i)
            {
                var status_rp = Get_Status(model[i].StatusRepair);
                a = HtmlToPlainText(model[i].Configuration);
                var b = @String.Format("{0:dd-MM-yyyy}", model[i].DateOfDelivery);
                //var NewConfig = model[i].Configuration.Replace(model[i].Configuration, a);
                //new NewConfig { DeviceCode = model[i].DeviceCode, DeviceName = model[i].DeviceName, TypeName=  model[i].TypeName, Configuration = a, DateOfDelivery = model[i].DateOfDelivery, Notes = model[i].Notes };            
                numbers.Add(new NewConfig { DeviceCode = model[i].DeviceCode, DeviceName = model[i].DeviceName, TypeName = model[i].TypeName, Configuration = a, DateOfDelivery = b, Notes = model[i].Notes, StatusRepair = status_rp });
            }
            using (StringWriter sw = new StringWriter())
            {
                using (System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw))
                {
                    GridView grid = new GridView();
                    grid.DataSource = numbers;
                    grid.DataBind();
                    Response.ClearContent();
                    Response.Buffer = true;
                    string strDateFormat = string.Empty;
                    strDateFormat = string.Format("{0:dd/MM/yyyy}", DateTime.Now);
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
                ViewBag.Sw,
            }, JsonRequestBehavior.AllowGet);
        }

        //[AuthorizationViewHandler]
        [HasCredential(RoleID = "ADD_DEVICEINPROJECT")]
        public ActionResult AddDeviceInDepartmentMachine(int Id)
        {
            return View(Ql.ProjectDKCs.Find(Id));
        }
        [HttpGet]
        [HasCredential(RoleID = "ADD_DEVICEINPROJECT")]
        public JsonResult GetDeviceInDepartmentMachine(string dvc)
        {
            Ql.Configuration.ProxyCreationEnabled = false;
            int? Result = 0;
            var stt = Ql.Devices.Where(x => x.DeviceCode.Trim() == dvc).Select(x => x.Status).First();
            if (stt == 0)
            {
                var findDvCode = Ql.Devices.Where(x => x.DeviceCode.Trim() == dvc).Select(x => x.Id).First();
                var check = Ql.DeviceDevices.Where(x => x.DeviceCodeChildren == findDvCode & x.IsDeleted == false).Count();
                if (check > 0)
                {
                    Result = 4;
                }
                else
                {
                    ViewBag.Scaner = Ql.SearchDevice(null, null, null, null, dvc).First();
                }
            }
            else
            { Result = stt; }
            var model = ViewBag.Scaner;
            var result = new { Result, model };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HasCredential(RoleID = "RETURN_DEVICETODEPOT")]
        public ActionResult ReturnDeviceInDepartmentMachine(int Id)
        {
            return View(Ql.ProjectDKCs.Find(Id));
        }
        [HttpGet]
        [HasCredential(RoleID = "RETURN_DEVICETODEPOT")]
        public JsonResult GetReturnDeviceInDepartmentMachine(string dvc, int pjId)
        {
            Ql.Configuration.ProxyCreationEnabled = false;
            int Result = 0;
            if (Ql.SearchDevice(null, null, null, pjId, null).Where(x => x.DeviceCode.Trim() == dvc).Count() == 1)
            {
                var findDvCode = Ql.Devices.Where(x => x.DeviceCode.Trim() == dvc).Select(x => x.Id).First();
                var check = Ql.DeviceDevices.Where(x => x.DeviceCodeChildren == findDvCode & x.IsDeleted == false).Count();
                if (check > 0)
                {
                    Result = 2;
                }
                else
                {
                    ViewBag.Scaner = Ql.SearchDevice(null, null, null, pjId, dvc).First();
                    Result = 3;
                }
            }
            else
            {
                Result = 1;
            }
            var model = ViewBag.Scaner;
            var result = new { Result, model };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //    [AuthorizationViewHandler]
        [HasCredential(RoleID = "VIEW_STATISTICAL_DEPARTMENT")]
        public ActionResult StatisticDepartment()
        {
            ViewData["StatisticProject"] = Ql.StatisticProject().ToList();
            return View();
        }
    }
}