using ExceptionLogging;
using FramWork.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FramWork.Controllers
{
    public class LogoutController : Controller
    {
        FrameworkEntities db = new FrameworkEntities();

        // GET: Logout

        #region Index
        public ActionResult Index()
        {

            var isSession = Request.QueryString["type"];
            var red = Request.QueryString["redirect"];

            try
            {
                var data = db.UserSessions.Where(a => a.UserId == Core.Login.Id && a.SessionCode == Session.SessionID && a.Status == false);

                if (data != null && data.Count() > 0)
                {
                    foreach (var item in data)
                    {
                        item.Status = true;
                    }
                    db.SaveChanges();

                }

                var browser = Request.Browser;
                Core.Basic.LogEntryToDb(Core.LogTypes.Logout, "User name: " + Core.Login.Username + " requested logout via" + (isSession != null && isSession == "1" ? "  session expire" : " logout button") + Environment.NewLine +
                               "From Ip: " + Request.UserHostAddress + Environment.NewLine +
                               "Browser: " + browser.Type + " " + browser.Browser + " " + browser.Version + Environment.NewLine +
                               "Platform: " + browser.Platform);


                if (Core.Login.Logout())
                {
                    if (isSession != null && isSession == "1")
                        TempData["msg"] = new string[] { "0", Resources.Resources.LogoutSessionExpired };
                    else
                        TempData["msg"] = new string[] { "1", Resources.Resources.GeneralMsgLoggedOut };




                    if (red != null)
                        return Redirect("~/Login?redirect=" + red);
                    else
                        return RedirectToAction("Index", "Login");
                }

            }
            catch (Exception ex)
            {
                Core.Basic.WriteLog("Logout_index_42", ex.Message, ex.ToString());
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion

            }

            TempData["msg"] = new string[] { "0", Resources.Resources.GeneralMsgLoggedOutFailed };
            return RedirectToAction("Index", "Login");
        }
        #endregion
      
      
    }
}