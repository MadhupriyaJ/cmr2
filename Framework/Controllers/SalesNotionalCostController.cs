using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FramWork.Controllers
{
    public class SalesNotionalCostController : Controller
    {
        #region SalesNotionalCost
        // GET: SalesNotionalCost
        [LoginAuthorize]
        public ActionResult SalesNotionalCost()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
     this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Enterd into Sales Notional Cost Page");
            return View();
        }
        #endregion
     
    }
}