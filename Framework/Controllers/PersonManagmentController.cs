using Core;
using ExceptionLogging;
using FramWork.HelloService;
using FramWork.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace FramWork.Controllers
{
    public class PersonManagmentController : Controller
    {
        #region Index
        [LoginAuthorize]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                ViewBag.PersonSpecLst = null;
                HelloServiceClient client = new FramWork.HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                List<Person> PersonLst = client.GetLstPerson(Login.OrganizationId).ToList();
                ViewBag.PersonLst = PersonLst;
                if (Session["PersonId"] != null)
                {
                    Guid PersonId = new Guid(Session["PersonId"].ToString());
                    Person PersonSpecLst = PersonLst.Where(s => s.Id == PersonId).SingleOrDefault();
                    ViewBag.PersonSpecLst = PersonSpecLst;
                }

                else
                {
                    Person PersonSpecLst = PersonLst.FirstOrDefault();
                    ViewBag.PersonSpecLst = PersonSpecLst;
                }

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
            return View();
        }
        #endregion

        #region GetPersonSpecific
        [LoginAuthorize]

        public bool GetPersonSpecific(string ID)
        {
            try
            {
                Guid PersonId = new Guid(ID);
                // HelloServiceClient client = new FramWork.HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                //var PersonLst = client.GetLstPerson(Login.OrganizationId);
                //var PersonSpecLst = PersonLst.Where(s => s.Id == PersonId).SingleOrDefault();
                //ViewBag.PersonLst = client.GetLstPerson(Login.OrganizationId);
                //TempData["PersonSpecLst"] = PersonSpecLst;
                HelloServiceClient client = new FramWork.HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                List<Person> PersonLst = client.GetLstPerson(Login.OrganizationId).ToList();
                ViewBag.PersonLst = PersonLst;
                Session["PersonId"] = ID;
                return true;
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
            return false;

        }
        #endregion

        #region Make Lead
        [LoginAuthorize]
        public ActionResult MakeLead(string ID)
        {
            Person objperson = new Person();
            try
            {
                HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                if (ID == null)
                {
                    objperson = client.GetLstPerson(Login.OrganizationId).FirstOrDefault();

                }
                else { objperson = client.GetOnePerson(Login.OrganizationId, new Guid(ID)); }

                return View(objperson);
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
            return View(objperson);
        }
        #endregion

        #region GetProductList
        [LoginAuthorize]
        public JsonResult GetProductList(string CampaignId)
        {
            try
            {
                Guid Id = new Guid(CampaignId);
                HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                List<CampaignProduct> CampaignProdcutLst = client.GetCampaignProductLst(Id, Login.OrganizationId).ToList();
                return Json(new { SUCCESS = true, CampaignProdcutLst = CampaignProdcutLst });
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
            return Json(null, JsonRequestBehavior.AllowGet);

        }
        #endregion   

        #region Service  called Assign Lead
        [LoginAuthorize]
        public bool AssignAsLead(string personId, string Salary, string CurrentLiablility,
            String MonthlyInstallment, String ExpectedValue, String SanctionedValue, string ActualValue, string ExpectedClosingDate, string Assignedto)
        {
            try
            {
                FrameworkEntities db = new FrameworkEntities();
                var stageone = db.DealStageNames.OrderBy(a => a.SortOrder).Where(a => a.Isvisible == true).FirstOrDefault();
                FramWork.Models.PersonLeads pl = new FramWork.Models.PersonLeads();
                pl.Id = Guid.NewGuid();
                pl.PersonId = new Guid(personId);
                pl.OrgId = Login.OrganizationId;
                if (stageone != null) { pl.DealStageNameId = stageone.Id; }
                if (!String.IsNullOrEmpty(Salary)) { pl.Salary = decimal.Parse(Regex.Replace(Convert.ToString(Salary), @"[^\d.]", "")); }
                if (!String.IsNullOrEmpty(CurrentLiablility)) { pl.CurrentLiablility = decimal.Parse(Regex.Replace(Convert.ToString(CurrentLiablility), @"[^\d.]", "")); }
                if (!String.IsNullOrEmpty(MonthlyInstallment)) { pl.MonthlyInstallment = decimal.Parse(Regex.Replace(Convert.ToString(MonthlyInstallment), @"[^\d.]", "")); }
                if (!String.IsNullOrEmpty(ExpectedValue)) { pl.ExpectedValue = decimal.Parse(Regex.Replace(Convert.ToString(ExpectedValue), @"[^\d.]", "")); }
                if (!String.IsNullOrEmpty(SanctionedValue)) { pl.SanctionedValue = decimal.Parse(Regex.Replace(Convert.ToString(SanctionedValue), @"[^\d.]", "")); }
                if (!String.IsNullOrEmpty(ActualValue)) { pl.ActualValue = decimal.Parse(Regex.Replace(Convert.ToString(ActualValue), @"[^\d.]", "")); }
                if (!String.IsNullOrEmpty(ExpectedClosingDate)) { pl.ExpectedClosingDate = Convert.ToDateTime(ExpectedClosingDate); }
                //pl.ActualValue = decimal.Parse(Regex.Replace(Convert.ToString(PersonLeadTbl.ActualValue), @"[^\d.]", ""));
                if (!String.IsNullOrEmpty(Assignedto)) { pl.Assignedto = new Guid(Assignedto); }
                pl.CreateDate = pl.ModifiedDate = DateTime.Now;
                pl.ModifiedBy = Login.Id;
                pl.MarkAsDelete = false;
                db.PersonLeads.Add(pl);
                    db.SaveChanges();
                    return true;
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
            return false;
        }
        #endregion


    }
}