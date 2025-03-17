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
    public class StateManagementController : Controller
    {
        // GET: StateManagement
        #region Index
        [LoginAuthorize]
        public ActionResult Index()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
     this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into List of State Page.");
            return View();
        }
        #endregion

        #region Table Data
        [HttpGet, LoginAuthorize]
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
                var LstState = client.GetStateLst(Login.OrganizationId);

                totalData = LstState.Count();
                if (!string.IsNullOrEmpty(search))
                    LstState = LstState.Where(a => a.Name.Contains(search)).ToArray();
                if (!string.IsNullOrEmpty(order))
                {
                    switch (order)
                    {
                        case "1":
                            LstState = (orderBy == "asc" ? LstState.OrderBy(a => a.Name) : LstState.OrderByDescending(a => a.Name)).ToArray();
                            break;
                        case "2":
                            LstState = (orderBy == "asc" ? LstState.OrderBy(a => a.CreateDate) : LstState.OrderByDescending(a => a.CreateDate)).ToArray();
                            break;
                        case "3":
                            LstState = (orderBy == "asc" ? LstState.OrderBy(a => a.ModifiedDate) : LstState.OrderByDescending(a => a.ModifiedDate)).ToArray();
                            break;
                        case "4":
                            LstState = (orderBy == "asc" ? LstState.OrderBy(a => a.Country.CountyName) : LstState.OrderByDescending(a => a.Country.CountyName)).ToArray();
                            break;
                        default:
                            LstState = (orderBy == "asc" ? LstState.OrderBy(a => a.Id) : LstState.OrderByDescending(a => a.Id)).ToArray();
                            break;
                    }
                }
                else
                    LstState = LstState = (orderBy == "asc" ? LstState.OrderBy(a => a.Id) : LstState.OrderByDescending(a => a.Id)).ToArray();
                if (!String.IsNullOrEmpty(page) && !string.IsNullOrEmpty(displaylimit) && displaylimit != "-1")
                {
                    try
                    {
                        LstState = LstState.Skip(Convert.ToInt32(!String.IsNullOrEmpty(page) ? page : "0")).Take(Convert.ToInt32(displaylimit)).ToArray();
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
                aa += "\"recordsTotal\": " + totalData + ", \"recordsFiltered\": " + LstState.Count() + ",\"data\":[";
                int temp = 0;
                foreach (var item in LstState)
                {
                    if (temp > 0)
                        aa += ",";
                    temp++;

                    var LstCountry = client.GetCountryLst(Login.OrganizationId);
                    var Country = LstCountry.Where(x => x.Id == item.CountryId).SingleOrDefault();
                    aa += string.Format("[\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\"]",
                        item.Id, Country.CountyName, item.Name, Convert.ToDateTime(item.CreateDate).ToString("dd/MM/yyyy"), Convert.ToDateTime(item.ModifiedDate).ToString("dd/MM/yyyy"), "<a class='btn default btn-xs blue' href='" + Url.Action("Edit") + "/" + item.Id + "'><i class='fa fa-edit'></i> Edit </a><a class='btn default btn-xs black' href='" + Url.Action("Delete") + "/" + item.Id + "'><i class='fa fa-trash-o'></i> Delete </a>");
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

        #region Create State
        [LoginAuthorize]
        public ActionResult CreateState()
        {
            HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
            var LstCountry = client.GetCountryLst(Login.OrganizationId);
            ViewBag.LstCountry = new SelectList(LstCountry, "Id", "CountyName");
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into State Create Page.");
            return View();
        }
        #endregion

        #region Add State
        [LoginAuthorize]
        public bool AddState(string Country, string StateName)
        {
            try
            {
                if (!String.IsNullOrEmpty(StateName))
                {
                    FramWork.HelloService.State_Provience_County tbl = new FramWork.HelloService.State_Provience_County();
                    tbl.Id = Guid.NewGuid();
                    tbl.CountryId = new Guid(Country);
                    tbl.Name = StateName;
                    tbl.ModifiedBy = Login.Id;
                    tbl.OrgId = Login.OrganizationId;
                    HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                    client.AddState(tbl);
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                    this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Added State.");
                }
                else
                {

                }
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
        
        #region LoadState On country Id
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
    }
}