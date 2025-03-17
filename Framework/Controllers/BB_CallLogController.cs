using Core;
using FramWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;
using System.IO;
using ExceptionLogging;

namespace FramWork.Controllers
{
public class BB_CallLogController : Controller
{


                // GET: BB_CallLog
                 #region Index
                [LoginAuthorize]
                public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
                {
                try
                {
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
         this.ControllerContext.RouteData.Values["action"].ToString(),
         "The user " + Login.FirstName + " " + Login.LastName + "Entered into Company Page");

                FrameworkEntities dc = new FrameworkEntities();
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(dc.ABB_Company.AsEnumerable().Where(a => a.OrgId == Login.OrganizationId && a.MarkAsDelete == false).ToPagedList(pageNumber, pageSize));
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
                List<ABB_Company> lst = new List<ABB_Company>();
                return View(lst.ToPagedList(pageNumberone, 10));
                }

                #endregion
            
                 #region Company Details
                [LoginAuthorize]
                public ActionResult Details(long? id)
                {
                    try
                    {
                        FrameworkEntities dc = new FrameworkEntities();
                        if (id == null)
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }
                        ABB_Company aBB_Company = dc.ABB_Company.Find(id);
                        if (aBB_Company == null)
                        {
                            return HttpNotFound();
                        }
                        Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
         this.ControllerContext.RouteData.Values["action"].ToString(),
         "The user " + Login.FirstName + " " + Login.LastName + "Entered into Company Details Page");

                        return View(aBB_Company);
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
                    ABB_Company obj = new ABB_Company();
                    return View(obj);
                }

                #endregion

                 #region CallLoglist
                [LoginAuthorize]
                public ActionResult CallLoglist(string sortOrder, string currentFilter, string searchString, int? page, long? companyId)
                {
                try
                {
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
         this.ControllerContext.RouteData.Values["action"].ToString(),
         "The user " + Login.FirstName + " " + Login.LastName + "Entered into Call Log Page");

                    FrameworkEntities dc = new FrameworkEntities();
                Session["companyid"] = companyId;           
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(dc.ABB_CallLog.AsEnumerable().Where(a => a.CompanyId == Convert.ToInt64(companyId) && a.OrgId == Login.OrganizationId && a.MarkAsDelete == false).ToPagedList(pageNumber, pageSize));
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
                List<ABB_CallLog> lst = new List<ABB_CallLog>();
                return View(lst.ToPagedList(pageNumberone, 10));
                }

                #endregion
                    
                 #region Add and Edit CallLog
                //Add and Edit CallLog based on the Id Field
                [LoginAuthorize]
                [HttpPost]
                public String savecallcog(string CompanyId, string Subject, string InteractionType, string MeetingDuration, string PlaceOfMeeting,
                string PlannedDate, string PlannedTime, string MeetingDate, string MeetingTime, string Description, string MarketWatch,
                string NewBusinessOpportunities, string ManagementNews, int? Id)
                {
                try
                {
                    FrameworkEntities dc = new FrameworkEntities();
                if (!String.IsNullOrEmpty(CompanyId) && !String.IsNullOrEmpty(Subject) && !String.IsNullOrEmpty(InteractionType) &&
                !String.IsNullOrEmpty(MeetingDuration) && !String.IsNullOrEmpty(PlaceOfMeeting) &&
                !String.IsNullOrEmpty(PlannedDate) && !String.IsNullOrEmpty(PlannedTime)
                && !String.IsNullOrEmpty(MeetingDate) && String.IsNullOrEmpty(Convert.ToString(Id)))
                {
                 ABB_CallLog tbl = new ABB_CallLog();
                tbl.OrgId = Login.OrganizationId;
                tbl.CompanyId = Convert.ToInt64(CompanyId);
                tbl.Subject = Subject;
                tbl.InteractionTypeId = Convert.ToInt32(InteractionType);
                tbl.MeetingDuration = MeetingDuration;
                tbl.PlaceOfMeeting = PlaceOfMeeting;
                tbl.PlannedDate = Convert.ToDateTime(PlannedDate).Date;
                tbl.PlannedTime = PlannedTime;
                tbl.MeetingDate = Convert.ToDateTime(MeetingDate).Date;
                tbl.MeetingTime = MeetingTime;
                tbl.Description = Description;
                tbl.MarketWatch = MarketWatch;
                tbl.NewBusinessOpportunities = NewBusinessOpportunities;
                tbl.ManagementNews = ManagementNews;
                tbl.CreateDate = tbl.ModifiedDate = DateTime.Now;
                tbl.ModifiedBy = Login.Id;
                tbl.MarkAsDelete = false;
                dc.ABB_CallLog.Add(tbl);
                dc.SaveChanges();
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
     this.ControllerContext.RouteData.Values["action"].ToString(),
     "The user " + Login.FirstName + " " + Login.LastName + "Added new Call Log Page");

                return tbl.Id.ToString();
                }
                else
                {
                    var tbl = dc.ABB_CallLog.Where(a => a.Id ==Id).FirstOrDefault();
                    tbl.OrgId = Login.OrganizationId;
                    tbl.CompanyId = Convert.ToInt64(CompanyId);
                    tbl.Subject = Subject;
                    tbl.InteractionTypeId = Convert.ToInt32(InteractionType);
                    tbl.MeetingDuration = MeetingDuration;
                    tbl.PlaceOfMeeting = PlaceOfMeeting;
                    tbl.PlannedDate = Convert.ToDateTime(PlannedDate).Date;
                    tbl.PlannedTime = PlannedTime;
                    tbl.MeetingDate = Convert.ToDateTime(MeetingDate).Date;
                    tbl.MeetingTime = MeetingTime;
                    tbl.Description = Description;
                    tbl.MarketWatch = MarketWatch;
                    tbl.NewBusinessOpportunities = NewBusinessOpportunities;
                    tbl.ManagementNews = ManagementNews;
                   tbl.ModifiedDate = DateTime.Now;
                    tbl.ModifiedBy = Login.Id;                  
                    dc.SaveChanges();
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
this.ControllerContext.RouteData.Values["action"].ToString(),
"The user " + Login.FirstName + " " + Login.LastName + "Edited Call Log Page");
                    return tbl.Id.ToString();
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
                return "";            
                }
                #endregion

                 #region EditCallLog Get
                [LoginAuthorize]
                public ActionResult EditCallLog(int? id)
                {
                    try
                    {
                        FrameworkEntities dc = new FrameworkEntities();
                        if (id == null)
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }
                        Session["EditCallLogId"] = id;
                        //ABB_CompanyShareHolders bshareholder =db.ABB_CompanyShareHolders.Find(id);
                        var result = (from c in dc.ABB_CallLog.AsEnumerable()
                                      where c.Id == id
                                      select new
                                      {
                                          CompanyId = c.CompanyId,
                                          Subject = c.Subject,
                                          InteractionTypeId = c.InteractionTypeId,
                                          MeetingDuration = c.MeetingDuration,
                                          PlaceOfMeeting = c.PlaceOfMeeting,
                                          PlannedDate = Convert.ToDateTime(c.PlannedDate).ToShortDateString(),
                                          PlannedTime = c.PlannedTime,
                                          MeetingDate = Convert.ToDateTime(c.MeetingDate).ToShortDateString(),
                                          MeetingTime = c.MeetingTime,
                                          Description = c.Description,
                                          MarketWatch = c.MarketWatch,
                                          NewBusinessOpportunities = c.NewBusinessOpportunities,
                                          ManagementNews = c.ManagementNews
                                      }).FirstOrDefault();

                        if (result == null)
                        {
                            return HttpNotFound();
                        }
                        return Json(result, JsonRequestBehavior.AllowGet);
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

                 #region Call Log Details
                 [LoginAuthorize]
                public ActionResult CLDetails(long? id)
                {
                    try
                    {
                        FrameworkEntities dc = new FrameworkEntities();
                        if (id == null)
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }
                        ABB_CallLog dtl = dc.ABB_CallLog.Find(id);
                        Session["CallLogSubject"]=dtl.Subject;
                        Session["companyid"] = dtl.CompanyId;
                        if (dtl == null)
                        {
                            return HttpNotFound();
                        }
                        Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
this.ControllerContext.RouteData.Values["action"].ToString(),
"The user " + Login.FirstName + " " + Login.LastName + "Entered into Call Log Details Page");
                        return View(dtl);
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
                    ABB_CallLog obj = new ABB_CallLog();
                    return View(obj);
                }
                #endregion

                 #region Call Log Table Details
                [LoginAuthorize]
                public ActionResult CallLogsDetails(long? id)
                {
                    try {
                        FrameworkEntities dc = new FrameworkEntities();
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    ABB_CallLog dtl = dc.ABB_CallLog.Find(id);
                    if (dtl == null)
                    {
                        return HttpNotFound();
                    }
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
this.ControllerContext.RouteData.Values["action"].ToString(),
"The user " + Login.FirstName + " " + Login.LastName + "Entered into Call Log Details Page");
                    return View(dtl);
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
                    ABB_CallLog obj = new ABB_CallLog();
                    return View(obj);
                }
                #endregion

                 #region Request Finanace Details
                 [LoginAuthorize]
                public ActionResult RequestFinanceDetails(string sortOrder, string currentFilter, string searchString, int? page, long? id)
                {
                    try
                    {
                        FrameworkEntities dc = new FrameworkEntities();
                        if (id == null)
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }
                        int pageSize = 10;
                        int pageNumber = (page ?? 1);
                        var dtl = dc.ABB_RequestFinance.AsEnumerable().Where(a => a.CallLogId == Convert.ToInt64(id) && a.OrgId == Login.OrganizationId && a.MarkAsDelete == false).OrderByDescending(a => a.ModifiedDate).ToPagedList(pageNumber, pageSize);
                        if (dtl == null)
                        {
                            return HttpNotFound();
                        }
                        Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
this.ControllerContext.RouteData.Values["action"].ToString(),
"The user " + Login.FirstName + " " + Login.LastName + "Entered into  Request Finanace Details Page");
                        return View(dtl);
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

                 #region General Discussion Details
                 [LoginAuthorize]
                 public ActionResult GeneralDiscussionDetails(string sortOrder, string currentFilter, string searchString, int? page, long? id)
                 {
                     try
                     {
                         FrameworkEntities dc = new FrameworkEntities();
                         if (id == null)
                         {
                             return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                         }
                         int pageSize = 10;
                         int pageNumber = (page ?? 1);
                         var dtl = dc.ABB_CallLogGeneralDiscussion.AsEnumerable().Where(a => a.CallLogId == Convert.ToInt64(id) && a.OrgId == Login.OrganizationId && a.MarkAsDelete == false).OrderByDescending(a => a.ModifiedDate).ToPagedList(pageNumber, pageSize);
                         if (dtl == null)
                         {
                             return HttpNotFound();
                         }
                         Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                            this.ControllerContext.RouteData.Values["action"].ToString(),
                            "The user " + Login.FirstName + " " + Login.LastName + "Entered into  General Discussion Details Page");
                         return View(dtl);
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

                 #region Action Points
                 [LoginAuthorize]
                 public ActionResult ActionPointsDetails(string sortOrder, string currentFilter, string searchString, int? page, long? id)
                 {
                     try
                     {
                         FrameworkEntities dc = new FrameworkEntities();
                         if (id == null)
                         {
                             return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                         }
                         int pageSize = 10;
                         int pageNumber = (page ?? 1);
                         var dtl = dc.ABB_CallLogActionPoint.AsEnumerable().Where(a => a.CallLogId == Convert.ToInt64(id) && a.OrgId == Login.OrganizationId && a.MarkAsDelete == false).OrderByDescending(a => a.ModifiedDate).ToPagedList(pageNumber, pageSize);
                         if (dtl == null)
                         {
                             return HttpNotFound();
                         }
                         Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                           this.ControllerContext.RouteData.Values["action"].ToString(),
                           "The user " + Login.FirstName + " " + Login.LastName + "Entered into  Action Points Details Page");
                         return View(dtl);
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

                 #region Add  and Edit Request Finance
                 [LoginAuthorize]      
                [HttpPost]
                public bool addrequestfinance(string CompanyId, string CallLogId,string Purpose, string ProductId, string RequiredOn, string Tenure, string Amount, string PaymentDetails, long? Id)
                {
                try
                {
                    FrameworkEntities dc = new FrameworkEntities();
                if (!string.IsNullOrEmpty(CompanyId) && !string.IsNullOrEmpty(Purpose) &&!string.IsNullOrEmpty(CallLogId)&&
                !string.IsNullOrEmpty(ProductId) && !string.IsNullOrEmpty(RequiredOn) && !string.IsNullOrEmpty(Tenure)
                && !string.IsNullOrEmpty(Amount) && !string.IsNullOrEmpty(PaymentDetails) && string.IsNullOrEmpty(Convert.ToString(Id)))
                {
                ABB_RequestFinance tbl = new ABB_RequestFinance();
                tbl.OrgId = Login.OrganizationId;
                tbl.CompanyId = Convert.ToInt64(CompanyId);
                tbl.CallLogId = Convert.ToInt32(CallLogId);
                tbl.ProductId = Convert.ToInt64(ProductId);
                tbl.Purpose = Purpose;
                tbl.RequiredOn = Convert.ToDateTime(RequiredOn);
                tbl.Tenure = Tenure;
                tbl.Amount = decimal.Parse(Regex.Replace(Amount, @"[^\d.]", ""));
                tbl.PaymentDetails = PaymentDetails;
                tbl.DealSatageId = Convert.ToInt32(Core.DealStagedef.Lead);
                tbl.CreateDate = tbl.ModifiedDate = DateTime.Now;
                tbl.ModifiedBy = Login.Id;
                tbl.MarkAsDelete = false;               
                dc.ABB_RequestFinance.Add(tbl);
                dc.SaveChanges();
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                this.ControllerContext.RouteData.Values["action"].ToString(),
                "The user " + Login.FirstName + " " + Login.LastName + "Added new  Request Finance");
                return true;

                }

                else
                {
                    ABB_RequestFinance tbl = dc.ABB_RequestFinance.Find(Id);
                    tbl.OrgId = Login.OrganizationId;
                    tbl.CompanyId = Convert.ToInt64(CompanyId);
                    tbl.CallLogId = Convert.ToInt32(CallLogId);
                    tbl.ProductId = Convert.ToInt64(ProductId);
                    tbl.Purpose = Purpose;
                    tbl.RequiredOn = Convert.ToDateTime(RequiredOn);
                    tbl.Tenure = Tenure;
                    tbl.Amount = decimal.Parse(Regex.Replace(Amount, @"[^\d.]", ""));
                    tbl.PaymentDetails = PaymentDetails;
                    tbl.DealSatageId = Convert.ToInt32(Core.DealStagedef.Lead);
                    tbl.ModifiedDate = DateTime.Now;
                    tbl.ModifiedBy = Login.Id;                              
                    dc.SaveChanges();
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                    this.ControllerContext.RouteData.Values["action"].ToString(),
                    "The user " + Login.FirstName + " " + Login.LastName + "Edited Request Finance");
                    return true;
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
                return false;
                }

                #endregion

                 #region Edit Request Finanace
                      [LoginAuthorize]
                public ActionResult EditRequestFinance(long? id)
                {
                    try
                    {
                        FrameworkEntities dc = new FrameworkEntities();
                        if (id == null)
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }                    
                        //ABB_CompanyShareHolders bshareholder =db.ABB_CompanyShareHolders.Find(id);
                        var result = (from c in dc.ABB_RequestFinance.AsEnumerable()
                                      where c.Id == id
                                      select new
                                      {
                                          Id=c.Id,
                                          CompanyId = c.CompanyId,
                                          ProductId = c.ProductId,
                                          Purpose = c.Purpose,
                                          RequiredOn = c.RequiredOn.ToShortDateString(),
                                          Tenure = c.Tenure,
                                          Amount = c.Amount,
                                          PaymentDetails = c.PaymentDetails,
                                          DealSatageId = c.DealSatageId                                      
                                      }).FirstOrDefault();

                        if (result == null)
                        {
                            return HttpNotFound();
                        }
                        return Json(result, JsonRequestBehavior.AllowGet);
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

                 #region Add and Edit Action Points
                 //Add and Edit Genral Discussion based on the Id Field in the parameter.
                 [LoginAuthorize]
                 [HttpPost]
                public bool addactionpoints(string CallLogId, string ActionTitle, string ActionDescription, string DueDate, string Tags, int? Id)
                {
                try
                {
                      FrameworkEntities dc = new FrameworkEntities();
                if (!string.IsNullOrEmpty(CallLogId) && !string.IsNullOrEmpty(ActionTitle) && 
                !string.IsNullOrEmpty(ActionDescription) && !string.IsNullOrEmpty(DueDate)
                && !string.IsNullOrEmpty(Tags) && (string.IsNullOrEmpty(Convert.ToString(Id))))
                {
                ABB_CallLogActionPoint tbl = new ABB_CallLogActionPoint();
                tbl.OrgId = Login.OrganizationId;                    
                tbl.CallLogId = Convert.ToInt32(CallLogId);
                tbl.ActionTitle = ActionTitle;
                tbl.ActionDescription = ActionDescription;
                tbl.DueDate = Convert.ToDateTime(DueDate);
                tbl.Tags = Tags;                   
                tbl.CreateDate = tbl.ModifiedDate = DateTime.Now;
                tbl.ModifiedBy = Login.Id;
                tbl.MarkAsDelete = false;              
                dc.ABB_CallLogActionPoint.Add(tbl);
                dc.SaveChanges();
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
           this.ControllerContext.RouteData.Values["action"].ToString(),
           "The user " + Login.FirstName + " " + Login.LastName + "Added new  action points");
                return true;

                }
                else
                {
                    ABB_CallLogActionPoint tbl = dc.ABB_CallLogActionPoint.Find(Id);
                    tbl.OrgId = Login.OrganizationId;
                    tbl.CallLogId = Convert.ToInt32(CallLogId);
                    tbl.ActionTitle = ActionTitle;
                    tbl.ActionDescription = ActionDescription;
                    tbl.DueDate = Convert.ToDateTime(DueDate);
                    tbl.Tags = Tags;
                    tbl.ModifiedDate = DateTime.Now;
                    tbl.ModifiedBy = Login.Id;                             
                    dc.SaveChanges();
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
               this.ControllerContext.RouteData.Values["action"].ToString(),
               "The user " + Login.FirstName + " " + Login.LastName + "Edited new  action points");
                    return true;
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
                return false;
                }
                #endregion

                 #region Edit Action Points
                 [LoginAuthorize]
                 public ActionResult EditActionPoints(int? id)
                 {
                     try
                     {
                         FrameworkEntities dc = new FrameworkEntities();
                         if (id == null)
                         {
                             return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                         }
                         //ABB_CompanyShareHolders bshareholder =db.ABB_CompanyShareHolders.Find(id);
                         var result = (from c in dc.ABB_CallLogActionPoint.AsEnumerable()
                                       where c.Id == id
                                       select new
                                       {
                                           Id = c.Id,
                                           ActionTitle = c.ActionTitle,
                                           ActionDescription = c.ActionDescription,
                                           DueDate = c.DueDate.ToShortDateString(),
                                           Tags = c.Tags,
                                       }).FirstOrDefault();

                         if (result == null)
                         {
                             return HttpNotFound();
                         }
                         return Json(result, JsonRequestBehavior.AllowGet);
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

                 #region Add and Edit General Discussion    
                //Add and Edit Genral Discussion based on the Id Field in the parameter.
                 [LoginAuthorize]
                 [HttpPost]
                public bool AddGeneralDiscussion(string CallLogId, string Title, string GeneralDiscussion, int? Id)
                {
                try
                {
                    FrameworkEntities dc = new FrameworkEntities();
                if (!string.IsNullOrEmpty(CallLogId) && !string.IsNullOrEmpty(Title) &&
                !string.IsNullOrEmpty(GeneralDiscussion) && (string.IsNullOrEmpty(Convert.ToString(Id))))
                {
                ABB_CallLogGeneralDiscussion tbl = new ABB_CallLogGeneralDiscussion();
                tbl.OrgId = Login.OrganizationId;
                tbl.CallLogId = Convert.ToInt32(CallLogId);
                tbl.Title = Title;
                tbl.GeneralDiscussion = GeneralDiscussion;                   
                tbl.CreateDate = tbl.ModifiedDate = DateTime.Now;
                tbl.ModifiedBy = Login.Id;
                tbl.MarkAsDelete = false;               
                dc.ABB_CallLogGeneralDiscussion.Add(tbl);
                dc.SaveChanges();
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
           this.ControllerContext.RouteData.Values["action"].ToString(),
           "The user " + Login.FirstName + " " + Login.LastName + "Added new General Discussion");
                return true;
                }
                else
                {
                    ABB_CallLogGeneralDiscussion tbl = dc.ABB_CallLogGeneralDiscussion.Find(Id);
                    tbl.OrgId = Login.OrganizationId;
                    tbl.CallLogId = Convert.ToInt32(CallLogId);
                    tbl.Title = Title;
                    tbl.GeneralDiscussion = GeneralDiscussion;
                    tbl.ModifiedDate = DateTime.Now;
                    tbl.ModifiedBy = Login.Id; 
                    dc.SaveChanges();
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
               this.ControllerContext.RouteData.Values["action"].ToString(),
               "The user " + Login.FirstName + " " + Login.LastName + "Edited  General Discussion");
                    return true;
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
                return false;
                }

                #endregion

                 #region Edit General Discussion
                 [LoginAuthorize]
                 public ActionResult EditGeneralDiscussion(int? id)
                 {
                     try
                     {
                         FrameworkEntities dc = new FrameworkEntities();
                         if (id == null)
                         {
                             return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                         }
                         //ABB_CompanyShareHolders bshareholder =db.ABB_CompanyShareHolders.Find(id);
                         var result = (from c in dc.ABB_CallLogGeneralDiscussion.AsEnumerable()
                                       where c.Id == id
                                       select new
                                       {
                                           Id = c.Id,
                                           Title = c.Title,
                                           GeneralDiscussion = c.GeneralDiscussion                                          
                                       }).FirstOrDefault();

                         if (result == null)
                         {
                             return HttpNotFound();
                         }
                         return Json(result, JsonRequestBehavior.AllowGet);
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

                 #region  Document Index
                  [LoginAuthorize]
                public ActionResult DocumentIndex(long? id)
                {
                    try
                    {
                        Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                          this.ControllerContext.RouteData.Values["action"].ToString(),
                          "The user " + Login.FirstName + " " + Login.LastName + "Entered into  Document Page");
                        FrameworkEntities dc = new FrameworkEntities();
                        var aBB_Company_Doc_folder = dc.ABB_Company_Doc_folder.Include(a => a.ABB_Company).Include(a => a.Organizations).Include(a => a.Users);
                        //.Where(a=>a.CompanyId==id);
                        return View(aBB_Company_Doc_folder.ToList());
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
                    return null;
                }
                #endregion

                 #region DocFolderView
                    [LoginAuthorize]      
                public ActionResult DocFolderView()
                {
                try
                {
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                      this.ControllerContext.RouteData.Values["action"].ToString(),
                      "The user " + Login.FirstName + " " + Login.LastName + "Entered into  Document Folder Page");
                    FrameworkEntities dc = new FrameworkEntities();
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

                return View();
                }
                #endregion

                 #region UploadfilestofolderView for Ajax
                        public ActionResult UploadfilestofolderView(string company, string fid)
                    {
                        try
                        {
                            ViewBag.company = company;
                            ViewBag.fid = fid;
                            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                      this.ControllerContext.RouteData.Values["action"].ToString(),
                      "The user " + Login.FirstName + " " + Login.LastName + "Entered into  Upload File List Page");
                            FrameworkEntities dc = new FrameworkEntities();
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

                        return View();
                    }
                    #endregion

                 #region Upload async FIles to folder
                [LoginAuthorize]  
                public ActionResult Uploadfilestofolder(string company, string fid)
                {
                try
                {
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                     this.ControllerContext.RouteData.Values["action"].ToString(),
                     "The user " + Login.FirstName + " " + Login.LastName + "Entered into  Upload File List Page");
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

                return View();
                }
                [HttpPost]
                [LoginAuthorize]
                public bool Multiple(IEnumerable<HttpPostedFileBase> files, FormCollection c)
                {
                try
                {
                FrameworkEntities dc = new FrameworkEntities();
                if (!string.IsNullOrEmpty(c["txtfilename"]) && !string.IsNullOrEmpty(c["txtcompany"]) && !string.IsNullOrEmpty(c["txtfolder"])
                && !string.IsNullOrEmpty(c["txtdoctype"]) && !string.IsNullOrEmpty(c["txtexpiry"]))
                {
                foreach (var file in files)
                {
                if (file != null && file.ContentLength > 0)
                {

                ABB_Company_Doc_folder_file tbl = new ABB_Company_Doc_folder_file();
                tbl.OrgId = Login.OrganizationId;
                tbl.FolderId = Convert.ToInt64(c["txtfolder"]);
                tbl.Name = c["txtdoctype"];
                tbl.filename = c["txtfilename"];
                tbl.ExpiryDate = Convert.ToDateTime(c["txtexpiry"]);                            
                tbl.FileType = Path.GetExtension(file.FileName);
                tbl.IconClass = getfileiconclass(Path.GetExtension(file.FileName));
                tbl.CreateDate = tbl.ModifiedDate = DateTime.Now;
                tbl.ModifiedBy = Login.Id;
                tbl.MarkASDelete = false;
                dc.ABB_Company_Doc_folder_file.Add(tbl);
                dc.SaveChanges();
                file.SaveAs(Path.Combine(Server.MapPath("~/assets/Programmer/CompanyDocs/" + c["txtcompany"] + "/" + c["txtfolder"] + "/"), tbl.Id + Path.GetExtension(file.FileName)));
                }
                }
                return true;
                }
                else
                {
                return false;
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

                return false;
                }
                #endregion

                 #region   get extension class
                public string getfileiconclass(string extn)
                {
                try
                {
                switch (extn.ToLower())
                {
                case ".doc":
                case ".docx":
                return "fa fa-file-word-o";

                case ".xls":
                case ".xlsx":
                case ".csv":
                return "fa fa-file-excel-o";
                case ".pdf":
                return "fa fa-file-pdf-o";
                case ".png":
                case ".bmp":
                case ".gif":
                case ".jpeg":
                case ".jpg":
                return "fa fa-file-image-o";
                case ".ppt":
                case ".pptx":
                return "fa fa-file-powerpoint-o";
                case ".psd":
                return "fa  fa-life-bouy";
                case ".zip":
                case ".rar":
                return "fa fa-file-zip-o";
                default:
                return "fa  fa-paw";

                }
                }
                catch (Exception)
                {

                throw;
                }
                }

                #endregion


        }
             }