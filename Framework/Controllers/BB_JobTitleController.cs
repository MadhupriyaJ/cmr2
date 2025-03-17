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
using System.IO;
using ExceptionLogging;

namespace FramWork.Controllers
{
    public class BB_JobTitleController : Controller
    {
        private FrameworkEntities db = new FrameworkEntities();

        #region Index
        // GET: BB_JobTitle
        [LoginAuthorize]
        public async Task<ActionResult> Index()
        {
            try
            {
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
              this.ControllerContext.RouteData.Values["action"].ToString(),
              "The user " + Login.FirstName + " " + Login.LastName + "Entered into Job Title Page");
                var aBB_JobTitle = db.ABB_JobTitle.Include(a => a.Organizations).Include(a => a.Users);
                return View(await aBB_JobTitle.ToListAsync());
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
            var aBB_JobTitles = new List<ABB_JobTitle>();
            return View(aBB_JobTitles.ToList());


        }
        #endregion

        #region Details
        // GET: BB_JobTitle/Details/5
        [LoginAuthorize]
        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ABB_JobTitle aBB_JobTitle = await db.ABB_JobTitle.FindAsync(id);
                if (aBB_JobTitle == null)
                {
                    return HttpNotFound();
                }
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
              this.ControllerContext.RouteData.Values["action"].ToString(),
              "The user " + Login.FirstName + " " + Login.LastName + "Entered into Job Title Details Page");
                return View(aBB_JobTitle);
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
            ABB_JobTitle objaBB_JobTitle = new ABB_JobTitle();
            return View(objaBB_JobTitle);

        }

        #endregion
      
        #region Create
        // GET: BB_JobTitle/Create
        [LoginAuthorize]
        public ActionResult Create()
        {
            try
            {
                ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias");
                ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation");
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

        // POST: BB_JobTitle/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public async Task<ActionResult> Create([Bind(Include = "Id,OrgId,Title,CreateDate,ModifiedDate,ModifiedBy,MarkAsDelete")] ABB_JobTitle tbl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tbl.OrgId = Login.OrganizationId;
                    tbl.CreateDate = tbl.ModifiedDate = DateTime.Now;
                    tbl.ModifiedBy = Login.Id;
                    tbl.MarkAsDelete = false;
                    db.ABB_JobTitle.Add(tbl);
                    await db.SaveChangesAsync();
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
              this.ControllerContext.RouteData.Values["action"].ToString(),
              "The user " + Login.FirstName + " " + Login.LastName + "Created new Job Title");
                    return RedirectToAction("Index");
                }

                ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", tbl.OrgId);
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
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", tbl.OrgId);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", tbl.ModifiedBy);
            return View(tbl);

        }

        #endregion
      
        #region Edit
        // GET: BB_JobTitle/Edit/5
        [LoginAuthorize]
        public async Task<ActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ABB_JobTitle aBB_JobTitle = await db.ABB_JobTitle.FindAsync(id);
                if (aBB_JobTitle == null)
                {
                    return HttpNotFound();
                }
                ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_JobTitle.OrgId);
                ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_JobTitle.ModifiedBy);
                return View(aBB_JobTitle);
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
            ABB_JobTitle objaBB_JobTitle = new ABB_JobTitle();
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", objaBB_JobTitle.OrgId);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", objaBB_JobTitle.ModifiedBy);
            return View(objaBB_JobTitle);

        }

        // POST: BB_JobTitle/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public async Task<ActionResult> Edit([Bind(Include = "Id,OrgId,Title,CreateDate,ModifiedDate,ModifiedBy,MarkAsDelete")] ABB_JobTitle aBB_JobTitle)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(aBB_JobTitle).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
          this.ControllerContext.RouteData.Values["action"].ToString(),
          "The user " + Login.FirstName + " " + Login.LastName + "Edited Job Title Page");
                    return RedirectToAction("Index");
                }
                ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_JobTitle.OrgId);
                ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_JobTitle.ModifiedBy);
                return View(aBB_JobTitle);
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
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_JobTitle.OrgId);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_JobTitle.ModifiedBy);
            return View(aBB_JobTitle);
        }
      
        #endregion
    
        #region Delete
        // GET: BB_JobTitle/Delete/5
        [LoginAuthorize]
        public async Task<ActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ABB_JobTitle aBB_JobTitle = await db.ABB_JobTitle.FindAsync(id);
                if (aBB_JobTitle == null)
                {
                    return HttpNotFound();
                }
                return View(aBB_JobTitle);
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
            ABB_JobTitle objaBB_JobTitle = new ABB_JobTitle();
            return View(objaBB_JobTitle);

        }

        // POST: BB_JobTitle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                ABB_JobTitle aBB_JobTitle = await db.ABB_JobTitle.FindAsync(id);
                aBB_JobTitle.MarkAsDelete = true;
                aBB_JobTitle.ModifiedDate = DateTime.Now;
                aBB_JobTitle.ModifiedBy = Login.Id;
                db.Entry(aBB_JobTitle).State = EntityState.Modified;
                await db.SaveChangesAsync();
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                this.ControllerContext.RouteData.Values["action"].ToString(),
                "The user " + Login.FirstName + " " + Login.LastName + "Deleted Job Title ");
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
            ABB_JobTitle objaBB_JobTitle = new ABB_JobTitle();
            return View(objaBB_JobTitle);

        }

        #endregion

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
       
    }
}
