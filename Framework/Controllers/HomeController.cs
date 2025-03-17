using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FramWork.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home

        #region Index
        [LoginAuthorize]
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region Home Page
        [LoginAuthorize]
        public ActionResult HomePage()
        {
            return View();
        }
        #endregion

    }
}