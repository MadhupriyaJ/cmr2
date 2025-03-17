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
    public class BB_PipeLineController : Controller
    {
        private FrameworkEntities db = new FrameworkEntities();

        #region Index
        // GET: BB_PipeLine
        [LoginAuthorize]
        public async Task<ActionResult> Index()
        {
            try
            {
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
             this.ControllerContext.RouteData.Values["action"].ToString(),
             "The user " + Login.FirstName + " " + Login.LastName + "Entered into Pipeline Page");
                var aBB_PipeLine = db.ABB_PipeLine.Include(a => a.Organizations).Include(a => a.Users);
                return View(await aBB_PipeLine.ToListAsync());
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
            var aBB_PipeLines = new List<ABB_PipeLine>();
            return View(aBB_PipeLines.ToList());
        }
        #endregion

        #region Details
        // GET: BB_PipeLine/Details/5
        [LoginAuthorize]
        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ABB_PipeLine aBB_PipeLine = await db.ABB_PipeLine.FindAsync(id);
                if (aBB_PipeLine == null)
                {
                    return HttpNotFound();
                }
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
             this.ControllerContext.RouteData.Values["action"].ToString(),
             "The user " + Login.FirstName + " " + Login.LastName + "Entered into Pipeline Details Page");
                return View(aBB_PipeLine);
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
            ABB_PipeLine aBB_PipeLine1 = new ABB_PipeLine();
            return View(aBB_PipeLine1);

        }
        #endregion

        #region Create
        // GET: BB_PipeLine/Create
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

        // POST: BB_PipeLine/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public async Task<ActionResult> Create([Bind(Include = "Id,OrgId,Name,CreateDate,ModifiedDate,ModifiedBy,MarkAsDelete")] ABB_PipeLine aBB_PipeLine)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    aBB_PipeLine.OrgId = Login.OrganizationId;
                    aBB_PipeLine.CreateDate = aBB_PipeLine.ModifiedDate = DateTime.Now;
                    aBB_PipeLine.ModifiedBy = Login.Id;
                    aBB_PipeLine.MarkAsDelete = false;
                    db.ABB_PipeLine.Add(aBB_PipeLine);
                    await db.SaveChangesAsync();
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
           this.ControllerContext.RouteData.Values["action"].ToString(),
           "The user " + Login.FirstName + " " + Login.LastName + "Added new Pipeline");
                    return RedirectToAction("Index");
                }

                ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_PipeLine.OrgId);
                ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_PipeLine.ModifiedBy);
                return View(aBB_PipeLine);
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
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_PipeLine.OrgId);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_PipeLine.ModifiedBy);
            return View(aBB_PipeLine);

        }

        #endregion
     
        #region Edit
        // GET: BB_PipeLine/Edit/5
        [LoginAuthorize]
        public async Task<ActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ABB_PipeLine aBB_PipeLine = await db.ABB_PipeLine.FindAsync(id);
                if (aBB_PipeLine == null)
                {
                    return HttpNotFound();
                }
                ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_PipeLine.OrgId);
                ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_PipeLine.ModifiedBy);
                return View(aBB_PipeLine);
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
            ABB_PipeLine aBB_PipeLine1 = new ABB_PipeLine();
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_PipeLine1.OrgId);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_PipeLine1.ModifiedBy);
            return View(aBB_PipeLine1);

        }

        // POST: BB_PipeLine/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public async Task<ActionResult> Edit([Bind(Include = "Id,OrgId,Name,CreateDate,ModifiedDate,ModifiedBy,MarkAsDelete")] ABB_PipeLine aBB_PipeLine)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(aBB_PipeLine).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
           this.ControllerContext.RouteData.Values["action"].ToString(),
           "The user " + Login.FirstName + " " + Login.LastName + "Edited Pipeline Page");
                    return RedirectToAction("Index");
                }
                ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_PipeLine.OrgId);
                ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_PipeLine.ModifiedBy);
                return View(aBB_PipeLine);
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
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_PipeLine.OrgId);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_PipeLine.ModifiedBy);
            return View(aBB_PipeLine);
        }

        #endregion
        
        #region Delete
        // GET: BB_PipeLine/Delete/5
        [LoginAuthorize]
        public async Task<ActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ABB_PipeLine aBB_PipeLine = await db.ABB_PipeLine.FindAsync(id);
                if (aBB_PipeLine == null)
                {
                    return HttpNotFound();
                }
                return View(aBB_PipeLine);
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
            ABB_PipeLine objaBB_PipeLine = new ABB_PipeLine();
            return View(objaBB_PipeLine);
        }

        // POST: BB_PipeLine/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                ABB_PipeLine aBB_PipeLine = await db.ABB_PipeLine.FindAsync(id);
                aBB_PipeLine.MarkAsDelete = true;
                aBB_PipeLine.ModifiedDate = DateTime.Now;
                aBB_PipeLine.ModifiedBy = Login.Id;
                db.Entry(aBB_PipeLine).State = EntityState.Modified;
                await db.SaveChangesAsync();
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
         this.ControllerContext.RouteData.Values["action"].ToString(),
         "The user " + Login.FirstName + " " + Login.LastName + "Deleted Pipeline Page");
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
            ABB_PipeLine objABB_PipeLine = new ABB_PipeLine();
            return View(objABB_PipeLine);

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
