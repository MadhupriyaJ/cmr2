using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FramWork.Controllers
{
    public class CommisionController : Controller
    {
        #region Product Commision
        [LoginAuthorize]
        public ActionResult ProductCommision()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                            this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Add Reward Processing Page");
            return View();
        }
        #endregion

        #region Commission Rules (Sales Agent)
        [LoginAuthorize]
        public ActionResult CommisionRuleSalesAgent()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                              this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Add Sales Agent Comission Rule Page");

            return View();
        }
        #endregion
    }
}