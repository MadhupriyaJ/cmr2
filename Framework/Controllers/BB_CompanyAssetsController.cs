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
    public class BB_CompanyAssetsController : Controller
    {
        private FrameworkEntities db = new FrameworkEntities();

        #region Index
        // GET: BB_CompanyAssets
        [LoginAuthorize]
        public async Task<ActionResult> Index()
        {
            try
            {
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
        this.ControllerContext.RouteData.Values["action"].ToString(),
        "The user " + Login.FirstName + " " + Login.LastName + "Entered into Company Assets Page");
                var aBB_CompanyAssets = db.ABB_CompanyAssets.Include(a => a.ABB_CompanyAssetType).Include(a => a.Organizations).Include(a => a.Users);
                return View(await aBB_CompanyAssets.ToListAsync());
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
            var aBB_CompanyAsset = new List<ABB_CompanyAssets>();
            return View(aBB_CompanyAsset.ToList());

        }

        #endregion

        #region Details
        // GET: BB_CompanyAssets/Details/5
        [LoginAuthorize]
        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ABB_CompanyAssets aBB_CompanyAssets = await db.ABB_CompanyAssets.FindAsync(id);
                if (aBB_CompanyAssets == null)
                {
                    return HttpNotFound();
                }
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
        this.ControllerContext.RouteData.Values["action"].ToString(),
        "The user " + Login.FirstName + " " + Login.LastName + "Entered into Company Asset Details Page");
                return View(aBB_CompanyAssets);
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
            ABB_CompanyAssets aBB_CompanyAsset = new ABB_CompanyAssets();
            return View(aBB_CompanyAsset);
        }

        #endregion

        #region Create
        // GET: BB_CompanyAssets/Create
        [LoginAuthorize]
        public ActionResult Create()
        {
            try
            {
                ViewBag.AssetTypeId = new SelectList(db.ABB_CompanyAssetType, "Id", "AssetType");
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

        // POST: BB_CompanyAssets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public async Task<ActionResult> Create([Bind(Include = "Id,OrgId,AssetName,AssetTypeId,CreateDate,ModifiedDate,ModifiedBy,MarkAsDelete")] ABB_CompanyAssets tbl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tbl.OrgId = Login.OrganizationId;
                    tbl.CreateDate = tbl.ModifiedDate = DateTime.Now;
                    tbl.ModifiedBy = Login.Id;
                    tbl.MarkAsDelete = false;
                    db.ABB_CompanyAssets.Add(tbl);
                    await db.SaveChangesAsync();
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
      this.ControllerContext.RouteData.Values["action"].ToString(),
      "The user " + Login.FirstName + " " + Login.LastName + "Added new Company Asset Page");
                    return RedirectToAction("Index");
                }

                ViewBag.AssetTypeId = new SelectList(db.ABB_CompanyAssetType, "Id", "AssetType", tbl.AssetTypeId);
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
            ViewBag.AssetTypeId = new SelectList(db.ABB_CompanyAssetType, "Id", "AssetType", tbl.AssetTypeId);
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", tbl.OrgId);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", tbl.ModifiedBy);
            return View(tbl);
        }

        #endregion

        #region Edit
        // GET: BB_CompanyAssets/Edit/5
        [LoginAuthorize]
        public async Task<ActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ABB_CompanyAssets aBB_CompanyAssets = await db.ABB_CompanyAssets.FindAsync(id);
                if (aBB_CompanyAssets == null)
                {
                    return HttpNotFound();
                }
                ViewBag.AssetTypeId = new SelectList(db.ABB_CompanyAssetType, "Id", "AssetType", aBB_CompanyAssets.AssetTypeId);
                ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_CompanyAssets.OrgId);
                ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_CompanyAssets.ModifiedBy);
                return View(aBB_CompanyAssets);
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
            ABB_CompanyAssets aBB_CompanyAsset = new ABB_CompanyAssets();
            ViewBag.AssetTypeId = new SelectList(db.ABB_CompanyAssetType, "Id", "AssetType", aBB_CompanyAsset.AssetTypeId);
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_CompanyAsset.OrgId);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_CompanyAsset.ModifiedBy);
            return View(aBB_CompanyAsset);
        }

        // POST: BB_CompanyAssets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public async Task<ActionResult> Edit([Bind(Include = "Id,OrgId,AssetName,AssetTypeId,CreateDate,ModifiedDate,ModifiedBy,MarkAsDelete")] ABB_CompanyAssets aBB_CompanyAssets)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(aBB_CompanyAssets).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                    this.ControllerContext.RouteData.Values["action"].ToString(),
                    "The user " + Login.FirstName + " " + Login.LastName + "Edited Company Asset Page");
                    return RedirectToAction("Index");
                }
                ViewBag.AssetTypeId = new SelectList(db.ABB_CompanyAssetType, "Id", "AssetType", aBB_CompanyAssets.AssetTypeId);
                ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_CompanyAssets.OrgId);
                ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_CompanyAssets.ModifiedBy);
                return View(aBB_CompanyAssets);
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
            ViewBag.AssetTypeId = new SelectList(db.ABB_CompanyAssetType, "Id", "AssetType", aBB_CompanyAssets.AssetTypeId);
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_CompanyAssets.OrgId);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_CompanyAssets.ModifiedBy);
            return View(aBB_CompanyAssets);
        }

        #endregion

        #region Delete
        // GET: BB_CompanyAssets/Delete/5
        [LoginAuthorize]
        public async Task<ActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ABB_CompanyAssets aBB_CompanyAssets = await db.ABB_CompanyAssets.FindAsync(id);
                if (aBB_CompanyAssets == null)
                {
                    return HttpNotFound();
                }
                return View(aBB_CompanyAssets);
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
            ABB_CompanyAssets aBB_CompanyAsset = new ABB_CompanyAssets();
            return View(aBB_CompanyAsset);
        }

        // POST: BB_CompanyAssets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                ABB_CompanyAssets aBB_CompanyAssets = await db.ABB_CompanyAssets.FindAsync(id);
                aBB_CompanyAssets.MarkAsDelete = true;
                aBB_CompanyAssets.ModifiedDate = DateTime.Now;
                aBB_CompanyAssets.ModifiedBy = Login.Id;
                db.Entry(aBB_CompanyAssets).State = EntityState.Modified;
                //db.ABB_CompanyAssets.Remove(aBB_CompanyAssets);
                await db.SaveChangesAsync();
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                this.ControllerContext.RouteData.Values["action"].ToString(),
                "The user " + Login.FirstName + " " + Login.LastName + "Deleted Company Asset");
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
            ABB_CompanyAssets aBB_CompanyAsset = new ABB_CompanyAssets();
            return View(aBB_CompanyAsset);
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
