using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FramWork.Controllers
{
    public class NotificationController : Controller
    {
        // GET: Notification

        #region Index
         [LoginAuthorize]
        public ActionResult Index()
        {
            return View();
        }
        #endregion
      
    }
}