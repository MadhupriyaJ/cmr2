using FramWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core
{
    public class LoginAuthorizeAttribute : AuthorizeAttribute
    {

        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            Type type = filterContext.Controller.GetType();
            ActionDescriptor desc = filterContext.ActionDescriptor;

            string module = type.Name.ToLower() + "." + desc.ActionName.ToLower() + (filterContext.HttpContext.Request.HttpMethod == "POST" ? ".post" : ".get");

            IEnumerable<RoleAssignedModules> data = null;

            try
            {
                // also copy of the code exists in login/keep

                if (!Login.IsLogin())
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        throw new HttpException(403, "Login Please");
                    }
                    else
                        filterContext.Result = new RedirectResult("~/Login?redirect=" + filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.RawUrl));
                }

                FrameworkEntities db = new FrameworkEntities();
                var user = db.Users.Find(Login.Id);
                var session = db.UserSessions.FirstOrDefault(a => a.UserId == Login.Id && a.SessionCode == HttpContext.Current.Session.SessionID && a.SessionActiveDate > DateTime.UtcNow && a.Status == false);
                if (session == null)
                {
                    filterContext.Result = new RedirectResult("~/logout?type=1&redirect=" + filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.RawUrl));
                }
                else
                {
                    session.SessionActiveDate = session.SessionActiveDate.AddMinutes(Convert.ToDouble(user.SessionExpireTime));
                }

                db.SaveChanges();

                data = db.RoleAssignedModules.Where(a => a.Modules.Code== module);

            }
            catch (Exception ex)
            {
                Core.Basic.WriteLog("Authorize_onAuthorization_42", ex.Message, ex.ToString());
            }

            if (data == null || data.Count() < 1)
            {
                //if (filterContext.HttpContext.Request.IsAjaxRequest())
                //{
                //    throw new HttpException(401, "Are you sure you're in the right place?");
                //}
                //else
                //{
                //    if (Login.IsLogin())
                //        filterContext.Result = new RedirectResult("~/Login/Permission?redirect=" + filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.RawUrl));
                //    else
                //        filterContext.Result = new RedirectResult("~/Login?redirect=" + filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.RawUrl));

                //}
            }


        }
    }
}
