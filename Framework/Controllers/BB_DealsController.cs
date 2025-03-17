using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FramWork.Models;
using Core;
using PagedList.Mvc;
using PagedList;
using System.IO;
using ExceptionLogging;

namespace FramWork.Controllers
{
    public class BB_DealsController : Controller
    {

        private FrameworkEntities db = new FrameworkEntities();

        #region Index
          [LoginAuthorize]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                this.ControllerContext.RouteData.Values["action"].ToString(),
                "The user " + Login.FirstName + " " + Login.LastName + "Entered into Deals Page");
                FrameworkEntities dc = new FrameworkEntities();
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(db.ABB_RequestFinance.AsEnumerable().Where(a => a.OrgId == Login.OrganizationId && a.MarkAsDelete == false
                     && a.DealSatageId != Convert.ToInt32(DealStagedef.won) && a.DealSatageId != Convert.ToInt32(DealStagedef.lost)
                      && a.DealSatageId != Convert.ToInt32(DealStagedef.dropped)).ToPagedList(pageNumber, pageSize));
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
            List<ABB_RequestFinance> lst = new List<ABB_RequestFinance>();
            return View(lst.ToPagedList(pageNumberone, 10));
        }

        #endregion

        #region PipeLine
          [LoginAuthorize]
        public ActionResult Pipeline(long? Id)
        {
            try
            {
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
               this.ControllerContext.RouteData.Values["action"].ToString(),
               "The user " + Login.FirstName + " " + Login.LastName + "Entered into Pipeline Page");
                FrameworkEntities dc = new FrameworkEntities();
                var info = dc.ABB_RequestFinance.AsEnumerable().Where(a => a.OrgId == Login.OrganizationId && a.MarkAsDelete == false && a.Id == Convert.ToInt64(Id)).FirstOrDefault();
                Session["dealstageId"] = info.DealSatageId;
                Session["requestfinanceid"] = info.Id;
                return View();
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
            return RedirectToAction("Index");
        }

        #endregion


        #region MoveDealSatge
          [LoginAuthorize]
        [HttpPost]
        public bool changedealstage(string Id, string DealstageId)
        {
            try
            {
                if (!string.IsNullOrEmpty(Id) && !String.IsNullOrEmpty(DealstageId))
                {
                    FrameworkEntities dc = new FrameworkEntities();
                    var info = dc.ABB_RequestFinance.AsEnumerable().Where(a => a.OrgId == Login.OrganizationId && a.MarkAsDelete == false && a.Id == Convert.ToInt64(Id)).FirstOrDefault();
                    info.DealSatageId = Convert.ToInt32(DealstageId);
                    dc.SaveChanges();
                    return true;
                }
                return false;
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
            return false;
        }
        #endregion
    }
}