using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FramWork.Models;
using Core;

namespace ForDelete.Controllers
{
    public class SalesAgentBankDetailsController : Controller
    {
        private FrameworkEntities db = new FrameworkEntities();

        //
        // GET: /SalesAgentBankDetails/
        [LoginAuthorize]
        public ActionResult Index()
        {
            var list = db.Commision_AgentBankDetails.Include(c => c.Organizations).Include(c => c.Teams).Include(c => c.Users).Include(c => c.Users1);
            return View(list.ToList());
        }

        //
        // GET: /SalesAgentBankDetails/Details/5
        [LoginAuthorize]
        public ActionResult Details(int id = 0)
        {
            Commision_AgentBankDetails tbl = db.Commision_AgentBankDetails.Find(id);
            if (tbl == null)
            {
                return HttpNotFound();
            }
            return View(tbl);
        }

        //
        // GET: /SalesAgentBankDetails/Create
        [LoginAuthorize]
        public ActionResult Create()
        {
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias");
            ViewBag.TeamId = new SelectList(db.Teams, "Id", "Name");
            ViewBag.SalesAgentId = new SelectList(db.Users, "Id", "Salutation");
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation");
            return View();
        }

        //
        // POST: /SalesAgentBankDetails/Create
        [LoginAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Commision_AgentBankDetails tbl)
        {
            if (ModelState.IsValid)
            {
                tbl.OrgId = Login.OrganizationId;
                tbl.ModifiedBy = Login.Id;
                tbl.CreateDate = tbl.ModifiedDate = DateTime.Now;
                tbl.MarkAsDelete = false;
                db.Commision_AgentBankDetails.Add(tbl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", tbl.OrgId);
            ViewBag.TeamId = new SelectList(db.Teams, "Id", "Name", tbl.TeamId);
            ViewBag.SalesAgentId = new SelectList(db.Users, "Id", "Salutation", tbl.SalesAgentId);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", tbl.ModifiedBy);
            return View(tbl);
        }

        //
        // GET: /SalesAgentBankDetails/Edit/5
        [LoginAuthorize]
        public ActionResult Edit(int id = 0)
        {
            Commision_AgentBankDetails tbl = db.Commision_AgentBankDetails.Find(id);
            if (tbl == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", tbl.OrgId);
            ViewBag.TeamId = new SelectList(db.Teams, "Id", "Name", tbl.TeamId);
            ViewBag.SalesAgentId = new SelectList(db.Users, "Id", "Salutation", tbl.SalesAgentId);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", tbl.ModifiedBy);
            return View(tbl);
        }

        //
        // POST: /SalesAgentBankDetails/Edit/5
        [LoginAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Commision_AgentBankDetails tbl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", tbl.OrgId);
            ViewBag.TeamId = new SelectList(db.Teams, "Id", "Name", tbl.TeamId);
            ViewBag.SalesAgentId = new SelectList(db.Users, "Id", "Salutation", tbl.SalesAgentId);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", tbl.ModifiedBy);
            return View(tbl);
        }

        //
        // GET: /SalesAgentBankDetails/Delete/5
        [LoginAuthorize]
        public ActionResult Delete(int id = 0)
        {
            Commision_AgentBankDetails commision_agentbankdetails = db.Commision_AgentBankDetails.Find(id);
            if (commision_agentbankdetails == null)
            {
                return HttpNotFound();
            }
            return View(commision_agentbankdetails);
        }

        //
        // POST: /SalesAgentBankDetails/Delete/5
        [LoginAuthorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Commision_AgentBankDetails tbl = db.Commision_AgentBankDetails.Find(id);
            db.Commision_AgentBankDetails.Remove(tbl);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}