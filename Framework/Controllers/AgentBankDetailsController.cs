using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FramWork.Models;
using Core;
using System.IO;
using ExceptionLogging;
namespace FramWork.Controllers
{
    public class AgentBankDetailsController : Controller
    {
        // GET: AgentBankDetails
        private FrameworkEntities db = new FrameworkEntities();

        #region Index
        [LoginAuthorize]
        public ActionResult Index()
        {
            try
            {
                var commision_agentbankdetails = db.Commision_AgentBankDetails.Include(c => c.Organizations).Include(c => c.Users).Where(c => c.MarkAsDelete == false && c.OrgId == Login.OrganizationId);
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
               this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Enterd into Agent Bank Details List Page");
                return View(commision_agentbankdetails.ToList());
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
            Commision_AgentBankDetails obj = new Commision_AgentBankDetails();
            return View(obj);

        }
        #endregion

        #region Details Get
        [LoginAuthorize]
        public ActionResult Details(int id = 0)
        {
            try
            {

                Commision_AgentBankDetails commision_agentbankdetails = db.Commision_AgentBankDetails.Find(id);
                if (commision_agentbankdetails == null)
                {
                    return HttpNotFound();
                }
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                        this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Enterd into Agent Bank Details Page");
                return View(commision_agentbankdetails);
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
            Commision_AgentBankDetails obj = new Commision_AgentBankDetails();
            return View(obj);

        }

        #endregion

        #region Create  get
        [LoginAuthorize]
        public ActionResult Create()
        {
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias");
            ViewBag.TeamId = new SelectList(db.Teams, "Id", "Name");
            ViewBag.SalesAgentId = new SelectList(db.Users, "Id", "Salutation");
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation");
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
             this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Enterd into Agent Bank Create Page");
            return View();
        }
        #endregion

        #region Create post

        [HttpPost]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public ActionResult Create(Commision_AgentBankDetails tbl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tbl.OrgId = Login.OrganizationId;
                    tbl.ModifiedBy = Login.Id;
                    tbl.CreateDate = tbl.ModifiedDate = DateTime.Now;
                    tbl.MarkAsDelete = false;
                    db.Commision_AgentBankDetails.Add(tbl);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", tbl.OrgId);
                ViewBag.TeamId = new SelectList(db.Teams, "Id", "Name", tbl.TeamId);
                ViewBag.SalesAgentId = new SelectList(db.Users, "Id", "Salutation", tbl.SalesAgentId);
                ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", tbl.ModifiedBy);
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
       this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Added New Agent Bank Details");
                return View(tbl);
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

            return View(tbl);
        }

        #endregion

        #region Edit Get
        [LoginAuthorize]
        public ActionResult Edit(int id = 0)
        {
            try
            {
                Commision_AgentBankDetails commision_agentbankdetails = db.Commision_AgentBankDetails.Find(id);
                if (commision_agentbankdetails == null)
                {
                    return HttpNotFound();
                }
                ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", commision_agentbankdetails.OrgId);
                ViewBag.TeamId = new SelectList(db.Teams, "Id", "Name", commision_agentbankdetails.TeamId);
                ViewBag.SalesAgentId = new SelectList(db.Users, "Id", "Salutation", commision_agentbankdetails.SalesAgentId);
                ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", commision_agentbankdetails.ModifiedBy);
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
           this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Enterd into Agent Bank Edit Page");

                return View(commision_agentbankdetails);
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
            Commision_AgentBankDetails obj = new Commision_AgentBankDetails();
            return View(obj);
        }
        #endregion

        #region Edit post
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public ActionResult Edit(Commision_AgentBankDetails tbl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tbl.ModifiedDate = DateTime.Now;
                    tbl.OrgId = Login.OrganizationId;
                    tbl.ModifiedBy = Login.Id;
                    db.Entry(tbl).State = EntityState.Modified;
                    db.SaveChanges();
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
       this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Has Editted Agent Bank Details.");
                    return RedirectToAction("Index");
                }
                ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", tbl.OrgId);
                ViewBag.TeamId = new SelectList(db.Teams, "Id", "Name", tbl.TeamId);
                ViewBag.SalesAgentId = new SelectList(db.Users, "Id", "Salutation", tbl.SalesAgentId);
                ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", tbl.ModifiedBy);
                return View(tbl);
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
            return View(tbl);
        }

        #endregion

        #region Delete get
        [LoginAuthorize]
        public ActionResult Delete(int id = 0)
        {
            try
            {
                Commision_AgentBankDetails commision_agentbankdetails = db.Commision_AgentBankDetails.Find(id);
                if (commision_agentbankdetails == null)
                {
                    return HttpNotFound();
                }
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
           this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Delete Agent Bank Page");
                return View(commision_agentbankdetails);
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
            Commision_AgentBankDetails obj = new Commision_AgentBankDetails();
            return View(obj);

        }
        #endregion

        #region Delete post

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Commision_AgentBankDetails tbl = db.Commision_AgentBankDetails.Find(id);
                tbl.MarkAsDelete = true;
                tbl.OrgId = Login.OrganizationId;
                tbl.ModifiedDate = DateTime.Now;
                tbl.ModifiedBy = Login.Id;
                db.SaveChanges();
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
           this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Has Deleted Agent Bank Details.");
                return RedirectToAction("Index");
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
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


    }
}