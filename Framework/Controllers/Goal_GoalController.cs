using Core;
using ExceptionLogging;
using FramWork.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FramWork.Controllers
{
    public class Goal_GoalController : Controller
    {
        private FrameworkEntities db = new FrameworkEntities();

        #region Goals
        #region Goal List
        [LoginAuthorize]
        public ActionResult Index()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into List of Goals Page.");
            return View(db.TGoal_Goal.Where(a => a.OrgId == Login.OrganizationId && a.MarkAsDelete == false).ToList());
        }
        #endregion

        #region Create Goal
        [LoginAuthorize]
        public ActionResult Create()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Add New Goal Page.");
            return View();
        }

        [LoginAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TGoal_Goal tbl)
        {
            if (ModelState.IsValid)
            {
                tbl.OrgId = Login.OrganizationId;
                tbl.CreateDate = tbl.ModifiedDate = DateTime.Now;
                tbl.ModifiedBy = Login.Id;
                tbl.MarkAsDelete = false;
                db.TGoal_Goal.Add(tbl);
                await db.SaveChangesAsync();
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Added New Goal.");
                return RedirectToAction("Index");
            }

            return View(tbl);
        }
        #endregion

        #region Details
        [LoginAuthorize]
        public ActionResult Details(int id)
        {
            return View();
        }
        #endregion

        #region Edit
        [LoginAuthorize]
        public ActionResult Edit(int id)
        {
            return View();
        }

        [LoginAuthorize]
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion

                return View();
            }
        }
        #endregion

        #region Delete
        // GET: Goal_Goal/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Goal_Goal/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

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

                return View();
            }
        }
        #endregion

        #region Goal Details
        [LoginAuthorize]
        public ActionResult GoalDetail()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                        this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Goal Detail Page.");
            return View();
        }
        #endregion
        #endregion
    }
}
