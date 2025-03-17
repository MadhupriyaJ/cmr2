using Core;
using FramWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using System.IO;
using ExceptionLogging;

namespace FramWork.Controllers
{
    public class BB_CalendarController : Controller
    {
        [LoginAuthorize]
        public ActionResult Index()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
             this.ControllerContext.RouteData.Values["action"].ToString(),
             "The user " + Login.FirstName + " " + Login.LastName + "Entered into Calendar Page");
            return View();
        }


        #region Add Customer Appoinment
        [LoginAuthorize]
        public ActionResult AddCustomerApp()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion


        #region Add Customer appoinment
        [LoginAuthorize]
        public bool Addappoinment(String Title, String Type, String FromDate, String FromTime, String ToDate, String ToTime, String Allday, String Repeat, String Location, String EventColor
            , String Description, String Owner, String AssignedTo, String sunday, String monday, String tuesday, String wednesday, String thursday, String friday, String Saturday
            , String noenddate, String onthisenddate)
        {
            try
            {
                if (!String.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(FromDate))
                {
                    ABB_Appoinment tbl = new ABB_Appoinment();
                    FrameworkEntities db = new FrameworkEntities();



                    tbl.OrgId = Login.OrganizationId;
                    tbl.Title = Title;
                    if (!String.IsNullOrEmpty(Type))
                        tbl.Type = Convert.ToInt32(Type);
                    var dt = Convert.ToDateTime(FromDate).Date;
                    var t = TimeSpan.Parse(FromTime);
                    var frmdatetime = dt.Add(t);
                    tbl.FromDate_time = frmdatetime;
                    if (!String.IsNullOrEmpty(ToDate))
                    {
                        var Tdt = Convert.ToDateTime(ToDate).Date;
                        var Ttime = TimeSpan.Parse(ToTime);
                        var Todatetime = Tdt.Add(Ttime);
                        tbl.ToDate_time = Todatetime;
                    }

                    tbl.AllDay = Convert.ToBoolean(Allday);
                    tbl.Repeat = Convert.ToBoolean(Repeat);
                    StringBuilder sb = new StringBuilder();

                    if (tbl.Repeat == true)
                    {
                        sb.Append("[");
                        if (Convert.ToBoolean(sunday))
                            sb.Append("0,");
                        if (Convert.ToBoolean(monday))
                            sb.Append("1,");
                        if (Convert.ToBoolean(tuesday))
                            sb.Append("2,");
                        if (Convert.ToBoolean(wednesday))
                            sb.Append("3,");
                        if (Convert.ToBoolean(thursday))
                            sb.Append("4,");
                        if (Convert.ToBoolean(friday))
                            sb.Append("5,");
                        if (Convert.ToBoolean(Saturday))
                            sb.Append("6");
                        sb.Append("]");
                        tbl.RepeatDays = sb.ToString();
                    }

                    if (tbl.Repeat == true && !String.IsNullOrEmpty(noenddate))
                    {
                        tbl.NOEnddate = Convert.ToBoolean(noenddate);
                    }
                    tbl.Location = Location;
                    tbl.EventColor = EventColor;
                    tbl.Description = Description;
                    if (!String.IsNullOrEmpty(Owner))
                    {
                        tbl.Owner = Convert.ToInt32(Owner);
                    }
                    if (!String.IsNullOrEmpty(AssignedTo))
                    {
                        tbl.AssignedTo = Convert.ToInt32(AssignedTo);
                    }

                    tbl.CreatedDate = tbl.ModifiedDate = DateTime.Now;
                    tbl.ModifiedBy = Login.Id;
                    db.ABB_Appoinment.Add(tbl);
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
             this.ControllerContext.RouteData.Values["action"].ToString(),
             "The user " + Login.FirstName + " " + Login.LastName + "Added new Customer Appointment");
                    db.SaveChanges();
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


        #region Get Appoinment Events
        [LoginAuthorize]
        public ActionResult getcompanyappoinments()
        {
            try
            {
                FrameworkEntities db = new FrameworkEntities();
                var eventList = (from c in db.ABB_Appoinment.AsEnumerable()
                                 // where c.Id == CompanyID
                                 select new
                                 {
                                     id = c.Id,
                                     title = c.Title,
                                     start = c.AllDay == true && c.Repeat == true ? c.FromDate_time.ToString("yyyy-MM-dd") : c.AllDay == true ? c.FromDate_time.ToString("yyyy-MM-dd") : (c.Repeat == true ? c.FromDate_time.ToString("HH:mm") : c.FromDate_time.ToString("s")),
                                     end = c.AllDay == true && c.Repeat == true ? c.ToDate_time.ToString("yyyy-MM-dd") : c.AllDay == true ? null : (c.Repeat == true ? c.ToDate_time.ToString("HH:mm") : c.ToDate_time.ToString("s")),
                                     color = c.EventColor,
                                     dow = c.AllDay == true ? null : (c.Repeat == true ? c.RepeatDays : "[]"),
                                     ranges = new { start = c.FromDate_time.AddDays(-1).ToString("yyyy-MM-dd"), end = c.NOEnddate == true ? c.ToDate_time.AddDays(7280).ToString("yyyy-MM-dd") : c.ToDate_time.AddDays(1).ToString("yyyy-MM-dd") },
                                     repeating = c.Repeat,
                                     isallday = c.AllDay

                                 }).ToList();
                return Json(eventList, JsonRequestBehavior.AllowGet);
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
            return Json(null, JsonRequestBehavior.AllowGet);
        }



        #endregion

        #region Today's Task
        [LoginAuthorize]
        public ActionResult TodaysTask(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
           this.ControllerContext.RouteData.Values["action"].ToString(),
           "The user " + Login.FirstName + " " + Login.LastName + "Entered into Todays Task Page");

                var todaysDate = DateTime.Now.Date;
                FrameworkEntities db = new FrameworkEntities();
                var list = db.ABB_Appoinment.AsEnumerable().Where(a => a.OrgId == Login.OrganizationId && a.AssignedTo == Login.Id && todaysDate >= a.FromDate_time && todaysDate <= a.ToDate_time && a.MarkAsDelete == false && a.appStatus == false);
                int pageSize = 20;
                int pageNumber = (page ?? 1);
              
                return View(list.ToPagedList(pageNumber, pageSize));
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
            List<ABB_Appoinment> lst = new List<ABB_Appoinment>();
            return View(lst.ToPagedList(pageNumberone, 10));
        }
        #endregion

        #region  Pending Tasks
        [LoginAuthorize]
        public ActionResult PendingTask(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(),
            "The user " + Login.FirstName + " " + Login.LastName + "Entered into Pending Task Page");

                var todaysDate = DateTime.Now.Date;
                FrameworkEntities db = new FrameworkEntities();
                var list = db.ABB_Appoinment.AsEnumerable()
                    .Where(a => a.OrgId == Login.OrganizationId && a.AssignedTo == Login.Id
                        && a.ToDate_time < todaysDate && a.MarkAsDelete == false && a.appStatus == false);
                int pageSize = 20;
                int pageNumber = (page ?? 1);

                return View(list.ToPagedList(pageNumber, pageSize));
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
            List<ABB_Appoinment> lst = new List<ABB_Appoinment>();
            return View(lst.ToPagedList(pageNumberone, 10));
        }
        #endregion

        #region Future Task
        [LoginAuthorize]
        public ActionResult FutureTask(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(),
            "The user " + Login.FirstName + " " + Login.LastName + "Entered into Future Task Page");

                var todaysDate = DateTime.Now.Date;
                FrameworkEntities db = new FrameworkEntities();
                var list = db.ABB_Appoinment.AsEnumerable()
                    .Where(a => a.OrgId == Login.OrganizationId &&
                        a.AssignedTo == Login.Id && a.ToDate_time > todaysDate && a.MarkAsDelete == false && a.appStatus == false);
                int pageSize = 20;
                int pageNumber = (page ?? 1);
                return View(list.ToPagedList(pageNumber, pageSize));
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
            List<ABB_Appoinment> lst = new List<ABB_Appoinment>();
            return View(lst.ToPagedList(pageNumberone, 10));
        }
        #endregion

        #region Completed Task
        [LoginAuthorize]
        public ActionResult CompletedTask(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
           this.ControllerContext.RouteData.Values["action"].ToString(),
           "The user " + Login.FirstName + " " + Login.LastName + "Entered into Completed Task Page");

                var todaysDate = DateTime.Now.Date;
                FrameworkEntities db = new FrameworkEntities();
                var list = db.ABB_Appoinment.AsEnumerable().Where(a => a.OrgId == Login.OrganizationId && a.AssignedTo == Login.Id && a.MarkAsDelete == false && a.appStatus == true);
                int pageSize = 20;
                int pageNumber = (page ?? 1);
                return View(list.ToPagedList(pageNumber, pageSize));
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
            List<ABB_Appoinment> lst = new List<ABB_Appoinment>();
            return View(lst.ToPagedList(pageNumberone, 10));
        }
        #endregion
    }
}