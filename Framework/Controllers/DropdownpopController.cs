using Core;
using FramWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FramWork.Controllers
{
    public class DropdownpopController : Controller
    {
        #region LoadCompanyAccount
        public JsonResult LoadCompanyAccount(String CompanyId)
        {
            try
            {
                if (!String.IsNullOrEmpty(CompanyId))
                {
                    FrameworkEntities dc = new FrameworkEntities();
                    var list = dc.ABB_CompanyAccount.AsEnumerable().Where(a => a.OrgId == Login.OrganizationId && a.MarkAsDelete == false
                        && a.CompanyId == Convert.ToInt64(CompanyId)).Select(a => new { Id = a.Id, Name = a.AccountNumber });
                    return Json(list, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        #endregion  
       
        #region Loadstaff
        public JsonResult Loadstaff(String CompanyId)
        {
            try
            {
                if (!String.IsNullOrEmpty(CompanyId))
                {
                    FrameworkEntities dc = new FrameworkEntities();
                    var list = dc.ABB_CompanyManagmentTeam_Staff.AsEnumerable().Where(a => a.OrgId == Login.OrganizationId && a.MarkAsDelete == false
                        && a.CompanyId == Convert.ToInt64(CompanyId)).Select(a => new { Id = a.Id, Name = a.FIrstName +" "+ a.LastName });
                    return Json(list, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        #endregion  
    

    }
}