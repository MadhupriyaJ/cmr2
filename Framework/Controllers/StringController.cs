using Core;
using ExceptionLogging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FramWork.Controllers
{
    public class StringController : Controller
    {

        #region Index
        [LoginAuthorize]
        // GET: String
        public ActionResult Index(FormCollection collection)
        {
            try
            {
                if (collection["selAuthMode"] == "0" || collection["txtServer"] == "" || collection["txtDb"] == "")
                {
                    ViewBag.Error = "Please Provide Complete Information!";
                    return View();
                }
                if (collection["selAuthMode"].ToString() == "1" && collection["txtUserName"] == "" || collection["txtPassword"] == "")
                {
                    ViewBag.Error = "Please Provide Complete Information!";
                    return View();
                }


                string ConnectionString;
                if (collection["selAuthMode"].ToString() == "2")//windows authentication
                {
                    ConnectionString = "data source=" + collection["txtServer"].ToString() + ";initial catalog=" + collection["txtDb"].ToString() + ";integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";
                }
                else
                {
                    ConnectionString = "data source=" + collection["txtServer"].ToString() + ";initial catalog=" + collection["txtDb"].ToString() + ";user id=" + collection["txtUserName"].ToString() + ";password=" + collection["txtPassword"].ToString() + ";MultipleActiveResultSets=True;App=EntityFramework";
                }




                if (ConnectionString != null && collection["optionsRadios2"] == "E")
                {
                    ViewBag.Msg = Basic.Encrypt(ConnectionString);//Encrypted Connection String
                }
                else
                {
                    ViewBag.Msg = ConnectionString;//Non Encryption Connection String
                }

                try
                {
                    ViewBag.Msgs = User.Identity.Name;
                }
                catch (Exception ex)
                {
                    #region Exception Logging
                    var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                    var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                    if (!Directory.Exists(dir))  // if it doesn't exist, create
                        Directory.CreateDirectory(dir);
                    System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                    #endregion
                }
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion
            }
            return View();
        }
        #endregion
        
    }
}