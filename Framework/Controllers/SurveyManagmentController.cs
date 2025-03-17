using Core;
using ExceptionLogging;
using FramWork.HelloService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FramWork.Controllers
{
    public class SurveyManagmentController : Controller
    {
        #region Index
        // GET: SurveyManagment
        [LoginAuthorize]
        public ActionResult Index()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
  this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Survey Page");
            return View();
        }
        #endregion     

        #region Survey Title
        [LoginAuthorize]
        public String AddSurveyMaster(String SurveyTitle)
        {
            try
            {
                if (!String.IsNullOrEmpty(SurveyTitle))
                {
                    HelloServiceClient client = new HelloServiceClient("BasicHttpBinding_IHelloService");
                    SurveyMaster tbl = new SurveyMaster();
                    tbl.Id = Guid.NewGuid();
                    tbl.OrgId = Login.OrganizationId;
                    tbl.Name = SurveyTitle;
                    tbl.ModifiedBy = Login.Id;
                    bool Success = client.AddsurveyMaster(tbl);
                    if (Success)
                    {
                        Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                    this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Added Survey.");
                        return tbl.Id.ToString();
                    }
                }
                return "";
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
            return "";
        }
        #endregion
        
        #region New Survey
        [LoginAuthorize]
        public ActionResult createsurvey(Guid? NestId)
        {
            try
            {
                if (NestId != null)
                {
                    ViewBag.NestId = NestId;
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                    this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Create Survey Questions Page");
                    return View();
                }
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

        #region Radio Button
        [LoginAuthorize]
        public ActionResult surveyradio()
        {
            return View();
        }
        #endregion
    }
}