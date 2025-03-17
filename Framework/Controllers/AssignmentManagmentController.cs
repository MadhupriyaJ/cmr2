using Core;
using FramWork.HelloService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using System.IO;
using ExceptionLogging;

namespace FramWork.Controllers
{
    public class AssignmentManagmentController : Controller
    {
        HelloServiceClient client = new FramWork.HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
        // GET: AssignmentManagment
        #region Single Lead Assignment
        [LoginAuthorize]
        public ActionResult SingleLeadAssignment()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Single Lead Assignment Page");
            return View();
        }
        #endregion

        #region Get PersonLead Details
        [LoginAuthorize]
        public ActionResult getPersonLeadDetails(String LeadId)
        {
            try
            {
                if (!String.IsNullOrEmpty(LeadId))
                {
                    HelloServiceClient client = new FramWork.HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                    PersonLead details = client.GetPersonleaddetails(Login.OrganizationId, new Guid(LeadId));
                    return View(details);
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

            PersonLead obj = new PersonLead();
            return View(obj);
        }
        #endregion

        #region EmployeeList
        [LoginAuthorize]
        public virtual JsonResult LoadEmployeedropdown(String teamid)
        {
            try
            {
                if (!String.IsNullOrEmpty(teamid))
                {
                    HelloServiceClient client = new FramWork.HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                    var query = client.getempbasedonteam(Login.OrganizationId, new Guid(teamid)).Select(a => new { Id = a.Id, Name = a.FullName });          //_repository.GetInformation(); //Here you return the data. 
                    return Json(query, JsonRequestBehavior.AllowGet);
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

            return Json(null, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Employee Details
        [LoginAuthorize]
        public ActionResult getEmployee_Details(String empid)
        {
            try
            {
                if (!String.IsNullOrEmpty(empid))
                {
                    HelloServiceClient client = new FramWork.HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                    Employee details = client.Getemployeedetails(Login.OrganizationId, new Guid(empid));
                    return View(details);
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

            PersonLead obj = new PersonLead();
            return View(obj);
        }
        #endregion

        #region Assign PersonLead to Employee
        [LoginAuthorize]
        public String AssignLead_toemployee(String Empid, List<String> LeadId)
        {
            try
            {
                if (!String.IsNullOrEmpty(Empid) && LeadId.Count() != 0)
                {
                    List<Guid> lid = new List<Guid>();
                    foreach (var item in LeadId)
                    {
                        lid.Add(new Guid(item));
                    }
                    HelloServiceClient client = new FramWork.HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                    String details = client.AssignLeadToEmployee(Login.OrganizationId, new Guid(Empid), lid.ToArray());
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                   this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Assigned Leads t0 Employee.");
                    return details;
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

            return "";
        }
        #endregion

        #region  Bulk Lead Assignment
        [LoginAuthorize]
        public ActionResult BulkLeadAssignment()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                   this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Bulk Lead Assignment Page.");
            return View();
        }


        #region View Employee Report
        [LoginAuthorize]
        public ActionResult getemployeeviewreport(string sortOrder, string currentFilter, string searchString, int? page, string Empid)
        {
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
            var Pl = client.GetPersonLeadLst(Login.OrganizationId).Where(a => a.Assignedto == new Guid(Empid)).ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                Pl = client.GetPersonLeadLst(Login.OrganizationId).Where(a => a.Assignedto == new Guid(Empid)).ToList();
            }
            switch (sortOrder)
            {
                case "name_desc":
                    Pl = Pl.OrderByDescending(s => s.Person.FirstName).ToList();
                    break;
                case "Date":
                    Pl = Pl.OrderBy(s => s.CreateDate).ToList(); ;
                    break;
                case "date_desc":
                    Pl = Pl.OrderByDescending(s => s.Person.FirstName).ToList(); ;
                    break;
                default:  // Name ascending 
                    Pl = Pl.OrderByDescending(s => s.CreateDate).ToList(); ;
                    break;
            }
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                  this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Employee Wise Report Page.");
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(Pl.ToPagedList(pageNumber, pageSize));
           
        }

        #endregion


        #region View Team Report
        [LoginAuthorize]
        public ActionResult getteamviewreport(string sortOrder, string currentFilter, string searchString, int? page, string teamid)
        {

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
            var Pl = client.GetPersonLeadLst(Login.OrganizationId).Where(a => a.TeamId == new Guid(teamid)).ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                Pl = client.GetPersonLeadLst(Login.OrganizationId).Where(a => a.TeamId == new Guid(teamid)).ToList();
            }
            switch (sortOrder)
            {
                case "name_desc":
                    Pl = Pl.OrderByDescending(s => s.Person.FirstName).ToList();
                    break;
                case "Date":
                    Pl = Pl.OrderBy(s => s.CreateDate).ToList();
                    break;
                case "date_desc":
                    Pl = Pl.OrderByDescending(s => s.Person.FirstName).ToList();
                    break;
                default:  // Name ascending 
                    Pl = Pl.OrderByDescending(s => s.CreateDate).ToList();
                    break;
            }
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                   this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Team Wise Report Page.");
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(Pl.ToPagedList(pageNumber, pageSize));

        }

        #endregion

        #region Get List of PersonLead
        [LoginAuthorize]

        public ActionResult getLeadList(string sortOrder, string currentFilter, string searchString, int? page)
        {
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
            var Pl = client.GetListOfOpenOppurtunities(Login.OrganizationId).ToList();

            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    Pl = client.GetPersonLeadLst(Login.OrganizationId).Where(a => a.Assignedto == new Guid(searchString)).ToList();
            //}
            switch (sortOrder)
            {
                case "name_desc":
                    Pl = Pl.OrderByDescending(s => s.Person.FirstName).ToList();
                    break;
                case "Date":
                    Pl = Pl.OrderBy(s => s.CreateDate).ToList();
                    break;
                case "date_desc":
                    Pl = Pl.OrderByDescending(s => s.Person.FirstName).ToList();
                    break;
                default:  // Name ascending 
                    Pl = Pl.OrderByDescending(s => s.CreateDate).ToList();
                    break;
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(Pl.ToPagedList(pageNumber, pageSize));

        }


        #endregion







        #endregion

        #region  Automated Lead Assignment

        [LoginAuthorize]
        public ActionResult AutomatedLeadAssignment()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                  this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Automated Lead Assignment.");
            return View();
        }

        [LoginAuthorize]
        public JsonResult Automatedleadassign()
        {
            try
            {
                Queue<Guid> queue = new Queue<Guid>();
                List<Employee> emp = client.GetEmployeeLst(Login.OrganizationId).ToList();
                List<EmployeeQueue> empqueuelist = client.GetEmployeeQueueLst(Login.OrganizationId).ToList();

                foreach (var item in emp)
                {
                    EmployeeQueue doesexists = empqueuelist.Where(a => a.EmpId == item.Id).FirstOrDefault();
                    EmployeeQueue tbl = new EmployeeQueue();
                    if (doesexists == null)
                    {
                        tbl.Id = Guid.NewGuid();
                        tbl.OrgId = Login.OrganizationId;
                        tbl.EmpId = item.Id;
                        tbl.Createdate = DateTime.Now;
                        tbl.MarkAsDelete = false;
                        client.AddemployeeQueue(tbl);

                    }
                }


                List<EmployeeQueue> empqueuelistcurrent = client.GetEmployeeQueueLst(Login.OrganizationId).OrderByDescending(c => c.Createdate).ToList();
                List<PersonLead> PlId = client.GetListOfOpenOppurtunities(Login.OrganizationId).ToList();
                var PlId1 = new List<pleadlist>();
                PlId1 = PlId.Select(a => new pleadlist { name = a.Person.FirstName + " " + a.Person.LastName }).ToList<pleadlist>();

                foreach (var item in PlId)
                {
                    queue.Enqueue(item.PersonId);
                }


                bool flag = true;
                int qcount = queue.Count();
                while (flag)
                {
                    foreach (var em in empqueuelistcurrent)
                    {
                        if (queue.Count != 0)
                        {
                            Guid Empid = queue.Dequeue();
                            PersonLead found = client.GetListOfOpenOppurtunities(Login.OrganizationId).
                                Where(a => a.PersonId == Empid).Select(x => new PersonLead { Id = x.Id }).FirstOrDefault();
                            client.SaveAssignedToPersonLead(found.Id, em.EmpId, Login.OrganizationId, Login.Id);

                            qcount--;

                            /*que current employee to  last*/
                            EmployeeQueue current_emp = client.GetEmployeeQueueLst(Login.OrganizationId).Where(a => a.EmpId == em.EmpId).FirstOrDefault();
                            EmployeeQueue CEmp = new EmployeeQueue();
                            CEmp.Id = current_emp.Id;
                            CEmp.EmpId = current_emp.EmpId;
                            CEmp.Createdate = DateTime.Now;
                            CEmp.OrgId = Login.OrganizationId;

                            client.EditemployeeQueue(CEmp);
                        }
                    }
                    empqueuelistcurrent = client.GetEmployeeQueueLst(Login.OrganizationId).OrderByDescending(c => c.Createdate).ToList();
                    if (qcount <= 0)
                    {
                        flag = false;
                    }
                }
                return Json(PlId1, JsonRequestBehavior.AllowGet);
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

        #region  Autonomation Lead Assignment
        [LoginAuthorize]
        public ActionResult AutonomationLeadAssignment()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                  this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Autonomation Lead Assignment Page.");
            return View();
        }
        #endregion
    }

    public class pleadlist
    {
        public String name { get; set; }
    }
}