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
    public class BB_BusinessSectorController : Controller
    {
        private FrameworkEntities db = new FrameworkEntities();

        #region Index
        // GET: BB_BusinessSector
        [LoginAuthorize]
        public async Task<ActionResult> Index()
        {
            try
            {
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
               this.ControllerContext.RouteData.Values["action"].ToString(),
               "The user " + Login.FirstName + " " + Login.LastName + "Entered Business Sector Page");
                var aBB_BusinessSector = db.ABB_BusinessSector.Include(a => a.Organizations).Include(a => a.Users);
                return View(await aBB_BusinessSector.ToListAsync());
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
            var aBB_BusinessSector1 = new List<ABB_BusinessSector>();
            return View(aBB_BusinessSector1.ToList());

        }

        #endregion

        #region Details
        // GET: BB_BusinessSector/Details/5
        [LoginAuthorize]
        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ABB_BusinessSector aBB_BusinessSector = await db.ABB_BusinessSector.FindAsync(id);
                if (aBB_BusinessSector == null)
                {
                    return HttpNotFound();
                }
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
             this.ControllerContext.RouteData.Values["action"].ToString(),
             "The user " + Login.FirstName + " " + Login.LastName + "Entered Business Sector Details Page");
                return View(aBB_BusinessSector);
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
            ABB_BusinessSector objABB_BusinessSector = new ABB_BusinessSector();
            return View(objABB_BusinessSector);
        }

        #endregion

        #region Create
        // GET: BB_BusinessSector/Create
        [LoginAuthorize]
        public ActionResult Create()
        {
            try
            {
                ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias");
                ViewBag.Id = new SelectList(db.Users, "Id", "Salutation");
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

        // POST: BB_BusinessSector/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public async Task<ActionResult> Create([Bind(Include = "Id,OrgId,Sector,Description,CreateDate,ModifiedDate,ModifiedBy,MarkAsDelete")] ABB_BusinessSector aBB_BusinessSector)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    aBB_BusinessSector.OrgId = Login.OrganizationId;
                    aBB_BusinessSector.CreateDate = aBB_BusinessSector.ModifiedDate = DateTime.Now;
                    aBB_BusinessSector.ModifiedBy = Login.Id;
                    aBB_BusinessSector.MarkAsDelete = false;
                    db.ABB_BusinessSector.Add(aBB_BusinessSector);
                    await db.SaveChangesAsync();

                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
             this.ControllerContext.RouteData.Values["action"].ToString(),
             "The user " + Login.FirstName + " " + Login.LastName + "Created new Business Sector Page");
                    return RedirectToAction("Index");
                }

                ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_BusinessSector.OrgId);
                ViewBag.Id = new SelectList(db.Users, "Id", "Salutation", aBB_BusinessSector.Id);
                return View(aBB_BusinessSector);
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
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_BusinessSector.OrgId);
            ViewBag.Id = new SelectList(db.Users, "Id", "Salutation", aBB_BusinessSector.Id);
            return View(aBB_BusinessSector);
        }

        #endregion

        #region Edit
        // GET: BB_BusinessSector/Edit/5
        [LoginAuthorize]
        public async Task<ActionResult> Edit(int? id)
        {
            try
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ABB_BusinessSector aBB_BusinessSector = await db.ABB_BusinessSector.FindAsync(id);
                if (aBB_BusinessSector == null)
                {
                    return HttpNotFound();
                }
                ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_BusinessSector.OrgId);
                ViewBag.Id = new SelectList(db.Users, "Id", "Salutation", aBB_BusinessSector.Id);
                return View(aBB_BusinessSector);
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
            ABB_BusinessSector objABB_BusinessSector = new ABB_BusinessSector();
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", objABB_BusinessSector.OrgId);
            ViewBag.Id = new SelectList(db.Users, "Id", "Salutation", objABB_BusinessSector.Id);
            return View(objABB_BusinessSector);

        }

        // POST: BB_BusinessSector/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public async Task<ActionResult> Edit([Bind(Include = "Id,OrgId,Sector,Description,CreateDate,ModifiedDate,ModifiedBy,MarkAsDelete")] ABB_BusinessSector aBB_BusinessSector)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(aBB_BusinessSector).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
             this.ControllerContext.RouteData.Values["action"].ToString(),
             "The user " + Login.FirstName + " " + Login.LastName + "Edited Business Sector Page");
                    return RedirectToAction("Index");
                }
                ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_BusinessSector.OrgId);
                ViewBag.Id = new SelectList(db.Users, "Id", "Salutation", aBB_BusinessSector.Id);
                return View(aBB_BusinessSector);
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
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_BusinessSector.OrgId);
            ViewBag.Id = new SelectList(db.Users, "Id", "Salutation", aBB_BusinessSector.Id);
            return View(aBB_BusinessSector);
        }

        #endregion

        #region Delete

        // GET: BB_BusinessSector/Delete/5
        [LoginAuthorize]
        public async Task<ActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ABB_BusinessSector aBB_BusinessSector = await db.ABB_BusinessSector.FindAsync(id);
                if (aBB_BusinessSector == null)
                {
                    return HttpNotFound();
                }
                return View(aBB_BusinessSector);
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
            ABB_BusinessSector objABB_BusinessSector = new ABB_BusinessSector();
            return View(objABB_BusinessSector);
        }

        // POST: BB_BusinessSector/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                ABB_BusinessSector aBB_BusinessSector = await db.ABB_BusinessSector.FindAsync(id);
                aBB_BusinessSector.ModifiedDate = DateTime.Now;
                aBB_BusinessSector.ModifiedBy = Login.Id;
              
                aBB_BusinessSector.MarkAsDelete = true;
                db.Entry(aBB_BusinessSector).State = EntityState.Modified;
                await db.SaveChangesAsync();
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
             this.ControllerContext.RouteData.Values["action"].ToString(),
             "The user " + Login.FirstName + " " + Login.LastName + "Deleted Business Sector Page");
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
            ABB_BusinessSector objABB_BusinessSector = new ABB_BusinessSector();
            return View(objABB_BusinessSector);

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
