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

namespace ForDelete.Controllers
{
    public class CommisionTypeController : Controller
    {
        private FrameworkEntities db = new FrameworkEntities();

        //
        // GET: /CommisionType/

        #region Index
        [LoginAuthorize]
        public ActionResult Index()
        {
            try
            {
                var commision_commisiontype = db.Commision_CommisionType.Include(c => c.Organizations).Include(c => c.Users).Where(c => c.MarkAsDelete == false && c.OrgId == Login.OrganizationId).ToList();
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Comission Type List Page");
                return View(commision_commisiontype.ToList());
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

            return View(new List<Commision_CommisionType>().ToList());

        }
        #endregion

        #region Details Get
        // GET: /CommisionType/Details/5
        [LoginAuthorize]
        public ActionResult Details(int id = 0)
        {
            try
            {
                Commision_CommisionType commision_commisiontype = db.Commision_CommisionType.Find(id);
                if (commision_commisiontype == null)
                {
                    return HttpNotFound();
                }
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                 this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Comission Type Details Page");
                return View(commision_commisiontype);
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
            return View(new Commision_CommisionType());

        }
        #endregion

        #region Create  get
        // GET: /CommisionType/Create
        [LoginAuthorize]
        public ActionResult Create()
        {
            try
            {
                ViewBag.OrgId = new SelectList(db.Organizations.Where(x => x.MarkAsDelete == false), "Id", "Alias");
                ViewBag.ModifiedBy = new SelectList(db.Users.Where(x => x.OrgId == Login.OrganizationId && x.MarkAsDelete == false), "Id", "Salutation");
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
               this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Comission Type Create Page");
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

        #region Create post
        // POST: /CommisionType/Create
        [LoginAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Commision_CommisionType tbl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tbl.OrgId = Login.OrganizationId;
                    tbl.ModifiedBy = Login.Id;
                    tbl.CreateDate = tbl.ModifiedDate = DateTime.Now;
                    tbl.MarkAsDelete = false;
                    db.Commision_CommisionType.Add(tbl);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.OrgId = new SelectList(db.Organizations.Where(x => x.MarkAsDelete == false), "Id", "Alias", tbl.OrgId);
                ViewBag.ModifiedBy = new SelectList(db.Users.Where(x => x.OrgId == Login.OrganizationId && x.MarkAsDelete == false), "Id", "Salutation", tbl.ModifiedBy);
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
               this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Added new Comission Type");
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
            ViewBag.OrgId = new SelectList(db.Organizations.Where(x => x.MarkAsDelete == false), "Id", "Alias", tbl.OrgId);
            ViewBag.ModifiedBy = new SelectList(db.Users.Where(x => x.OrgId == Login.OrganizationId && x.MarkAsDelete == false), "Id", "Salutation", tbl.ModifiedBy);
            return View(tbl);
        }
        #endregion

        #region Edit Get
        // GET: /CommisionType/Edit/5
        [LoginAuthorize]
        public ActionResult Edit(int id = 0)
        {
            try
            {
                Commision_CommisionType commision_commisiontype = db.Commision_CommisionType.Find(id);
                if (commision_commisiontype == null)
                {
                    return HttpNotFound();
                }
                ViewBag.OrgId = new SelectList(db.Organizations.Where(x => x.MarkAsDelete == false), "Id", "Alias", commision_commisiontype.OrgId);
                ViewBag.ModifiedBy = new SelectList(db.Users.Where(x => x.OrgId == Login.OrganizationId && x.MarkAsDelete == false), "Id", "Salutation", commision_commisiontype.ModifiedBy);
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Comission Type Edit Page");
                return View(commision_commisiontype);
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
            Commision_CommisionType commision_commisiontype1 = new Commision_CommisionType();
            ViewBag.OrgId = new SelectList(db.Organizations.Where(x => x.MarkAsDelete == false), "Id", "Alias", commision_commisiontype1.OrgId);
            ViewBag.ModifiedBy = new SelectList(db.Users.Where(x => x.OrgId == Login.OrganizationId && x.MarkAsDelete == false), "Id", "Salutation", commision_commisiontype1.ModifiedBy);
            return View(commision_commisiontype1);
        }
        #endregion

        #region Edit post
        // POST: /CommisionType/Edit/5
        [LoginAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Commision_CommisionType tbl)
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
                  this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Editted Comission Type");
                    return RedirectToAction("Index");
                }
                ViewBag.OrgId = new SelectList(db.Organizations.Where(x => x.MarkAsDelete == false), "Id", "Alias", tbl.OrgId);
                ViewBag.ModifiedBy = new SelectList(db.Users.Where(x => x.OrgId == Login.OrganizationId && x.MarkAsDelete == false), "Id", "Salutation", tbl.ModifiedBy);
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
            ViewBag.OrgId = new SelectList(db.Organizations.Where(x => x.MarkAsDelete == false), "Id", "Alias", tbl.OrgId);
            ViewBag.ModifiedBy = new SelectList(db.Users.Where(x => x.OrgId == Login.OrganizationId && x.MarkAsDelete == false), "Id", "Salutation", tbl.ModifiedBy);
            return View(tbl);

        }
        #endregion

        #region Delete get
        // GET: /CommisionType/Delete/5
        [LoginAuthorize]
        public ActionResult Delete(int id = 0)
        {
            try
            {
                Commision_CommisionType commision_commisiontype = db.Commision_CommisionType.Find(id);
                if (commision_commisiontype == null)
                {
                    return HttpNotFound();
                }
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Comission Type Delete Page");
                return View(commision_commisiontype);
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
            return View(new Commision_CommisionType());
        }

        #endregion

        #region Delete post
        [LoginAuthorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Commision_CommisionType tbl = db.Commision_CommisionType.Find(id);
                tbl.OrgId = Login.OrganizationId;
                tbl.MarkAsDelete = true;
                tbl.ModifiedDate = DateTime.Now;
                tbl.ModifiedBy = Login.Id;
                db.SaveChanges();
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
               this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Has Deleted Comission Type");
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
            return View(new Commision_CommisionType());

        }
        #endregion
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}