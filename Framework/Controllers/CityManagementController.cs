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
    public class CityManagementController : Controller
    {
        // GET: CityManagement
        #region Index

        [LoginAuthorize]
        public ActionResult Index()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into List of City Page.");
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
                var LstCity = client.GetCityLst(Login.OrganizationId);

                totalData = LstCity.Count();
                if (!string.IsNullOrEmpty(search))
                    LstCity = LstCity.Where(a => a.Name.Contains(search)).ToArray();
                if (!string.IsNullOrEmpty(order))
                {
                    switch (order)
                    {
                        case "1":
                            LstCity = (orderBy == "asc" ? LstCity.OrderBy(a => a.Name) : LstCity.OrderByDescending(a => a.Name)).ToArray();
                            break;
                        case "2":
                            LstCity = (orderBy == "asc" ? LstCity.OrderBy(a => a.CreateDate) : LstCity.OrderByDescending(a => a.CreateDate)).ToArray();
                            break;
                        case "3":
                            LstCity = (orderBy == "asc" ? LstCity.OrderBy(a => a.ModifiedDate) : LstCity.OrderByDescending(a => a.ModifiedDate)).ToArray();
                            break;
                        case "4":
                            LstCity = (orderBy == "asc" ? LstCity.OrderBy(a => a.State_Provience_County.Name) : LstCity.OrderByDescending(a => a.State_Provience_County.Name)).ToArray();
                            break;
                        default:
                            LstCity = (orderBy == "asc" ? LstCity.OrderBy(a => a.Id) : LstCity.OrderByDescending(a => a.Id)).ToArray();
                            break;
                    }
                }
                else
                    LstCity = LstCity = (orderBy == "asc" ? LstCity.OrderBy(a => a.Id) : LstCity.OrderByDescending(a => a.Id)).ToArray();
                if (!String.IsNullOrEmpty(page) && !string.IsNullOrEmpty(displaylimit) && displaylimit != "-1")
                {
                    try
                    {
                        LstCity = LstCity.Skip(Convert.ToInt32(!String.IsNullOrEmpty(page) ? page : "0")).Take(Convert.ToInt32(displaylimit)).ToArray();
                    }
                    catch { }
                }


                string aa = "{";
                aa += "\"recordsTotal\": " + totalData + ", \"recordsFiltered\": " + LstCity.Count() + ",\"data\":[";
                int temp = 0;
                foreach (var item in LstCity)
                {
                    if (temp > 0)
                        aa += ",";
                    temp++;

                    var LstState = client.GetStateLst(Login.OrganizationId);
                    var state = LstState.Where(x => x.Id == item.State_provience_county_ID).SingleOrDefault();
                    aa += string.Format("[\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\"]",
                        item.Id, state.Name, item.Name, Convert.ToDateTime(item.CreateDate).ToString("dd/MM/yyyy"), Convert.ToDateTime(item.ModifiedDate).ToString("dd/MM/yyyy"), "<a class='btn default btn-xs blue' href='" + Url.Action("Edit") + "/" + item.Id + "'><i class='fa fa-edit'></i> Edit </a><a class='btn default btn-xs black' href='" + Url.Action("Delete") + "/" + item.Id + "'><i class='fa fa-trash-o'></i> Delete </a>");
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

        #region Create City
        [LoginAuthorize]
        public ActionResult CreateCity()
        {
            try
            {
                HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                var LstState = client.GetStateLst(Login.OrganizationId);
                ViewBag.LstState = new SelectList(LstState, "Id", "Name");
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Create City Page.");
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
            var LstState1 = new List<State_Provience_County>();
            ViewBag.LstState = new SelectList(LstState1, "Id", "Name");
            return View();
        }
        #endregion

        #region Add City
        [LoginAuthorize]
        public bool AddCity(string State, string City)
        {
            try
            {
                if (!String.IsNullOrEmpty(City))
                {
                    FramWork.HelloService.City tbl = new FramWork.HelloService.City();
                    tbl.Id = Guid.NewGuid();
                    tbl.State_provience_county_ID = new Guid(State);
                    tbl.Name = City;
                    tbl.ModifiedBy = Login.Id;
                    tbl.OrgId = Login.OrganizationId;
                    HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                    client.AddCity(tbl);
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                     this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Added City.");
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

        #region Get List Of City Based On StateId
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

        //[HttpGet]
        //public bool DeleteCity(Guid Id)
        //{
        //    try
        //    {
        //        HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
        //        client.DeleteCity(Id);
        //        return true;
        //    }

        //    catch (Exception)
        //    {

        //        throw;
        //        return false;
        //    }
        //}
    }
}