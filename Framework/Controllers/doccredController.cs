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
    public class doccredController : Controller
    {
        private FrameworkEntities db = new FrameworkEntities();

        //
        // GET: /doccred/

        public ActionResult Index()
        {
            var documents = db.Documents.Include(d => d.Organizations).Include(d => d.Users);
            return View(documents.ToList());
        }

        //
        // GET: /doccred/Details/5

        public ActionResult Details(Guid? id)
        {
            Documents documents = db.Documents.Find(id);
            if (documents == null)
            {
                return HttpNotFound();
            }
            return View(documents);
        }

        //
        // GET: /doccred/Create

        public ActionResult Create()
        {
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias");
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation");
            return View();
        }

        //
        // POST: /doccred/Create
        [LoginAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Documents tbl)
        {
            if (ModelState.IsValid)
            {
                tbl.Id = Guid.NewGuid();
                tbl.OrgId = Login.OrganizationId;
                tbl.ModifiedBy = Login.Id;
                tbl.CreateDate = tbl.ModifiedDate = DateTime.Now;
                tbl.MarkAsDelete = false;
                tbl.Id = Guid.NewGuid();
                db.Documents.Add(tbl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", tbl.OrgId);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", tbl.ModifiedBy);
            return View(tbl);
        }

        //
        // GET: /doccred/Edit/5

        public ActionResult Edit(Guid? id)
        {
            Documents documents = db.Documents.Find(id);
            if (documents == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", documents.OrgId);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", documents.ModifiedBy);
            return View(documents);
        }

        //
        // POST: /doccred/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Documents documents)
        {
            if (ModelState.IsValid)
            {
                db.Entry(documents).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", documents.OrgId);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", documents.ModifiedBy);
            return View(documents);
        }

        //
        // GET: /doccred/Delete/5

        public ActionResult Delete(Guid? id)
        {
            Documents documents = db.Documents.Find(id);
            if (documents == null)
            {
                return HttpNotFound();
            }
            return View(documents);
        }

        //
        // POST: /doccred/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Documents documents = db.Documents.Find(id);
            db.Documents.Remove(documents);
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