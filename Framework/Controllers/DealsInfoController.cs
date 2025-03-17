using FramWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using Core;

namespace FramWork.Controllers
{
    public class DealsInfoController : Controller
    {
        #region DealsWon
        public ActionResult DealsWon(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                FrameworkEntities db = new FrameworkEntities();
                var userassigned = db.Employees.Where(a => a.UserId == Login.Id).Select(a=>a.Id).FirstOrDefault();
                var list = db.PersonLeads.AsEnumerable()
                              .Where(a => 
                                  a.OrgId == Login.OrganizationId
                                  && a.Assignedto == userassigned
                                  && a.MarkAsDelete == false
                                  && a.DealStageNameId == new Guid("8fc13a30-255d-41dd-8385-ded2987cb93c"));
                int pageSize = 20;
                int pageNumber = (page ?? 1);
                return View(list.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region DealsLost
        public ActionResult DealsLost(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                FrameworkEntities db = new FrameworkEntities();
                var userassigned = db.Employees.Where(a => a.UserId == Login.Id).Select(a => a.Id).FirstOrDefault();
                var list = db.PersonLeads.AsEnumerable()
                              .Where(a =>
                                  a.OrgId == Login.OrganizationId
                                  && a.Assignedto == userassigned
                                  && a.MarkAsDelete == false
                                  && a.DealStageNameId == new Guid("b4a42d7c-0cac-42cd-a464-ef1a504cb5b8"));
                int pageSize = 20;
                int pageNumber = (page ?? 1);
                return View(list.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
        #region DealsDropped
        public ActionResult DealsDropped(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                FrameworkEntities db = new FrameworkEntities();
                var userassigned = db.Employees.Where(a => a.UserId == Login.Id).Select(a => a.Id).FirstOrDefault();
                var list = db.PersonLeads.AsEnumerable()
                              .Where(a =>
                                  a.OrgId == Login.OrganizationId
                                  && a.Assignedto == userassigned
                                  && a.MarkAsDelete == false
                                  && a.DealStageNameId == new Guid("1adbe8a1-5b00-4a01-830a-bad79927707e"));
                int pageSize = 20;
                int pageNumber = (page ?? 1);
                return View(list.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
        #region Deals
        public ActionResult DealsOngoing(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                FrameworkEntities db = new FrameworkEntities();
                var userassigned = db.Employees.Where(a => a.UserId == Login.Id).Select(a => a.Id).FirstOrDefault();
                var list = db.PersonLeads.AsEnumerable()
                              .Where(a =>
                                  a.OrgId == Login.OrganizationId
                                  && a.Assignedto == userassigned
                                  && a.MarkAsDelete == false
                                  && a.DealStageNameId != new Guid("b4a42d7c-0cac-42cd-a464-ef1a504cb5b8")
                                  && a.DealStageNameId != new Guid("8fc13a30-255d-41dd-8385-ded2987cb93c")
                                  && a.DealStageNameId != new Guid("1adbe8a1-5b00-4a01-830a-bad79927707e"));
                int pageSize = 20;
                int pageNumber = (page ?? 1);
                return View(list.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}