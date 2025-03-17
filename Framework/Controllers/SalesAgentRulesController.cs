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
    public class SalesAgentRulesController : Controller
    {
        // GET: SalesAgentRules
        private FrameworkEntities db = new FrameworkEntities();

            #region Index
            //GET: /SalesAgentRules/
     
            [LoginAuthorize]
            public ActionResult Index()
            {
            try
            {
            var commision_salesagentrules = db.Commision_SalesAgentRules.Include(c => c.CampaignProducts).Include(c => c.Organizations).Include(c => c.Teams).Include(c => c.Users).Include(c => c.Users1).Where(c => c.MarkAsDelete == false && c.OrgId == Login.OrganizationId).ToList(); ;
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
         this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Enterd into Sales Agent Rules List Page");
            return View(commision_salesagentrules.ToList());
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
            Commision_SalesAgentRules obj = new Commision_SalesAgentRules();
            return View(obj);
            }

            #endregion

            #region Details Get
            // GET: /SalesAgentRules/Details/5
            [LoginAuthorize]
            public ActionResult Details(int id = 0)
            {
            try
            {
            Commision_SalesAgentRules commision_salesagentrules = db.Commision_SalesAgentRules.Find(id);
            if (commision_salesagentrules == null)
            {
            return HttpNotFound();
            }
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                   this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Enterd into Sales Agent Rules Page");
            return View(commision_salesagentrules);
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
            Commision_SalesAgentRules obj = new Commision_SalesAgentRules();
            return View(obj);
            }
            #endregion

            #region Create  get
            // GET: /SalesAgentRules/Create
            [LoginAuthorize]
            public ActionResult Create()
            {
            ViewBag.ProductId = new SelectList(db.CampaignProducts, "Id", "ProductName");
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias");
            ViewBag.TeamId = new SelectList(db.Teams, "Id", "Name");
            ViewBag.SalesAgentId = new SelectList(db.Users, "Id", "Salutation");
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation");
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
       this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Enterd into Sales Agent Rules Create Page");
            return View();
            }

            #endregion

            #region Create post
            // POST: /SalesAgentRules/Create

            [HttpPost]
            [ValidateAntiForgeryToken]
            [LoginAuthorize]
            public ActionResult Create(Commision_SalesAgentRules tbl)
            {
            try {
            ViewBag.ProductId = new SelectList(db.CampaignProducts, "Id", "ProductName", tbl.ProductId);
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", tbl.OrgId);
            ViewBag.TeamId = new SelectList(db.Teams, "Id", "Name", tbl.TeamId);
            ViewBag.SalesAgentId = new SelectList(db.Users, "Id", "Salutation", tbl.SalesAgentId);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", tbl.ModifiedBy);
            if (ModelState.IsValid)
            {
            tbl.OrgId = Login.OrganizationId;
            tbl.ModifiedBy = Login.Id;
            tbl.CreateDate = tbl.ModifiedDate = DateTime.Now;
            tbl.MarkAsDelete = false;
            db.Commision_SalesAgentRules.Add(tbl);
            db.SaveChanges();
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Added New Sales Agent Rules");
            return RedirectToAction("Index");
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
            return View(tbl);
            }
            #endregion

            #region Edit Get
            // GET: /SalesAgentRules/Edit/5
            [LoginAuthorize]
            public ActionResult Edit(int id = 0)
            {

            try
            {
            Commision_SalesAgentRules commision_salesagentrules = db.Commision_SalesAgentRules.Find(id);
            if (commision_salesagentrules == null)
            {
            return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.CampaignProducts, "Id", "ProductName", commision_salesagentrules.ProductId);
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", commision_salesagentrules.OrgId);
            ViewBag.TeamId = new SelectList(db.Teams, "Id", "Name", commision_salesagentrules.TeamId);
            ViewBag.SalesAgentId = new SelectList(db.Users, "Id", "Salutation", commision_salesagentrules.SalesAgentId);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", commision_salesagentrules.ModifiedBy);
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
  this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Enterd into Sales Agent Rules Edit Page");
            return View(commision_salesagentrules);
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
            Commision_SalesAgentRules obj = new Commision_SalesAgentRules();
            return View(obj);
            }
            #endregion

            #region Edit post  
            // POST: /SalesAgentRules/Edit/5

            [HttpPost]
            [ValidateAntiForgeryToken]
            [LoginAuthorize]
            public ActionResult Edit(Commision_SalesAgentRules tbl)
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
 this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Has Editted Sales Agent Rules.");
            return RedirectToAction("Index");
            }
            ViewBag.ProductId = new SelectList(db.CampaignProducts, "Id", "ProductName", tbl.ProductId);
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
            Commision_SalesAgentRules obj = new Commision_SalesAgentRules();
            return View(obj);

            }

            #endregion

            #region Delete get
            // GET: /SalesAgentRules/Delete/5
            [LoginAuthorize]
            public ActionResult Delete(int id = 0)
            {
            try
            {
            Commision_SalesAgentRules commision_salesagentrules = db.Commision_SalesAgentRules.Find(id);
            if (commision_salesagentrules == null)
            {
            return HttpNotFound();
            }
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
    this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Delete Sales Agent Rules Page");
            return View(commision_salesagentrules);
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
            Commision_SalesAgentRules obj = new Commision_SalesAgentRules();
            return View(obj);
            }

            #endregion

            #region Delete post
            // POST: /SalesAgentRules/Delete/5
         
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            [LoginAuthorize]
            public ActionResult DeleteConfirmed(int id)
            {
            try
            {
            Commision_SalesAgentRules tbl = db.Commision_SalesAgentRules.Find(id);
            tbl.MarkAsDelete = true;
            tbl.OrgId = Login.OrganizationId;
            tbl.ModifiedDate = DateTime.Now;
            tbl.ModifiedBy = Login.Id;
            db.Commision_SalesAgentRules.Remove(tbl);
            db.SaveChanges();
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
      this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Has Deleted Sales Agent Rules.");
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