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
    public class BB_CompanyAccountController : Controller
    {
        private FrameworkEntities db = new FrameworkEntities();

        #region Index
        // GET: BB_CompanyAccount
        [LoginAuthorize]
        public async Task<ActionResult> Index()
        {
            try
            {
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
         this.ControllerContext.RouteData.Values["action"].ToString(),
         "The user " + Login.FirstName + " " + Login.LastName + "Entered into Company Account Page");

                var aBB_CompanyAccount = db.ABB_CompanyAccount.Include(a => a.ABB_Company).Include(a => a.Organizations).Include(a => a.Users);
                return View(await aBB_CompanyAccount.ToListAsync());
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
            var aBB_CompanyAccounts = new List<ABB_CompanyAccount>();
            return View(aBB_CompanyAccounts.ToList());

        }

        #endregion

        #region Details
        // GET: BB_CompanyAccount/Details/5
        [LoginAuthorize]
        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ABB_CompanyAccount aBB_CompanyAccount = await db.ABB_CompanyAccount.FindAsync(id);
                if (aBB_CompanyAccount == null)
                {
                    return HttpNotFound();
                }
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                this.ControllerContext.RouteData.Values["action"].ToString(),
                "The user " + Login.FirstName + " " + Login.LastName + "Entered into Company Account Details Page");
                return View(aBB_CompanyAccount);
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
            ABB_CompanyAccount aBB_CompanyAccounts = new ABB_CompanyAccount();
            return View(aBB_CompanyAccounts);

        }

        #endregion

        #region Create
        // GET: BB_CompanyAccount/Create
        [LoginAuthorize]
        public ActionResult Create()
        {
            try
            {
                ViewBag.CompanyId = new SelectList(db.ABB_Company, "Id", "CRNo");
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

        // POST: BB_CompanyAccount/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public async Task<ActionResult> Create([Bind(Include = "Id,OrgId,CompanyId,AccountNumber,CreateDate,ModifiedDate,ModifiedBy,MarkAsDelete")] ABB_CompanyAccount tbl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tbl.OrgId = Login.OrganizationId;
                    tbl.CreateDate = tbl.ModifiedDate = DateTime.Now;
                    tbl.ModifiedBy = Login.Id;
                    tbl.MarkAsDelete = false;
                    db.ABB_CompanyAccount.Add(tbl);
                    await db.SaveChangesAsync();
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                    this.ControllerContext.RouteData.Values["action"].ToString(),
                    "The user " + Login.FirstName + " " + Login.LastName + "Added new Company Account Page");
                    return RedirectToAction("Index");
                }

                ViewBag.CompanyId = new SelectList(db.ABB_Company, "Id", "CRNo", tbl.CompanyId);
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
            ViewBag.CompanyId = new SelectList(db.ABB_Company, "Id", "CRNo", tbl.CompanyId);
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", tbl.OrgId);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", tbl.ModifiedBy);
            return View(tbl);

        }

        #endregion

        #region Edit
        // GET: BB_CompanyAccount/Edit/5
        [LoginAuthorize]
        public async Task<ActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ABB_CompanyAccount aBB_CompanyAccount = await db.ABB_CompanyAccount.FindAsync(id);
                if (aBB_CompanyAccount == null)
                {
                    return HttpNotFound();
                }
                ViewBag.CompanyId = new SelectList(db.ABB_Company, "Id", "CRNo", aBB_CompanyAccount.CompanyId);
                ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_CompanyAccount.OrgId);
                ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_CompanyAccount.ModifiedBy);
                return View(aBB_CompanyAccount);
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
            ABB_CompanyAccount aBB_CompanyAccounts = new ABB_CompanyAccount();
            ViewBag.CompanyId = new SelectList(db.ABB_Company, "Id", "CRNo", aBB_CompanyAccounts.CompanyId);
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_CompanyAccounts.OrgId);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_CompanyAccounts.ModifiedBy);
            return View(aBB_CompanyAccounts);
        }

        // POST: BB_CompanyAccount/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public async Task<ActionResult> Edit([Bind(Include = "Id,OrgId,CompanyId,AccountNumber,CreateDate,ModifiedDate,ModifiedBy,MarkAsDelete")] ABB_CompanyAccount aBB_CompanyAccount)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(aBB_CompanyAccount).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                    this.ControllerContext.RouteData.Values["action"].ToString(),
                    "The user " + Login.FirstName + " " + Login.LastName + "Edited Company Account Page");
                    return RedirectToAction("Index");
                }
                ViewBag.CompanyId = new SelectList(db.ABB_Company, "Id", "CRNo", aBB_CompanyAccount.CompanyId);
                ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_CompanyAccount.OrgId);
                ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_CompanyAccount.ModifiedBy);
                return View(aBB_CompanyAccount);
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
            ViewBag.CompanyId = new SelectList(db.ABB_Company, "Id", "CRNo", aBB_CompanyAccount.CompanyId);
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_CompanyAccount.OrgId);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_CompanyAccount.ModifiedBy);
            return View(aBB_CompanyAccount);

        }

        #endregion

        #region Delete
        // GET: BB_CompanyAccount/Delete/5
        [LoginAuthorize]
        public async Task<ActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ABB_CompanyAccount aBB_CompanyAccount = await db.ABB_CompanyAccount.FindAsync(id);
                if (aBB_CompanyAccount == null)
                {
                    return HttpNotFound();
                }
                return View(aBB_CompanyAccount);
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
            ABB_CompanyAccount aBB_CompanyAccounts = new ABB_CompanyAccount();
            return View(aBB_CompanyAccounts);

        }

        // POST: BB_CompanyAccount/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                ABB_CompanyAccount aBB_CompanyAccount = await db.ABB_CompanyAccount.FindAsync(id);
                aBB_CompanyAccount.MarkAsDelete = true;
                aBB_CompanyAccount.ModifiedDate = DateTime.Now;
                aBB_CompanyAccount.ModifiedBy = Login.Id;
                db.Entry(aBB_CompanyAccount).State = EntityState.Modified;
                //db.ABB_CompanyAccount.Remove(aBB_CompanyAccount);
                await db.SaveChangesAsync();
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                    this.ControllerContext.RouteData.Values["action"].ToString(),
                    "The user " + Login.FirstName + " " + Login.LastName + "Deleted Company Account Page");
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
            ABB_CompanyAccount aBB_CompanyAccounts = new ABB_CompanyAccount();
            return View(aBB_CompanyAccounts);

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
