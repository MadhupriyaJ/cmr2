using Core;
using FramWork.HelloService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FramWork.Controllers
{
    public class ExceptionManagmentController : Controller
    {

        #region  Exception
        [LoginAuthorize]
        public ActionResult Exceptions()
        {
            return View();
        }
        #endregion

        #region SystemExceptions
        [LoginAuthorize]
        public ActionResult SystemExceptions()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                 this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into System Exceptions Page.");
            return View();
        }
        #endregion

        #region  Document Exceptions
        [LoginAuthorize]
        public ActionResult DocumentExceptions()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
     this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Document Exceptions Page.");
            return View();
        }
        #endregion
    }
}