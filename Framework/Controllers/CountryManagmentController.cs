using Core;
using ExceptionLogging;
using FramWork.HelloService;
using FramWork.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FramWork.Controllers
{
    public class CountryManagmentController : Controller
    {

        #region Index
        // GET: CountryManagment
        [LoginAuthorize]
        public ActionResult Index()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
       this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Country List Page");
            return View();
        }
        #endregion

        #region TableData
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
                var LstCountry = client.GetCountryLst(Login.OrganizationId);

                totalData = LstCountry.Count();
                if (!string.IsNullOrEmpty(search))
                    LstCountry = LstCountry.Where(a => a.CountyName.Contains(search)).ToArray();
                if (!string.IsNullOrEmpty(order))
                {
                    switch (order)
                    {
                        case "1":
                            LstCountry = (orderBy == "asc" ? LstCountry.OrderBy(a => a.CountyName) : LstCountry.OrderByDescending(a => a.CountyName)).ToArray();
                            break;
                        case "2":
                            LstCountry = (orderBy == "asc" ? LstCountry.OrderBy(a => a.CreateDate) : LstCountry.OrderByDescending(a => a.CreateDate)).ToArray();
                            break;
                        case "3":
                            LstCountry = (orderBy == "asc" ? LstCountry.OrderBy(a => a.ModifiedDate) : LstCountry.OrderByDescending(a => a.ModifiedDate)).ToArray();
                            break;
                        case "4":
                            LstCountry = (orderBy == "asc" ? LstCountry.OrderBy(a => a.IsoCode) : LstCountry.OrderByDescending(a => a.IsoCode)).ToArray();
                            break;
                        default:
                            LstCountry = (orderBy == "asc" ? LstCountry.OrderBy(a => a.Id) : LstCountry.OrderByDescending(a => a.Id)).ToArray();
                            break;
                    }
                }
                else
                    LstCountry = LstCountry = (orderBy == "asc" ? LstCountry.OrderBy(a => a.Id) : LstCountry.OrderByDescending(a => a.Id)).ToArray();
                if (!String.IsNullOrEmpty(page) && !string.IsNullOrEmpty(displaylimit) && displaylimit != "-1")
                {
                    try
                    {
                        LstCountry = LstCountry.Skip(Convert.ToInt32(!String.IsNullOrEmpty(page) ? page : "0")).Take(Convert.ToInt32(displaylimit)).ToArray();
                    }
                    catch { }
                }


                string aa = "{";
                aa += "\"recordsTotal\": " + totalData + ", \"recordsFiltered\": " + LstCountry.Count() + ",\"data\":[";
                int temp = 0;
                foreach (var item in LstCountry)
                {
                    if (temp > 0)
                        aa += ",";
                    temp++;
                    aa += string.Format("[\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\"]",
                        item.Id, item.CountyName, item.Locale, Convert.ToDateTime(item.CreateDate).ToString("dd/MM/yyyy"), Convert.ToDateTime(item.ModifiedDate).ToString("dd/MM/yyyy"), "<a class='btn default btn-xs blue' href='" + Url.Action("Edit") + "/" + item.Id + "'><i class='fa fa-edit'></i> Edit </a><a class='btn default btn-xs black' href='" + Url.Action("Delete") + "/" + item.Id + "'><i class='fa fa-trash-o'></i> Delete </a>");
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

        #region Create

        [LoginAuthorize]
        public ActionResult Create()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
       this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Country Create Page");

            return View();
        }
        #endregion

        #region Add Country
        [LoginAuthorize]
        public bool AddCountry(string CountryName, string IsoCode, string IsoCurrency, string GMT, string Locale)
        {
            try
            {
                if (!String.IsNullOrEmpty(CountryName))
                {
                    FramWork.HelloService.Country tbl = new FramWork.HelloService.Country();
                    tbl.Id = Guid.NewGuid();
                    tbl.OrgId = Login.OrganizationId;
                    tbl.IsoCode = IsoCode;
                    tbl.CountyName = CountryName;
                    tbl.IsoCurrency = IsoCurrency;
                    tbl.GMT = Convert.ToInt32(GMT);
                    tbl.Locale = Locale;
                    tbl.ModifiedBy = Login.Id;
                    HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
        this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Added Country");
                    client.AddCountry(tbl);
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

        #region Get country list
        [LoginAuthorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Autocomplete(string term)
        {
            try
            {
                var result = new List<KeyValuePair<string, string>>();
                IList<SelectListItem> List = new List<SelectListItem>();
                List<RegionInfo> countries = new List<RegionInfo>();
                foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
                {
                    RegionInfo country = new RegionInfo(culture.LCID);
                    if (countries.Where(p => p.Name == country.Name).Count() == 0)
                        countries.Add(country);
                }
                foreach (var item in countries)
                {
                    result.Add(new KeyValuePair<string, string>(item.TwoLetterISORegionName, item.DisplayName));
                }
                var result3 = result.Where(s => s.Value.ToLower().Contains(term.ToLower())).Select(w => w).ToList();
                return Json(result3, JsonRequestBehavior.AllowGet);
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

        [LoginAuthorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetDetail(String id)
        {
            try
            {
                RegionInfo ri = new RegionInfo(id);
                ctryModel model = new ctryModel();
                model.ISOCurrency = ri.CurrencyEnglishName;
                model.ISO = ri.ThreeLetterISORegionName;

                return Json(model);
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
        #endregion

        public class ctryModel
        {
            public String ISOCurrency { get; set; }
            public string ISO { get; set; }

            public String GMT { get; set; }
            public String Locale { get; set; }


        }

    }
}