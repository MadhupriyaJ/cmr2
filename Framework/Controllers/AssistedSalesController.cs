using Core;
using FramWork.HelloService;
using FramWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using System.IO;
using ExceptionLogging;

namespace FramWork.Controllers
{
    public class AssistedSalesController : Controller
    {
        // GET: AssistedSales
        #region salesliterature
        [LoginAuthorize]
        public ActionResult salesliterature()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                  this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Sales Literature Page");
            return View();
        }
        #endregion

        #region Required Documents
        [LoginAuthorize]
        public ActionResult RequiredDocuments(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                FrameworkEntities dc = new FrameworkEntities();
                int pageSize = 10;
                int pageNumber = (page ?? 1);

                var list = dc.Campaigns.AsEnumerable().Where(a => a.OrgId == Login.OrganizationId && a.MarkAsDelete == false).ToPagedList(pageNumber, pageSize);

                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Required Documents Page");
                return View(list);
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
            int pageNumberone = (page ?? 1);
            List<Campaign> lst = new List<Campaign>();
            return View(lst.ToPagedList(pageNumberone, 10));

        }
        #endregion


        #region What If Calculator
        [LoginAuthorize]
        public ActionResult WhatIfCalculator()
        {

            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into  What If Calculator Page");
            return View();
        }
        #endregion

        #region Eligibility Calculator
        [LoginAuthorize]
        public ActionResult EligibilityCalculator()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
          this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into  Eligibility Calculator Page");
            return View();
        }
        #endregion

        #region Sales Script
        [LoginAuthorize]
        public ActionResult SalesScript()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Sales Script Page");
            return View();
        }
        #endregion

        #region  Financial Analysis
        [LoginAuthorize]
        public ActionResult FinancialAnalysis()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                  this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Financial Analysis Page");
            return View();
        }
        #endregion

        #region FAQ
        [LoginAuthorize]
        public ActionResult FAQ()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
        this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into FAQ Page");
            return View();
        }
        #endregion
    }
}