using Core;
using ExceptionLogging;
using FramWork.HelloService;
using GoogleMaps.LocationServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FramWork.Controllers
{
    public class LocationManagementController : Controller
    {
        // GET: LocationManagement
        #region Index
        [LoginAuthorize]
        public ActionResult Index()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into List of Location.");
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
                var LstLoc = client.GetLocLst(Login.OrganizationId);

                totalData = LstLoc.Count();
                if (!string.IsNullOrEmpty(search))
                    LstLoc = LstLoc.Where(a => a.Name.Contains(search)).ToArray();
                if (!string.IsNullOrEmpty(order))
                {
                    switch (order)
                    {
                        case "1":
                            LstLoc = (orderBy == "asc" ? LstLoc.OrderBy(a => a.Name) : LstLoc.OrderByDescending(a => a.Name)).ToArray();
                            break;
                        case "2":
                            LstLoc = (orderBy == "asc" ? LstLoc.OrderBy(a => a.CreateDate) : LstLoc.OrderByDescending(a => a.CreateDate)).ToArray();
                            break;
                        case "3":
                            LstLoc = (orderBy == "asc" ? LstLoc.OrderBy(a => a.ModifiedDate) : LstLoc.OrderByDescending(a => a.ModifiedDate)).ToArray();
                            break;
                        default:
                            LstLoc = (orderBy == "asc" ? LstLoc.OrderBy(a => a.Id) : LstLoc.OrderByDescending(a => a.Id)).ToArray();
                            break;
                    }
                }
                else
                    LstLoc = LstLoc = (orderBy == "asc" ? LstLoc.OrderBy(a => a.Id) : LstLoc.OrderByDescending(a => a.Id)).ToArray();
                if (!String.IsNullOrEmpty(page) && !string.IsNullOrEmpty(displaylimit) && displaylimit != "-1")
                {
                    try
                    {
                        LstLoc = LstLoc.Skip(Convert.ToInt32(!String.IsNullOrEmpty(page) ? page : "0")).Take(Convert.ToInt32(displaylimit)).ToArray();
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
                aa += "\"recordsTotal\": " + totalData + ", \"recordsFiltered\": " + LstLoc.Count() + ",\"data\":[";
                int temp = 0;
                foreach (var item in LstLoc)
                {
                    if (temp > 0)
                        aa += ",";
                    temp++;
                    aa += string.Format("[\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\"]",
                     item.Id, item.Name, Convert.ToDateTime(item.CreateDate).ToString("dd/MM/yyyy"), Convert.ToDateTime(item.ModifiedDate).ToString("dd/MM/yyyy"), "<a class='btn default btn-xs blue' href='" + Url.Action("Edit") + "/" + item.Id + "'><i class='fa fa-edit'></i> Edit </a><a class='btn default btn-xs black' href='" + Url.Action("Delete") + "/" + item.Id + "'><i class='fa fa-trash-o'></i> Delete </a>");
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

        #region Create Location
        [LoginAuthorize]
        public ActionResult CreateLocation()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Location Create Page.");
            return View();
        }
        #endregion

        #region Get Latitude Longitude
        [LoginAuthorize]
        [HttpPost]
        public JsonResult GetLatitudeLongitude(string LocationName)
        {
            try
            {
                var address = LocationName;

                var locationService = new GoogleLocationService();
                var point = locationService.GetLatLongFromAddress(address);

                var latitude = point.Latitude;
                var longitude = point.Longitude;

                return Json(new { latitude, longitude });
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

        #region Add Location
        [LoginAuthorize]
        public bool AddLocation(String Location, string GeoLocation, string Latittude, string Longitude)
        {
            try
            {
                if (!String.IsNullOrEmpty(Location))
                {
                    FramWork.HelloService.Location tbl = new FramWork.HelloService.Location();
                    tbl.Id = Guid.NewGuid();
                    tbl.Name = Location;
                    tbl.GeoLocation = GeoLocation;
                    tbl.Latittude = Latittude;
                    tbl.Longitude = Longitude;
                    tbl.ModifiedBy = Login.Id;
                    tbl.OrgId = Login.OrganizationId;
                    HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                    client.AddLocation(tbl);
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Added Location.");
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

    }
}