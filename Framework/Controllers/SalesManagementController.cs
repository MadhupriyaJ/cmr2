using Core;
using FramWork.HelloService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.IO;
using ExceptionLogging;

namespace FramWork.Controllers
{
    public class SalesManagementController : Controller
    {

        HelloServiceClient client = new FramWork.HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");

        #region Sales Pipeline
        [LoginAuthorize]
        public ActionResult SalesAgentView()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "entered into Sales Agent View Page.");
            return View();
        }

        #region   List Of  PersonLeads
        [LoginAuthorize]
        public ActionResult ListOfPersonLead(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {  
                List<PersonLead> list = client.GetPersonLeadLst(Login.OrganizationId).ToList();
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;
                int pageSize = 3;
                int pageNumber = (page ?? 1);
                return View(list.ToPagedList(pageNumber, pageSize));
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
            int pageNumberone = (page ?? 1);
            List<PersonLead> lst = new List<PersonLead>();
            return View(lst.ToPagedList(pageNumberone, 10));


        }
        #endregion

        #region   List Of  Person(Contact or Oppurtunity)
        [LoginAuthorize]
        public ActionResult ListOfPerson(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                List<Person> list = client.GetLstPerson(Login.OrganizationId).ToList();
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;
                int pageSize = 3;
                int pageNumber = (page ?? 1);
                return View(list.ToPagedList(pageNumber, pageSize));
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
            int pageNumberone = (page ?? 1);
            List<Person> lst = new List<Person>();
            return View(lst.ToPagedList(pageNumberone, 10));


        }
        #endregion

        #endregion

        #region Index
        [LoginAuthorize]
        public ActionResult Index()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
              this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "entered into Sales Management Index Page.");
            return View();
        }
        #endregion

        #region SalesPerformance
        [LoginAuthorize]
        public ActionResult SalesPerformance()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
              this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Sales Performance Page.");
            return View();
        }
        #endregion

        #region CustomerService Agent
        [LoginAuthorize]
        public ActionResult CustomerService()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
               this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Customer Service Agent Page.");
            return View();
        }
        #endregion

        #region CustomerServiceManager
        [LoginAuthorize]
        public ActionResult CustomerServiceManager()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
              this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Customer Service Manager Page.");
            return View();
        }
        #endregion

        #region OpenOppurtunities
        [LoginAuthorize]
        public ActionResult OpenOppurtunities()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
              this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Open Oppurtunities Page.");
            return View();
        }
        #endregion

        #region OpenOpputunties Tabledata
        [LoginAuthorize]
        public string TableData()
        {

            try
            {
                string search = Request.QueryString["search[value]"];
                string page = Request.QueryString["start"];
                string displaylimit = Request.QueryString["length"];
                string order = Request.QueryString["order[0][column]"];
                string orderBy = Request.QueryString["order[0][dir]"];
                int totalData = 0;
                HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                //var LstOppurtunity= client.GetListOfOpenOppurtunities(Login.OrganizationId, '4CD3CFB9-66DC-47DC-908D-BC12A01FF6A6', 'C5C1338D-513B-42A4-A380-DAB78EABC507');
                var LstOppurtunity = client.GetListOfOpenOppurtunities(Login.OrganizationId);
                totalData = LstOppurtunity.Count();
                if (!string.IsNullOrEmpty(search))
                    //LstOppurtunity = LstOppurtunity.Where(a => a.CountyName.Contains(search)).ToArray();
                    if (!string.IsNullOrEmpty(order))
                    {
                        switch (order)
                        {
                            case "1":
                                LstOppurtunity = (orderBy == "asc" ? LstOppurtunity.OrderBy(a => a.Salary) : LstOppurtunity.OrderByDescending(a => a.Salary)).ToArray();
                                break;
                            case "2":
                                LstOppurtunity = (orderBy == "asc" ? LstOppurtunity.OrderBy(a => a.CurrentLiablility) : LstOppurtunity.OrderByDescending(a => a.CurrentLiablility)).ToArray();
                                break;
                            case "3":
                                LstOppurtunity = (orderBy == "asc" ? LstOppurtunity.OrderBy(a => a.MonthlyInstallment) : LstOppurtunity.OrderByDescending(a => a.MonthlyInstallment)).ToArray();
                                break;
                            case "4":
                                LstOppurtunity = (orderBy == "asc" ? LstOppurtunity.OrderBy(a => a.ExpectedValue) : LstOppurtunity.OrderByDescending(a => a.ExpectedValue)).ToArray();
                                break;
                            default:
                                LstOppurtunity = (orderBy == "asc" ? LstOppurtunity.OrderBy(a => a.Person) : LstOppurtunity.OrderByDescending(a => a.Person)).ToArray();
                                break;
                        }
                    }
                    else
                        LstOppurtunity = LstOppurtunity = (orderBy == "asc" ? LstOppurtunity.OrderBy(a => a.Id) : LstOppurtunity.OrderByDescending(a => a.Id)).ToArray();
                if (!String.IsNullOrEmpty(page) && !string.IsNullOrEmpty(displaylimit) && displaylimit != "-1")
                {
                    try
                    {
                        LstOppurtunity = LstOppurtunity.Skip(Convert.ToInt32(!String.IsNullOrEmpty(page) ? page : "0")).Take(Convert.ToInt32(displaylimit)).ToArray();
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
                }


                string aa = "{";
                aa += "\"recordsTotal\": " + totalData + ", \"recordsFiltered\": " + LstOppurtunity.Count() + ",\"data\":[";
                int temp = 0;
                foreach (var item in LstOppurtunity)
                {
                    if (temp > 0)
                        aa += ",";
                    temp++;
                    aa += string.Format("[\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\"]",
                        item.Id, item.Person.FirstName, item.Salary, item.CurrentLiablility, item.MonthlyInstallment, item.MonthlyInstallment);
                }
                return aa + "]}";
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

        #region GetDataAssets for chart
        [LoginAuthorize]
        public JsonResult GetDataAssets()
        {
            try
            {
                HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                var LstOppurtunity = client.GetListOfOpenOppurtunities(Login.OrganizationId);
                List<ClassForChart> Chartdata = new List<ClassForChart>();
                //Chartdata.Add(new object[] { "Persons", "Expected Value", "Sanctioned Value", "Actual Value" });
                foreach (var item in LstOppurtunity)
                {
                    // Chartdata.Add(new object[] { item.Person.FirstName, item.ExpectedValue, item.SanctionedValue, item.ActualValue });

                    Chartdata.Add(new ClassForChart
                    {
                        FirstName = item.Person.FirstName,
                        ExpectedValue = Convert.ToDecimal(item.ExpectedValue),
                        SanctionedValue = Convert.ToDecimal(item.SanctionedValue),
                        ActualValue = Convert.ToDecimal(item.ActualValue)
                    });
                }
                return Json(Chartdata, JsonRequestBehavior.AllowGet);
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
            return Json(null,JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Person Detail Page
        [LoginAuthorize]

        public ActionResult ManagePerson(string id)
        {

            try
            {
                Person PersonDetail = client.GetOnePerson(Login.OrganizationId, new Guid(id));
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
             this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Person Detail Page.");
                return View(PersonDetail);
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
            Person obj = new Person();
            return View(obj);
        }
        #endregion

        #region Lead Stages
        [LoginAuthorize]
        public ActionResult LeadStage(string id)
        {
            try
            {
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
             this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Lead Stage Page.");
                PersonLead PersonLeadDetail = client.GetOnePersonLead(Login.OrganizationId, new Guid(id));
                return View(PersonLeadDetail);
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
            PersonLead obj = new PersonLead();
            return View(obj);

        }
        #endregion

        #region Create a new Person/Contact [checking By Serin]
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult CreatePerson()
        {
            try
            {
                var LstCompanyMode = client.GetCompanyMode(Login.OrganizationId);
                ViewBag.LstCompanyMode = new SelectList(LstCompanyMode, "Id", "Mode", "8fa4e759-9cec-45bd-9d2b-f2e1d1875482");
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
             this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Create Person Page.");
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

        #region AddPerson
        [LoginAuthorize]
        public bool AddPerson(Person persontbl, string Location, string CompanyExternal)
        {
            try
            {
                HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                persontbl.Id = Guid.NewGuid();
                persontbl.OrgId = Login.OrganizationId;
                if (CompanyExternal != null)
                { persontbl.Company = new Guid(CompanyExternal); }

                var LstLocation = client.GetLocLst(Login.OrganizationId);
                var LocationData = LstLocation.Where(x => x.Name == Location).SingleOrDefault();
                persontbl.LocationLatitude = LocationData.Latittude;
                persontbl.Locationlongitude = LocationData.Longitude;
                persontbl.ModifiedBy = Login.Id;
                client.AddPerson(persontbl);
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Added New Person.");
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

        #region Edit Person
        [LoginAuthorize]
        public ActionResult EditPerson(string ID)
        {

            try
            {
                HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                Person objperson = client.GetOnePerson(Login.OrganizationId, new Guid(ID));
                ViewBag.PersonId = new Guid(ID);
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
               this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Edit Person Page.");
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
            Person obj = new Person();
            return View(obj);
        }

        [LoginAuthorize]
        [HttpPost]
        public bool EditPerson(Person persontbl, string Location, string CompanyExternal)
        {
            try
            {
                persontbl.OrgId = Login.OrganizationId;
                if (CompanyExternal != null)
                { persontbl.Company = new Guid(CompanyExternal); }

                var LstLocation = client.GetLocLst(Login.OrganizationId);
                
                persontbl.ModifiedBy = Login.Id;
                client.EditPerson(persontbl);
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
              this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "has Edited Person.");
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

        #region Make A lead from Manage Person Page
        [LoginAuthorize]
        public ActionResult MakeLead(string ID)
        {
            try
            {
                Person objperson = new Person();
                if (ID == null)
                {
                    objperson = client.GetLstPerson(Login.OrganizationId).FirstOrDefault();

                }
                else { objperson = client.GetOnePerson(Login.OrganizationId, new Guid(ID)); }
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Make Lead Page.");

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
            Person obj = new Person();
            return View(obj);
        }

        #endregion

        #region Get Product List for Make A lead
        [LoginAuthorize]
        public JsonResult GetProductList(string CampaignId)
        {
            try
            {
                Guid Id = new Guid(CampaignId);
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
            return Json(JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region AddNote
        [LoginAuthorize]
        public ActionResult AddNote(FormCollection collection)
        {
            if (!String.IsNullOrEmpty(collection["txtNotes"]))
            {
                try
                {
                    PersonLeadNote LeadNote = new PersonLeadNote();
                    LeadNote.Id = Guid.NewGuid();
                    LeadNote.Note = collection["txtNotes"];
                    LeadNote.PersonLeadId = new Guid(collection["PersonLeadId"]);
                    LeadNote.OrgId = Login.OrganizationId;
                    LeadNote.ModifiedBy = Login.Id;
                    client.AddLeadNote(LeadNote);
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Added Note.");
                    return RedirectToAction("LeadStage/" + collection["PersonLeadId"], "SalesManagement");
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
                return RedirectToAction("LeadStage/" + collection["PersonLeadId"], "SalesManagement");
            }

            return RedirectToAction("LeadStage/" + collection["PersonLeadId"], "SalesManagement");
        }

        #endregion

        #region Deal Stage Questions
        [LoginAuthorize]
        public ActionResult LeadStageQuestions(String personLeadId, String DealStageNameId, String movementMode)
        {
            try
            {
                TempData["personLeadId"] = personLeadId;
                TempData["DealStageNameId"] = DealStageNameId;
                TempData["movementMode"] = movementMode;
                var questions = client.GetListOfDealStageQuestions(Login.OrganizationId, new Guid(DealStageNameId), movementMode);

                List<PipelineQuestion> Questions = new List<PipelineQuestion>();
                foreach (var item in questions)
                {
                    Questions.Add(new PipelineQuestion
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
                ViewBag.MovementMode = movementMode;
                return View(Questions);
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
            List<PipelineQuestion> obj = new List<PipelineQuestion>();
            return View(obj);

        }


        #endregion

        #region move to next stage
        [LoginAuthorize]
        [HttpPost]
        public ActionResult NextStage(ICollection<DealStageAns> vls)
        {
            try
            {
                Guid dealstageid = new Guid();
                Guid PersonLeadId = new Guid();
                PersonLeadId = new Guid(TempData["personLeadId"].ToString());
                dealstageid = new Guid(TempData["DealStageNameId"].ToString());
                string movementMode = TempData["movementMode"].ToString();
                if (movementMode == "Won" || movementMode == "Lost" || movementMode == "dropped" || movementMode == "revertlead")
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
                        Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                        this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "moved stage to" + movementMode);
                        return RedirectToAction("SalesAgentView", "SalesManagement");
                    }
                }
                else if (movementMode == "backward")
                {
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
                        Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                      this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "moved stage to" + movementMode);
                        return RedirectToAction("LeadStage/" + PersonLeadId, "SalesManagement");
                    }
                }
                else
                {

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
                        Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                         this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "moved stage to" + movementMode);
                        return RedirectToAction("LeadStage/" + PersonLeadId, "SalesManagement");
                    }
                }
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "moved stage to" + movementMode);
                return RedirectToAction("LeadStage/" + PersonLeadId, "SalesManagement");
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
            return RedirectToAction("LeadStage/" + new Guid(TempData["personLeadId"].ToString()), "SalesManagement");
        }
        #endregion

        #region  Won Stage
        [LoginAuthorize]
        [HttpPost]
        public bool DealWon(string ActualValue, string SanctionedValue, DateTime ClosedDate, string PersonLeadId)
        {
            try
            {
                if (!string.IsNullOrEmpty(PersonLeadId))
                {

                    client.WonStage(Login.OrganizationId, ActualValue, SanctionedValue, ClosedDate, PersonLeadId);
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                    this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "moved to Won Stage.");

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

        #region ProductComparision

        public ActionResult ProductComparision()
        {
            return View();
        }

        #endregion


        public class PipelineQuestion
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
        public class ClassForChart
        {
            public string FirstName;
            public decimal ExpectedValue;
            public decimal SanctionedValue;
            public decimal ActualValue;
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



        [LoginAuthorize]
        public virtual JsonResult LoadState(String ID)
        {
            try
            {
                if (!String.IsNullOrEmpty(ID))
                {
                    HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                    var Statelst = client.GetStateLst(Login.OrganizationId).Where(a => a.CountryId == new Guid(ID)).Select(a => new { Id = a.Id, Name = a.Name }).ToList();

                    return Json(Statelst, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }


        [LoginAuthorize]
        public virtual JsonResult LoadCity(String ID)
        {
            try
            {
                if (!String.IsNullOrEmpty(ID))
                {
                    HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                    var Citylst = client.GetCityLst(Login.OrganizationId).Where(a => a.State_provience_county_ID == new Guid(ID)).Select(a => new { Id = a.Id, Name = a.Name }).ToList();

                    return Json(Citylst, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }



    }
}