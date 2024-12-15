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

    [HasCredential(RoleID = "")]
    public class ProjectController : Controller
    {
        QuanLyTaiSanCtyEntities Ql = new QuanLyTaiSanCtyEntities();

        public ActionResult Project()
        {
            ViewBag.ProjectNb = Ql.SearchProject(null, null, 0,null).Count();
            ViewData["User"] = Ql.Users.ToList();
            var lstProject = Ql.SearchProject(null, null, 0,null).ToList();
            return View(lstProject);
        }
        [HttpPost]
        public ActionResult SeachProject(FormCollection colection, ProjectDKC Project)
        {
            ViewData["User"] = Ql.Users.ToList();
            String ProjectSymbol = colection["ProjectSymbol"].Trim();
            int? Status = colection["Status"].Equals("0") ? (int?)null : Convert.ToInt32(colection["Status"]);
            int? ManagerProject = colection["ManagerProject"].Equals("0") ? (int?)null : Convert.ToInt32(colection["ManagerProject"]);
            var lstProject = Ql.SearchProject(ManagerProject, Status,0, ProjectSymbol).ToList();
            var ViewProject = lstProject;
            ViewBag.Status = Status;
            ViewBag.ManagerProject = ManagerProject;
            ViewBag.ProjectSymbol = ProjectSymbol;
            ViewBag.ProjectNb = Ql.SearchProject(ManagerProject, Status,0, ProjectSymbol).Count();
            return View("Project", ViewProject);
        }

        public ActionResult AddProject()
        {
            ViewData["User"] = Ql.Users.Where(x => x.Status != 1 && x.IsDeleted != true).ToList();
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult AddRepairType(string TypeName, string Notes)
        {
            Ql.AddRepairType(TypeName, Notes);
            bool result = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddProject(FormCollection colection, ProjectDKC Project)
        {

            String ProjectSymbol = colection["ProjectSymbol"];
            String ProjectName = colection["ProjectName"];
            int? ManagerProject = colection["ManagerProject"].Equals("0") ? (int?)null : Convert.ToInt32(colection["ManagerProject"]);
            String Address = colection["Address"];
            DateTime? FromDate = colection["FromDate"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["FromDate"]);
            DateTime? ToDate = colection["ToDate"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["ToDate"]);
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
                ViewBag.FromDate = @String.Format("{0:yyyy-MM-dd}", FromDate);
                ViewBag.ToDate = @String.Format("{0:yyyy-MM-dd}", ToDate);
                ViewBag.Status = Status;
                ViewData["User"] = Ql.Users.Where(x => x.Status != 1 && x.IsDeleted != true).ToList();
                return View();
            }
            else
            {
                Ql.AddProject_(ProjectSymbol, ProjectName, ManagerProject, Address, FromDate, ToDate, Status);
                return RedirectToAction("Project", "Project");
            }
        }

        public ActionResult EditProject(int Id)
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
            ViewData["User"] = Ql.Users.Where(x => x.Status != 1 && x.IsDeleted != true).ToList();
            return View(Ql.ProjectDKCs.Find(Id));
        }

        [HttpPost]
        public ActionResult EditProject(FormCollection colection, ProjectDKC Project)
        {
            int? Id = colection["Id"].Equals("0") ? (int?)null : Convert.ToInt32(colection["Id"]);
            String ProjectSymbol = colection["ProjectSymbol"];
            String ProjectName = colection["ProjectName"];
            int? ManagerProject = colection["ManagerProject"].Equals("0") ? (int?)null : Convert.ToInt32(colection["ManagerProject"]);
            String Address = colection["Address"];
            DateTime? FromDate = colection["FromDate"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["FromDate"]);
            DateTime? ToDate = colection["ToDate"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["ToDate"]);
            DateTime? CreatedDate = colection["CreatedDate"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["CreatedDate"]);
            DateTime ModifiedDate = Convert.ToDateTime(colection["ModifiedDate"]);
            int? Status = colection["Status"].Equals("0") ? (int?)null : Convert.ToInt32(colection["Status"]);
            bool IsDeleted = Convert.ToBoolean(colection["IsDeleted"]);
            Ql.UpdateProject(Id, ProjectSymbol, ProjectName, ManagerProject, Address, FromDate, ToDate, CreatedDate, ModifiedDate, Status, IsDeleted);
            return RedirectToAction("EditProject", "Project");
        }

        public JsonResult DeleteProject(string Id)
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

        public JsonResult DeleteProject1(int Id)
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

        public ActionResult AddDeviceInProject(int Id, int DeviceType)
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

        public ActionResult AddDeviceInProject1(int Idpr, int Iddv, String Notes)
        {
            bool result = false;
            int checkdele = Ql.AddDeviceOfProject(Idpr, Iddv, Notes);
            if (checkdele > 0)
                result = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReturnDeviceInProject(int Idpr, int Iddv, string notes)
        {
            bool result = false;
            int checkdele = Ql.ReturnDeviceOfProject(Idpr, Iddv, notes);
            if (checkdele > 0)
                result = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddDeviceProjectAll(string Id, int PJ)
        {

            var lstId = Id.Split(',');
            for (int i = 0; i < lstId.Length; i++)
            {
                if (!lstId[i].Equals(""))
                    Ql.AddDeviceOfProject(PJ, Convert.ToInt32(lstId[i]), "");
            }
            bool result = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ReturnDeviceProjectAll(string Id, int PJ, string notes)
        {
            var lstId = Id.Split(',');
            for (int i = 0; i < lstId.Length; i++)
            {
                if (!lstId[i].Equals(""))
                    Ql.ReturnDeviceOfProject(PJ, Convert.ToInt32(lstId[i]), notes);
            }
            bool result = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult StatisticsDevicesInProject()
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
            public string DeviceCode { get; set; }
            public string DeviceName { get; set; }
            public string TypeName { get; set; }
            public string Configuration { get; set; }
            public string Notes { get; set; }
            public string Status { get; set; }
            public string DateOfDelivery { get; set; }
        }
       public string Get_Status(int? status) {
            var st = "";
            if (status ==  1)
            {
                st = "Đang sửa";
            }   
            return st;
        }

        public JsonResult ExportToExcel(int? IdProject)
        {
            Ql.Configuration.ProxyCreationEnabled = false;
            var charts = Ql.DeviceOfProjectAll(IdProject).Select(i => new { i.DeviceCode, i.DeviceName, i.TypeName, i.Configuration, i.DateOfDelivery, i.Notes,i.StatusRepair }).ToList();
            var model = charts.ToList();
          
            var a = "";
            List<NewConfig> numbers = new List<NewConfig>();
            for (int i = 0; i < model.Count; ++i)
            {
              var  status =  Get_Status(model[i].StatusRepair);
                a = HtmlToPlainText(model[i].Configuration);
                var b = @String.Format("{0:dd-MM-yyyy}", model[i].DateOfDelivery);
                //var NewConfig = model[i].Configuration.Replace(model[i].Configuration, a);                                                       
                //new NewConfig { DeviceCode = model[i].DeviceCode, DeviceName = model[i].DeviceName, TypeName=  model[i].TypeName, Configuration = a, DateOfDelivery = model[i].DateOfDelivery, Notes = model[i].Notes };            
                numbers.Add(new NewConfig { DeviceCode = model[i].DeviceCode, DeviceName = model[i].DeviceName, TypeName = model[i].TypeName, Configuration = a, DateOfDelivery = b, Notes = model[i].Notes, Status = status });
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

        public ActionResult AddDeviceInProjectMachine(int Id)
        {
            return View(Ql.ProjectDKCs.Find(Id));
        }
        [HttpGet]
        public JsonResult GetDeviceInProjectMachine(string dvc)
        {
            Ql.Configuration.ProxyCreationEnabled = false;
            bool Result = false;
            if (Ql.SearchDevice(null, null, null, null, null).Where(x => x.DeviceCode.Trim() == dvc).Count() == 1)
            {
                ViewBag.Scaner = Ql.SearchDevice(null, null, null, null, dvc).First();
                Result = true;
            }
            else Result = false;
            var model = ViewBag.Scaner;
            var result = new { Result, model };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReturnDeviceInProjectMachine(int Id)
        {
            return View(Ql.ProjectDKCs.Find(Id));
        }
        [HttpGet]
        public JsonResult GetReturnDeviceInProjectMachine(string dvc, int pjId)
        {
            Ql.Configuration.ProxyCreationEnabled = false;
            bool Result = false;
            if (Ql.SearchDevice(null, null, null, pjId, null).Where(x => x.DeviceCode.Trim() == dvc).Count() == 1)
            {
                ViewBag.Scaner = Ql.SearchDevice(null, null, null, pjId, dvc).First();
                Result = true;
            }
            else Result = false;
            var model = ViewBag.Scaner;
            var result = new { Result, model };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StatisticProject()
        {
            ViewData["StatisticProject"] = Ql.StatisticProject().ToList();
            return View();

        }
    }
}