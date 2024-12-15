using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using QuanLyTaiSan_UserManagement.Helper;
using QuanLyTaiSan_UserManagement.Models;

namespace QuanLyTaiSan_UserManagement.Attribute
{
    public class AuthorizationHandler : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var identity = filterContext.RequestContext.HttpContext.User.Identity;
            //get list role
            var lstRole = ((ClaimsIdentity)identity).Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value).ToList();
            if (!lstRole.Any())
            {
                RequestError(filterContext);
            }
            else if (lstRole.Contains("Admin"))
            {
                //get all action able to using of role
                var sql =
                    "SELECT Id, ControllerName, ActionName FROM SystemFeature";
                var lstRecords = new ApplicationDbContext().Database.SqlQuery<SystemFeature>(sql).ToList();

                HttpContext.Current.Session["AuthenticationFeature"] = lstRecords;
                base.OnActionExecuting(filterContext);
            }
            else
            {
                //get all action able to using of role
                var sql =
                    "SELECT s.* FROM SystemFeature s WHERE s.Id IN (SELECT u.FeatureId FROM UserAuthorization u WHERE u.RoleName = '" +
                    lstRole.FirstOrDefault() + "')";
                var lstRecords = new ApplicationDbContext().Database.SqlQuery<SystemFeature>(sql).ToList();

                HttpContext.Current.Session["AuthenticationFeature"] = lstRecords;
                base.OnActionExecuting(filterContext);
            }

            //string actionName = filterContext.ActionDescriptor.ActionName;
            //string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            //var a = CanAccess(filterContext.RequestContext.HttpContext.User.Identity, controllerName, actionName);
            //if (a)
            //    base.OnActionExecuting(filterContext);
            //else
            //    RequestError(filterContext);
        }
        private void RequestError(ActionExecutingContext filterContext)
        {
            var messageError = "Bạn không có quyền thực hiện thao tác này";
            var result = new ViewResult { ViewName = "Error" };
            result.ViewBag.Message = "Bạn không có quyền thực hiện thao tác này";
            result.ViewBag.ErrorMessage = messageError;
            filterContext.Result = result;
        }
        private bool CanAccess(IIdentity identity, string controller, string action)
        {
            //get list role
            var lstRole = ((ClaimsIdentity)identity).Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value);

            //Store action user able to use
            var lstAction = new List<SystemFeature>();
            //get all action able to using of role
            foreach (var item in lstRole)
            {
                var lstRecords = new UserAuthorizationDatabseAction().GetFeaturesByRoleName(item);
                lstAction.AddRange(lstRecords);
            }
            return lstAction
                .Any(k => k.ControllerName.EqualsIgnoringCase(controller) &&
                          k.ActionName.EqualsIgnoringCase(action)); 
        }
    }
}