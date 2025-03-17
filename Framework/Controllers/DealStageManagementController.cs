using Core;
using ExceptionLogging;
using FramWork.HelloService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FramWork.Controllers
{
    public class DealStageManagementController : Controller
    {
        #region Pipeline
        [LoginAuthorize]
        public ActionResult Pipeline()
        {
            try
            {
                List<PipeLine> pipelinenames = new List<PipeLine>();
                HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                pipelinenames = client.GetPipeLineLst(Login.OrganizationId).ToList();
                ViewBag.Pipelines = pipelinenames;
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                  this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into List of PipeLine Page");
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
            ViewBag.Pipelines = new List<PipeLine>().ToList();
            return View();
        }
        #endregion

        #region Add PipeLine
        [LoginAuthorize]
        public bool AddPipeLine(PipeLine PiplineTbl)
        {
            try
            {
                if (!String.IsNullOrEmpty(PiplineTbl.Name))
                {
                    PiplineTbl.Id = Guid.NewGuid();
                    PiplineTbl.ModifiedBy = Login.Id;
                    PiplineTbl.OrgId = Login.OrganizationId;
                    HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                    client.AddPipeLine(PiplineTbl);
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                    this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Has Added PipeLine.");
                    return true;
                }
                else
                {
                    return false;
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
            return false;
        }
        #endregion

        #region Deal Stage
        [LoginAuthorize]
        public ActionResult DealStage(string ID)
        {
            try
            {
                List<DealStageName> DealStageObj = new List<DealStageName>();
                HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                DealStageObj = client.GetDealStageLst(Login.OrganizationId, new Guid(ID)).ToList();
                ViewBag.DealStageObj = DealStageObj;
                ViewBag.Id = ID;
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                  this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into List of Deal Stage Page");
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
            ViewBag.DealStageObj = new List<DealStageName>().ToList();
            ViewBag.Id = ID;
            return View();
        }
        #endregion

        #region Add DealStageName

        [LoginAuthorize]
        public bool AddDealStageName(DealStageName DSName)
        {
            string status = "";
            try
            {
                HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                if (!String.IsNullOrEmpty(DSName.Name))
                {
                    DSName.Id = Guid.NewGuid();
                    DSName.ModifiedBy = Login.Id;
                    DSName.OrgId = Login.OrganizationId;

                    status = client.AddDealStageName(DSName);

                }
                if (status == "true")
                {
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                    this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Added Deal Stage Name.");
                    return true;
                }

                else
                {
                    return false;
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
            return false;
        }
        #endregion

        #region DealStageQuestions
        [LoginAuthorize]
        public ActionResult DealStageQuestions(string DealStageId)
        {
            try
            {
                HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                List<DealStageQuestion> DealStageObj = client.GetDSQuesLst(Login.OrganizationId, new Guid(DealStageId)).ToList();
                ViewBag.DealStageObj = DealStageObj;
                ViewBag.Id = DealStageId;
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into  Deal Stage Questions List Page");
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
            ViewBag.DealStageObj = new List<DealStageName>().ToList();
            ViewBag.Id = DealStageId;
            return View();
        }
        #endregion

        #region AddDSQuestionValues
        [LoginAuthorize]
        public bool AddDSQuestionValues(DealStageQuestionValue DSQuestionValuetbl)
        {
            try
            {
                if (!String.IsNullOrEmpty(DSQuestionValuetbl.Text))
                {
                    DSQuestionValuetbl.Id = Guid.NewGuid();
                    DSQuestionValuetbl.ModifiedBy = Login.Id;
                    DSQuestionValuetbl.OrgId = Login.OrganizationId;
                    HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                    client.AddDSQuestionValues(DSQuestionValuetbl);
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                     this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Added Deal Stage Questions");
                    return true;
                }
                else
                {
                    return false;
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
            return false;
        }
        #endregion

        //DealStageQuestionValues

        [LoginAuthorize]
        public JsonResult AddDealStageQuestions(DealStageQuestion DSQuestiontbl)
        {
            try
            {

                DSQuestiontbl.Id = Guid.NewGuid();
                DSQuestiontbl.ModifiedBy = Login.Id;
                DSQuestiontbl.OrgId = Login.OrganizationId;
                HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                Guid DSQuestionId = client.AddDealStageQuestions(DSQuestiontbl);
                return Json(new { SUCCESS = true, ID = DSQuestionId });
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
            return Json(null);

        }

        #region MyRegion
        //public ActionResult Pipeline()
        //{
        //    List<PipeLine> pipelinenames = new List<PipeLine>();
        //    HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
        //    pipelinenames = client.GetPipeLineLst(Login.OrganizationId).ToList();
        //    ViewBag.Pipelines = JsonConvert.SerializeObject(pipelinenames);
        //    return View();
        //}


        //public bool AddPipeLine(PipeLine PiplineTbl)
        //{
        //    try
        //    {
        //        if (!String.IsNullOrEmpty(PiplineTbl.Name))
        //        {
        //            PiplineTbl.Id = Guid.NewGuid();
        //            PiplineTbl.ModifiedBy = Login.Id;
        //            PiplineTbl.OrgId = Login.OrganizationId;
        //            HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
        //            client.AddPipeLine(PiplineTbl);
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //        throw;

        //    }



        //}





        //[LoginAuthorize]
        //public ActionResult CreateDealStageName()
        //{
        //    return View();
        //}
        //[LoginAuthorize]
        //public bool AddDealStageName(DealStageName DSName, string PipelineName)
        //{
        //    string status = "";
        //    try
        //    {
        //        HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
        //        if (!String.IsNullOrEmpty(DSName.Name))
        //        {
        //            DSName.Id = Guid.NewGuid();
        //            PipeLine Objpipeline = client.GetPipeLineLst(Login.OrganizationId).Where(x => x.Name == PipelineName).FirstOrDefault();
        //            DSName.PipeLineId = Objpipeline.Id;
        //            DSName.ModifiedBy = Login.Id;
        //            DSName.OrgId = Login.OrganizationId;

        //            status = client.AddDealStageName(DSName);

        //        }
        //        if (status == "true")
        //        {

        //            return true;
        //        }

        //        else
        //        {
        //            return false;
        //        }


        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //        return false;
        //    }

        //}

        //public JsonResult GetDealsStageLst(string PipelineName)
        //{
        //    List<DealStageName> objDS = new List<DealStageName>();
        //    HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
        //    objDS = client.GetDealStageLst(Login.OrganizationId, PipelineName).ToList();
        //    return Json(new { objDS });

        //}
        #endregion


    }
}