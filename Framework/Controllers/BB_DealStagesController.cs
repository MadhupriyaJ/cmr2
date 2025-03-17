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
    public class BB_DealStagesController : Controller
    {
        private FrameworkEntities db = new FrameworkEntities();

        #region Index
        // GET: BB_DealStages
        [LoginAuthorize]
        public async Task<ActionResult> Index()
        {
            try
            {
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
               this.ControllerContext.RouteData.Values["action"].ToString(),
               "The user " + Login.FirstName + " " + Login.LastName + "Entered into Deal Stage Page");
                var aBB_DealStages = db.ABB_DealStages.Include(a => a.ABB_PipeLine).Include(a => a.Organizations).Include(a => a.Users);
                return View(await aBB_DealStages.ToListAsync());
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
            var aBB_DealStage = new List<ABB_DealStages>();
            return View(aBB_DealStage.ToList());

        }

        #endregion

        #region Details
        // GET: BB_DealStages/Details/5
        [LoginAuthorize]
        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ABB_DealStages aBB_DealStages = await db.ABB_DealStages.FindAsync(id);
                if (aBB_DealStages == null)
                {
                    return HttpNotFound();
                }
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
               this.ControllerContext.RouteData.Values["action"].ToString(),
               "The user " + Login.FirstName + " " + Login.LastName + "Entered into Deal Stage Details Page");
                return View(aBB_DealStages);
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
            ABB_DealStages aBB_DealStage = new ABB_DealStages();
            return View(aBB_DealStage);

        }

        #endregion

        #region Create
        // GET: BB_DealStages/Create
        [LoginAuthorize]
        public ActionResult Create()
        {
            try
            {
                ViewBag.PipeLineId = new SelectList(db.ABB_PipeLine, "Id", "Name");
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

        // POST: BB_DealStages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public async Task<ActionResult> Create([Bind(Include = "Id,OrgId,PipeLineId,Name,IsVisible,SortOrder,CreateDate,ModifiedDate,ModifiedBy,MarkAsDelete")] ABB_DealStages aBB_DealStages)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    aBB_DealStages.OrgId = Login.OrganizationId;
                    aBB_DealStages.CreateDate = aBB_DealStages.ModifiedDate = DateTime.Now;
                    aBB_DealStages.ModifiedBy = Login.Id;

                    aBB_DealStages.MarkAsDelete = false;
                    db.ABB_DealStages.Add(aBB_DealStages);
                    await db.SaveChangesAsync();
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
              this.ControllerContext.RouteData.Values["action"].ToString(),
              "The user " + Login.FirstName + " " + Login.LastName + "Created new Deal Stage");
                    return RedirectToAction("Index");
                }

                ViewBag.PipeLineId = new SelectList(db.ABB_PipeLine, "Id", "Name", aBB_DealStages.PipeLineId);
                ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_DealStages.OrgId);
                ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_DealStages.ModifiedBy);
                return View(aBB_DealStages);
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
            ViewBag.PipeLineId = new SelectList(db.ABB_PipeLine, "Id", "Name", aBB_DealStages.PipeLineId);
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_DealStages.OrgId);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_DealStages.ModifiedBy);
            return View(aBB_DealStages);
        }

        #endregion

        #region Edit
        // GET: BB_DealStages/Edit/5
        [LoginAuthorize]
        public async Task<ActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ABB_DealStages aBB_DealStages = await db.ABB_DealStages.FindAsync(id);
                if (aBB_DealStages == null)
                {
                    return HttpNotFound();
                }
                ViewBag.PipeLineId = new SelectList(db.ABB_PipeLine, "Id", "Name", aBB_DealStages.PipeLineId);
                ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_DealStages.OrgId);
                ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_DealStages.ModifiedBy);
                return View(aBB_DealStages);
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
            ABB_DealStages aBB_DealStage = new ABB_DealStages();
            ViewBag.PipeLineId = new SelectList(db.ABB_PipeLine, "Id", "Name", aBB_DealStage.PipeLineId);
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_DealStage.OrgId);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_DealStage.ModifiedBy);
         
            return View(aBB_DealStage);

        }

        // POST: BB_DealStages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public async Task<ActionResult> Edit([Bind(Include = "Id,OrgId,PipeLineId,Name,IsVisible,SortOrder,CreateDate,ModifiedDate,ModifiedBy,MarkAsDelete")] ABB_DealStages aBB_DealStages)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(aBB_DealStages).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                     this.ControllerContext.RouteData.Values["action"].ToString(),
                     "The user " + Login.FirstName + " " + Login.LastName + "Edited Deal Stage");
                    return RedirectToAction("Index");
                }
                ViewBag.PipeLineId = new SelectList(db.ABB_PipeLine, "Id", "Name", aBB_DealStages.PipeLineId);
                ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_DealStages.OrgId);
                ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_DealStages.ModifiedBy);
                return View(aBB_DealStages);
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
            ViewBag.PipeLineId = new SelectList(db.ABB_PipeLine, "Id", "Name", aBB_DealStages.PipeLineId);
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_DealStages.OrgId);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_DealStages.ModifiedBy);
            return View(aBB_DealStages);
        }

        #endregion

        #region Delete
        // GET: BB_DealStages/Delete/5
        [LoginAuthorize]
        public async Task<ActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ABB_DealStages aBB_DealStages = await db.ABB_DealStages.FindAsync(id);
                if (aBB_DealStages == null)
                {
                    return HttpNotFound();
                }
                return View(aBB_DealStages);
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
            ABB_DealStages aBB_DealStage = new ABB_DealStages();
            return View(aBB_DealStage);
        }

        // POST: BB_DealStages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                ABB_DealStages aBB_DealStages = await db.ABB_DealStages.FindAsync(id);
                aBB_DealStages.MarkAsDelete = true;
                aBB_DealStages.ModifiedDate = DateTime.Now;
                aBB_DealStages.ModifiedBy = Login.Id;
                db.Entry(aBB_DealStages).State = EntityState.Modified;
                //db.ABB_DealStages.Remove(aBB_DealStages);
                await db.SaveChangesAsync();
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                    this.ControllerContext.RouteData.Values["action"].ToString(),
                    "The user " + Login.FirstName + " " + Login.LastName + "Deleted Deal Stage");
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
            ABB_DealStages aBB_DealStage = new ABB_DealStages();
            return View(aBB_DealStage);
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
