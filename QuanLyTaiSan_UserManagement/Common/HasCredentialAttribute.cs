using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyTaiSan_UserManagement.Models;
using QuanLyTaiSan_UserManagement.Common;
using System.Web.Routing;

namespace QuanLyTaiSan_UserManagement
{
    //Kế thừa AuthorizeAttribute của entity framework
    public class HasCredentialAttribute : AuthorizeAttribute
    {
        public string RoleID { set; get; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var session = (UserLoginSec)HttpContext.Current.Session[CommonConstants.USER_SESSION];
            if (session == null)
            {
                return false;
            }
          
            List<string> privilegeLevels = this.GetCredentialByLoggedInUser(session.UserName.Trim()); // Call another method to get rights of the user from DB
            // Contains kiểm tra RoleID có nằm trong List privilegeLevels(List quyền của ng dùng)
            if (privilegeLevels.Contains(this.RoleID) || session.GroupID == CommonConstants1.ADMIN_GROUP)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Shared/Error401.cshtml" 
             
            };
        }
        private List<string> GetCredentialByLoggedInUser(string userName)
        {
          //  List<string> privilegeLevelsNew = UserDao.GetListCredential(session.UserName.Trim());
            var credentials = (List<string>)HttpContext.Current.Session[CommonConstants.SESSION_CREDENTIALS];
            return credentials;
        }
    }
}