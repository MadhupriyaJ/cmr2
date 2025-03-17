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
using System.Threading;
using System.IO;
using ExceptionLogging;

namespace FramWork.Controllers
{
    public class BB_AccountTypesController : Controller
    {
        private FrameworkEntities db = new FrameworkEntities();

        #region Index
        // GET: BB_AccountTypes
        [LoginAuthorize]
        public async Task<ActionResult> Index()
        {
            try
            {
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                this.ControllerContext.RouteData.Values["action"].ToString(),
                "The user " + Login.FirstName + " " + Login.LastName + "Entered Account Type Page");
                var aBB_AccountTypes = db.ABB_AccountTypes.Include(a => a.Organizations).Include(a => a.Users);
                return View(await aBB_AccountTypes.ToListAsync());
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

            var aBB_AccountType = new List<ABB_AccountTypes>();
            return View(aBB_AccountType.ToList());
        }
        #endregion

        #region Details
        // GET: BB_AccountTypes/Details/5
        [LoginAuthorize]
        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ABB_AccountTypes aBB_AccountTypes = await db.ABB_AccountTypes.FindAsync(id);
                if (aBB_AccountTypes == null)
                {
                    return HttpNotFound();
                }
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                this.ControllerContext.RouteData.Values["action"].ToString(),
                "The user " + Login.FirstName + " " + Login.LastName + "Entered Account Type Details Page");
                return View(aBB_AccountTypes);
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
            ABB_AccountTypes objaBB_AccountTypes = new ABB_AccountTypes();
            return View(objaBB_AccountTypes);
        }

        #endregion

        #region Create
        // GET: BB_AccountTypes/Create
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


        // POST: BB_AccountTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public async Task<ActionResult> Create([Bind(Include = "Id,OrgId,Name,CreateDate,ModifiedDate,ModifiedBy,MarkAsDelete")] ABB_AccountTypes aBB_AccountTypes)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    aBB_AccountTypes.OrgId = Login.OrganizationId;
                    aBB_AccountTypes.CreateDate = aBB_AccountTypes.ModifiedDate = DateTime.Now;
                    aBB_AccountTypes.ModifiedBy = Login.Id;
                    aBB_AccountTypes.MarkAsDelete = false;
                    db.ABB_AccountTypes.Add(aBB_AccountTypes);
                    await db.SaveChangesAsync();
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
             this.ControllerContext.RouteData.Values["action"].ToString(),
             "The user " + Login.FirstName + " " + Login.LastName + "Created  new Account Type");
                    return RedirectToAction("Index");
                }

                ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_AccountTypes.OrgId);
                ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_AccountTypes.ModifiedBy);
              
                return View(aBB_AccountTypes);
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
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_AccountTypes.OrgId);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_AccountTypes.ModifiedBy);
            return View(aBB_AccountTypes);

        }

        #endregion

        #region Edit

        // GET: BB_AccountTypes/Edit/5
        [LoginAuthorize]
        public async Task<ActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ABB_AccountTypes aBB_AccountTypes = await db.ABB_AccountTypes.FindAsync(id);
                if (aBB_AccountTypes == null)
                {
                    return HttpNotFound();
                }
                ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_AccountTypes.OrgId);
                ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_AccountTypes.ModifiedBy);
                return View(aBB_AccountTypes);
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

           
            ABB_AccountTypes aBB_AccountType = new ABB_AccountTypes();
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_AccountType.OrgId);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_AccountType.ModifiedBy);
            return View(aBB_AccountType);

        }

        // POST: BB_AccountTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public async Task<ActionResult> Edit([Bind(Include = "Id,OrgId,Name,CreateDate,ModifiedDate,ModifiedBy,MarkAsDelete")] ABB_AccountTypes aBB_AccountTypes)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    aBB_AccountTypes.ModifiedDate = DateTime.Now;
                    db.Entry(aBB_AccountTypes).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
             this.ControllerContext.RouteData.Values["action"].ToString(),
             "The user " + Login.FirstName + " " + Login.LastName + "Edited  Account Type Page");
                    return RedirectToAction("Index");
                }
                ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_AccountTypes.OrgId);
                ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_AccountTypes.ModifiedBy);
                return View(aBB_AccountTypes);
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
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_AccountTypes.OrgId);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_AccountTypes.ModifiedBy);
            return View(aBB_AccountTypes);
        }

        #endregion

        #region Delete

        // GET: BB_AccountTypes/Delete/5
        [LoginAuthorize]
        public async Task<ActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ABB_AccountTypes aBB_AccountTypes = await db.ABB_AccountTypes.FindAsync(id);
                if (aBB_AccountTypes == null)
                {
                    return HttpNotFound();
                }
                return View(aBB_AccountTypes);
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
            ABB_AccountTypes objABB_AccountTypes = new ABB_AccountTypes();
            return View(objABB_AccountTypes);

        }

        // POST: BB_AccountTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                ABB_AccountTypes aBB_AccountTypes = await db.ABB_AccountTypes.FindAsync(id);
                aBB_AccountTypes.MarkAsDelete = true;
                aBB_AccountTypes.ModifiedDate = DateTime.Now;
                aBB_AccountTypes.ModifiedBy = Login.Id;
                db.Entry(aBB_AccountTypes).State = EntityState.Modified;
                await db.SaveChangesAsync();
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
           this.ControllerContext.RouteData.Values["action"].ToString(),
           "The user " + Login.FirstName + " " + Login.LastName + "Deleted  Account Type Page");
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
            ABB_AccountTypes objABB_AccountTypes = new ABB_AccountTypes();
            return View(objABB_AccountTypes);
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
