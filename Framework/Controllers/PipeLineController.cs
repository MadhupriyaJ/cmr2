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
    public class PipeLineController : Controller
    {
        #region Index
        // GET: PipeLine
        [LoginAuthorize]
        public ActionResult Index()
        {
        return View();
        }
        #endregion

        #region getPersonAddview
        [LoginAuthorize]
        public ActionResult getPersonAddview()
        {
        return View();
        }	 
        #endregion
     
        #region AddOppurtunity
        [LoginAuthorize]
        public ActionResult AddOppurtunity()
        {
        return View();
        }	 
        #endregion
    
        #region PipeLineQuestions
        [LoginAuthorize]
        public ActionResult PipeLineQuestions(String personLeadId, String DealStageNameId, String movementMode)
        {
        try{
        TempData["personLeadId"] = personLeadId;
        TempData["DealStageNameId"] = DealStageNameId;
        TempData["movementMode"] = movementMode;
        ViewBag.btnmode = movementMode;
        HelloServiceClient client = new FramWork.HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
        var questions = client.GetListOfDealStageQuestions(Login.OrganizationId, new Guid(DealStageNameId), movementMode);
        List<Pipelineq> q = new List<Pipelineq>();
        foreach (var item in questions)
        {
        q.Add(new Pipelineq
        {
        Id = item.Id.ToString(),
        Title = item.Title,
        Questions = item.Question,
        Controltype = item.HtmlControlName.Name,
        ControlName = item.ControlName,
        ControlId = item.ControlId,
        PersonLeadId = personLeadId,
        DealStageNameId = DealStageNameId,
        questType = (EnumQuestionType)Enum.Parse(typeof(EnumQuestionType), item.HtmlControlName.enumnameId.ToString())
        });
        }
        return View(q);
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
        Pipelineq obj=new Pipelineq();
        return View(obj);
        }
	 
        #endregion

        #region Move_Next_Stage
        [LoginAuthorize]
        public ActionResult Move_Next_Stage(ICollection<DealStageAns> vls)
        {
        try
        {
        Guid dealstageid = new Guid();
        Guid PersonLeadId = new Guid();
        PersonLeadId = new Guid(TempData["personLeadId"].ToString());
        dealstageid = new Guid(TempData["DealStageNameId"].ToString());
        string movementMode = TempData["movementMode"].ToString();
        if (movementMode == "won" || movementMode == "lost" || movementMode == "dropped" || movementMode == "revertlead")
        {
        HelloServiceClient client = new FramWork.HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
        if (vls != null)
        {
            foreach (var item in vls)
            {
                DealStageAnswer tbl = new DealStageAnswer();
                tbl.OrgId = Login.OrganizationId;
                tbl.QuestionId = new Guid(item.QuestionId);
                PersonLeadId = tbl.PersonId = new Guid(item.PersonId);
                tbl.Ans = item.Answer;
                tbl.ModifiedBy = Login.Id;
                dealstageid = new Guid(item.DealStageId);
                client.AddDYnamicQuestions_answer(tbl);
            }
        }
        bool Success = client.won_lost_dropped_Revert(Login.OrganizationId, dealstageid, PersonLeadId, movementMode);
        if (Success)
        {
            return RedirectToAction("Index", "PipeLine");
        }
        }
        else if (movementMode == "backward")
        {
        HelloServiceClient client = new FramWork.HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
        if (vls != null)
        {
            foreach (var item in vls)
            {
                DealStageAnswer tbl = new DealStageAnswer();
                tbl.OrgId = Login.OrganizationId;
                tbl.QuestionId = new Guid(item.QuestionId);
                PersonLeadId = tbl.PersonId = new Guid(item.PersonId);
                tbl.Ans = item.Answer;
                tbl.ModifiedBy = Login.Id;
                dealstageid = new Guid(item.DealStageId);
                client.AddDYnamicQuestions_answer(tbl);
            }
        }
        bool Success = client.MovetoPreviousDealStage(Login.OrganizationId, dealstageid, PersonLeadId);
        if (Success)
        {
            return RedirectToAction("Index", "PipeLine");
        }
        }
        else
        {
        HelloServiceClient client = new FramWork.HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
        if (vls != null)
        {
            foreach (var item in vls)
            {
                DealStageAnswer tbl = new DealStageAnswer();
                tbl.OrgId = Login.OrganizationId;
                tbl.QuestionId = new Guid(item.QuestionId);
                PersonLeadId = tbl.PersonId = new Guid(item.PersonId);
                tbl.Ans = item.Answer;
                tbl.ModifiedBy = Login.Id;
                dealstageid = new Guid(item.DealStageId);
                client.AddDYnamicQuestions_answer(tbl);
            }
        }
        bool Success = client.MovetoNextDealStage(Login.OrganizationId, dealstageid, PersonLeadId);
        if (Success)
        {
            return RedirectToAction("Index", "PipeLine");
        }
        }
        return RedirectToAction("Index", "PipeLine");
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
        return RedirectToAction("Index", "PipeLine");
        }
        #endregion

        #region Dynamic crm
        [LoginAuthorize]
        public ActionResult SalesActivity()
        {
        return View();
        }

        #endregion


    }

    public class Pipelineq
    {

        public String Id { get; set; }
        public String Title { get; set; }
        public String Questions { get; set; }
        public String Controltype { get; set; }
        public String ControlName { get; set; }
        public String ControlId { get; set; }
        public EnumQuestionType questType { get; set; }
        public String PersonLeadId { get; set; }
        public String DealStageNameId { get; set; }
    }

    public enum EnumQuestionType
    {
        None = 0,
        Select = 3,
        Text = 1,
        TextArea = 4
    }

    public class DealStageAns
    {
        public string OrgId { get; set; }
        public string QuestionId { get; set; }
        public String PersonId { get; set; }
        public String DealStageId { get; set; }
        public String Answer { get; set; }
        public String stageMovingMode { get; set; }
    }
}