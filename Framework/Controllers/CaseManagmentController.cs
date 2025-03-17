using Core;
using FramWork.HelloService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.IO;
using ExceptionLogging;

namespace FramWork.Controllers
{
    public class CaseManagmentController : Controller
    {
        // GET: CaseManagment

        #region  Index
        /// <summary>
        /// Code added by Sherin Maliakkal on july 2015
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult Index()
        {
            try
            {
                HelloServiceClient client = new FramWork.HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                List<CaseManagment> CaseManagementLst = client.GetCaseMnagementLst(Login.OrganizationId).ToList();
                ViewBag.CaseManagementLst = CaseManagementLst;
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
             List<CaseManagment> CaseManagementLst1 = new  List<CaseManagment>();
             ViewBag.CaseManagementLst = CaseManagementLst1;
             return View();
        }
        #endregion

        #region  Case Managment
        [LoginAuthorize]
        public ActionResult CaseList(string CaseId, string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                HelloServiceClient client = new FramWork.HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                List<CaseManagment> CaseManagementLst = client.GetCaseMnagementLst(Login.OrganizationId).ToList();
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;
                int pageSize = 15;
                int pageNumber = (page ?? 1);
                return View(CaseManagementLst.ToPagedList(pageNumber, pageSize));
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
            List<CaseManagment> lst = new List<CaseManagment>();
            return View(lst.ToPagedList(pageNumberone, 10));

        }
        #endregion


        #region  get case Item
        /// <summary>
        ///  code added by sherin Maliakkal on july 2015
        /// </summary>
        /// <param name="CaseId"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult CaseItem(string CaseId)
        {
            try
            {
                if (!String.IsNullOrEmpty(CaseId))
                {
                    ViewBag.PatientId = CaseId;
                    Session["CaseId"] = CaseId;
                    return View();
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


        #region Change Stage Of a case item
        /// <summary>
        /// Code Added by sherin maliakkal on july 2015
        /// </summary>
        /// <param name="caseId"></param>
        /// <param name="stageId"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public bool Changestage(String caseId, string stageId)
        {
            try
            {
                if (!String.IsNullOrEmpty(caseId) && !String.IsNullOrEmpty(stageId))
                {
                    HelloServiceClient client = new FramWork.HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                    client.changeCaseManagmentStage(Login.OrganizationId, new Guid(caseId), new Guid(stageId));
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


        #region Add Case Note, Followers,Attachment,etc

        #region Add Note
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CaseId"></param>
        /// <param name="Note"></param>
        /// <param name="notedate"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public bool AddCMNote(String CaseId, String Note, String notedate)
        {
            try
            {
                if (!String.IsNullOrEmpty(CaseId) && !String.IsNullOrEmpty(Note))
                {
                    HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                    CaseManagment_Note tbl = new CaseManagment_Note();
                    tbl.Id = Guid.NewGuid();
                    tbl.OrgId = Login.OrganizationId;
                    tbl.CaseId = new Guid(CaseId);
                    tbl.CaseNote = Note;
                    if (!String.IsNullOrEmpty(notedate))
                        tbl.when_it_happened = Convert.ToDateTime(notedate);
                    tbl.ModifiedBy = Login.Id;
                    bool Success = client.AddCMNote(tbl);
                    if (Success)
                        return true;
                    else
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
        #endregion


        #region Add CM Follower
        [LoginAuthorize]
        public bool AddCMFollowers(String CaseId, String casefollowers)
        {
            try
            {
                if (!String.IsNullOrEmpty(CaseId) && !String.IsNullOrEmpty(casefollowers))
                {
                    HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                    CaseManagment_Followers tbl = new CaseManagment_Followers();
                    tbl.Id = Guid.NewGuid();
                    tbl.CaseID = new Guid(CaseId);
                    tbl.CaseFollowers = new Guid(casefollowers);
                    tbl.ModifiedBy = Login.Id;
                    tbl.OrgId = Login.OrganizationId;
                    bool Success = client.AddCMFollowers(tbl);
                    if (Success)
                        return true;
                    else
                        return false;
                }



                return true;

            }
            catch (Exception)
            {
                return false;
                throw;

            }
        }
        #endregion

        #region Case Managment Task
        #region   Add Case Managment Task
        /// <summary>
        /// Code added by sherin on August 11 2015
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Owner"></param>
        /// <param name="Date"></param>
        /// <param name="Category"></param>
        /// <param name="CaseId"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public bool AddCMTask(String Title, String Owner, String Date, String Category, String CaseId)
        {
            try
            {
                if (!String.IsNullOrEmpty(Title) && !String.IsNullOrEmpty(Owner) && !String.IsNullOrEmpty(Date) && !String.IsNullOrEmpty(Category) && !String.IsNullOrEmpty(CaseId))
                {
                    HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                    CaseManagment_Task tbl = new CaseManagment_Task();
                    tbl.OrgId = Login.OrganizationId;
                    tbl.CaseId = new Guid(CaseId);
                    tbl.TaskTitle = Title;
                    tbl.WhoisResponsible = new Guid(Owner);
                    tbl.TaskDueDate = Convert.ToDateTime(Date);
                    tbl.TaskCategory = new Guid(Category);
                    tbl.ModifiedBy = Login.Id;
                    if (client.AddCMTask(tbl))
                    {
                        return true;
                    }
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

        #region List Of Tasks
        /// <summary>
        /// Code added by sherin on july 2015
        /// </summary>
        /// <param name="CaseId"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetListOfTasks(string CaseId)
        {
            try
            {
                if (!String.IsNullOrEmpty(CaseId))
                {
                    HelloServiceClient client = new FramWork.HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                    List<CaseManagment_Task> CMTaskLst = client.GetCMTaskLst(Login.OrganizationId, new Guid(CaseId)).ToList();
                    return View(CMTaskLst);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }
        #endregion
        #endregion

        #region List Of Notes
        [LoginAuthorize]
        public ActionResult GetListOfNotes(string CaseId, string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                HelloServiceClient client = new FramWork.HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                List<CaseManagment_Note> CMNoteLst = client.GetCMNoteLst(Login.OrganizationId, new Guid(CaseId)).ToList();
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;
                int pageSize = 4;
                int pageNumber = (page ?? 1);
                return View(CMNoteLst.ToPagedList(pageNumber, pageSize));
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
            List<CaseManagment_Note> lst = new List<CaseManagment_Note>();
            return View(lst.ToPagedList(pageNumberone, 10));

        }
        //public ActionResult GetListOfNotes(string CaseId)
        //{
        //    try
        //    {
        //        HelloServiceClient client = new FramWork.HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
        //        List<CaseManagment_Note> CMNoteLst = client.GetCMNoteLst(Login.OrganizationId, new Guid(CaseId)).ToList();
        //        return View(CMNoteLst);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        #endregion


        #region List Of Followers
        [LoginAuthorize]
        public ActionResult GetListOfFollowers(string CaseId)
        {
            try
            {
                HelloServiceClient client = new FramWork.HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                List<CaseManagment_Followers> CMFollowersLst = client.GetCMFollowers(Login.OrganizationId, new Guid(CaseId)).ToList();
                return View(CMFollowersLst);

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
            List<CaseManagment_Followers> CMFollowersLsts = new List<CaseManagment_Followers>();
            return View(CMFollowersLsts);

        }
        #endregion


        #region Case Mangement 28/07/2015

        [LoginAuthorize]
        public ActionResult CreateCase()
        {

            return View();
        }

        [LoginAuthorize]
        public bool AddCase(CaseManagment CaseTbl)
        {
            try
            {
                CaseTbl.Id = Guid.NewGuid();
                CaseTbl.ModifiedBy = Login.Id;
                CaseTbl.OrgId = Login.OrganizationId;
                HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                client.AddCase(CaseTbl);
                return true;
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
        [LoginAuthorize]
        public JsonResult GetPersonPhoneEmail(string PId)
        {

         try
            {
                HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                Guid PersonLeadId = new Guid(PId);
                PersonLead ObjPersonLead = client.GetPersonLeadLst(Login.OrganizationId).Where(x => x.Id == PersonLeadId).FirstOrDefault();
                Person ObjPerson = client.GetLstPerson(Login.OrganizationId).Where(x => x.Id == ObjPersonLead.PersonId).FirstOrDefault();
                return Json(new { SUCCESS = true, Phone = ObjPerson.Phone, Email = ObjPerson.Email });
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
            return Json(null);
        }

        #endregion

    }
}