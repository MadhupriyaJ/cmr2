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
    public class BB_TypeOfBusinessController : Controller
    {
        private FrameworkEntities db = new FrameworkEntities();

        #region Index
        // GET: BB_TypeOfBusiness
        [LoginAuthorize]
        public async Task<ActionResult> Index()
        {
            try
            {
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(),
            "The user " + Login.FirstName + " " + Login.LastName + "Entered into Business Type Page");
                return View(await db.ABB_TypeOfBusiness.ToListAsync());
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
            var aBB_TypeOfBusiness = new List<ABB_TypeOfBusiness>();
            return View(aBB_TypeOfBusiness.ToList());

        }
        #endregion
       
        #region Details
        // GET: BB_TypeOfBusiness/Details/5
        [LoginAuthorize]
        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ABB_TypeOfBusiness aBB_TypeOfBusiness = await db.ABB_TypeOfBusiness.FindAsync(id);
                if (aBB_TypeOfBusiness == null)
                {
                    return HttpNotFound();
                }
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(),
            "The user " + Login.FirstName + " " + Login.LastName + "Entered into Business Type Details Page");
                return View(aBB_TypeOfBusiness);
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
            ABB_TypeOfBusiness objABB_TypeOfBusiness = new ABB_TypeOfBusiness();
            return View(objABB_TypeOfBusiness);

        }

        #endregion
      
        #region Create
        // GET: BB_TypeOfBusiness/Create
        [LoginAuthorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: BB_TypeOfBusiness/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public async Task<ActionResult> Create([Bind(Include = "Id,TypeOfBusiness,Description,CreateDate,ModifiedDate,ModifiedBy,MarkAsDelete")] ABB_TypeOfBusiness aBB_TypeOfBusiness)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    aBB_TypeOfBusiness.OrgId = Login.OrganizationId;
                    aBB_TypeOfBusiness.CreateDate = aBB_TypeOfBusiness.ModifiedDate = DateTime.Now;
                    aBB_TypeOfBusiness.ModifiedBy = Login.Id;
                    aBB_TypeOfBusiness.MarkAsDelete = false;
                    db.ABB_TypeOfBusiness.Add(aBB_TypeOfBusiness);
                    await db.SaveChangesAsync();
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(),
            "The user " + Login.FirstName + " " + Login.LastName + "Created new Business Type");
                    return RedirectToAction("Index");
                }

                return View(aBB_TypeOfBusiness);
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
            return View(aBB_TypeOfBusiness);
        }

        #endregion
       
        #region Edit
        // GET: BB_TypeOfBusiness/Edit/5
        [LoginAuthorize]
        public async Task<ActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ABB_TypeOfBusiness aBB_TypeOfBusiness = await db.ABB_TypeOfBusiness.FindAsync(id);
                if (aBB_TypeOfBusiness == null)
                {
                    return HttpNotFound();
                }
                return View(aBB_TypeOfBusiness);
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
            ABB_TypeOfBusiness objABB_TypeOfBusiness = new ABB_TypeOfBusiness();
            return View(objABB_TypeOfBusiness);
        }

        // POST: BB_TypeOfBusiness/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public async Task<ActionResult> Edit([Bind(Include = "Id,TypeOfBusiness,Description,CreateDate,ModifiedDate,ModifiedBy,MarkAsDelete")] ABB_TypeOfBusiness aBB_TypeOfBusiness)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(aBB_TypeOfBusiness).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                    this.ControllerContext.RouteData.Values["action"].ToString(),
                    "The user " + Login.FirstName + " " + Login.LastName + "Edited Business Type Page");
                    return RedirectToAction("Index");
                }
                return View(aBB_TypeOfBusiness);
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
            return View(aBB_TypeOfBusiness);
        }

        #endregion
       
        #region Delete
        // GET: BB_TypeOfBusiness/Delete/5
        [LoginAuthorize]
        public async Task<ActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ABB_TypeOfBusiness aBB_TypeOfBusiness = await db.ABB_TypeOfBusiness.FindAsync(id);
                if (aBB_TypeOfBusiness == null)
                {
                    return HttpNotFound();
                }
                return View(aBB_TypeOfBusiness);
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
            ABB_TypeOfBusiness objaBB_TypeOfBusiness = new ABB_TypeOfBusiness();
            return View(objaBB_TypeOfBusiness);

        }

        // POST: BB_TypeOfBusiness/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                ABB_TypeOfBusiness aBB_TypeOfBusiness = await db.ABB_TypeOfBusiness.FindAsync(id);
                aBB_TypeOfBusiness.MarkAsDelete = true;
                aBB_TypeOfBusiness.ModifiedDate = DateTime.Now;
                aBB_TypeOfBusiness.ModifiedBy = Login.Id;
                db.Entry(aBB_TypeOfBusiness).State = EntityState.Modified;
                await db.SaveChangesAsync();
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                   this.ControllerContext.RouteData.Values["action"].ToString(),
                   "The user " + Login.FirstName + " " + Login.LastName + "Deleted Business Type Page");
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
            ABB_TypeOfBusiness objABB_TypeOfBusiness = new ABB_TypeOfBusiness();
            return View(objABB_TypeOfBusiness);
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
