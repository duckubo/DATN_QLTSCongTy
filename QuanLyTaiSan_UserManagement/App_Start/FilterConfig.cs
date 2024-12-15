using System.Web;
using System.Web.Mvc;

namespace QuanLyTaiSan_UserManagement
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
