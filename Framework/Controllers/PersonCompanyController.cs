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
    public class PersonCompanyController : Controller
    {
        #region PersonCompany
        // GET: PersonCompany
        [LoginAuthorize]
        public ActionResult Index()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
     this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Person Company List Page.");
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
            var LstPersonCompany = client.GetPersonCompany(Login.OrganizationId);

            totalData = LstPersonCompany.Count();
            if (!string.IsNullOrEmpty(search))
                LstPersonCompany = LstPersonCompany.Where(a => a.Name.Contains(search)).ToArray();
            if (!string.IsNullOrEmpty(order))
            {
                switch (order)
                {
                    case "1":
                        LstPersonCompany = (orderBy == "asc" ? LstPersonCompany.OrderBy(a => a.Name) : LstPersonCompany.OrderByDescending(a => a.Name)).ToArray();
                        break;
                    case "2":
                        LstPersonCompany = (orderBy == "asc" ? LstPersonCompany.OrderBy(a => a.CreateDate) : LstPersonCompany.OrderByDescending(a => a.CreateDate)).ToArray();
                        break;
                    case "3":
                        LstPersonCompany = (orderBy == "asc" ? LstPersonCompany.OrderBy(a => a.ModifiedDate) : LstPersonCompany.OrderByDescending(a => a.ModifiedDate)).ToArray();
                        break;
                    default:
                        LstPersonCompany = (orderBy == "asc" ? LstPersonCompany.OrderBy(a => a.Id) : LstPersonCompany.OrderByDescending(a => a.Id)).ToArray();
                        break;
                }
            }
            else
                LstPersonCompany = LstPersonCompany = (orderBy == "asc" ? LstPersonCompany.OrderBy(a => a.Id) : LstPersonCompany.OrderByDescending(a => a.Id)).ToArray();
            if (!String.IsNullOrEmpty(page) && !string.IsNullOrEmpty(displaylimit) && displaylimit != "-1")
            {
                try
                {
                    LstPersonCompany = LstPersonCompany.Skip(Convert.ToInt32(!String.IsNullOrEmpty(page) ? page : "0")).Take(Convert.ToInt32(displaylimit)).ToArray();
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
            aa += "\"recordsTotal\": " + totalData + ", \"recordsFiltered\": " + LstPersonCompany.Count() + ",\"data\":[";
            int temp = 0;
            foreach (var item in LstPersonCompany)
            {
                if (temp > 0)
                    aa += ",";
                temp++;
                var LstCity = client.GetCityLst(Login.OrganizationId);
                var City = LstCity.Where(x => x.Id == item.CityId).SingleOrDefault();

                aa += string.Format("[\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\"]",
                 item.Id, item.Name, City.Name, Convert.ToDateTime(item.CreateDate).ToString("dd/MM/yyyy"), Convert.ToDateTime(item.ModifiedDate).ToString("dd/MM/yyyy"), "<a class='btn default btn-xs blue' href='" + Url.Action("Edit") + "/" + item.Id + "'><i class='fa fa-edit'></i> Edit </a><a class='btn default btn-xs black' href='" + Url.Action("Delete") + "/" + item.Id + "'><i class='fa fa-trash-o'></i> Delete </a>");
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

        #region Create Person Company
        [LoginAuthorize]
        public ActionResult CreatePersonCompany()
        {
            try
            {
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Create Person Company Page.");
                HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                var LstCompanyMode = client.GetCompanyMode(Login.OrganizationId);
                ViewBag.LstCompanyMode = new SelectList(LstCompanyMode, "Id", "Mode", "8fa4e759-9cec-45bd-9d2b-f2e1d1875482");

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

        #region Add Person Company
        [LoginAuthorize]
        public JsonResult AddPersonCompany(Person_Company PersonCompany)
        {
            try
            {   PersonCompany.Id = Guid.NewGuid();
                PersonCompany.ModifiedBy = Login.Id;
                PersonCompany.OrgId = Login.OrganizationId;
                HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                Guid ID = client.AddPersonCompany(PersonCompany);
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Added Person Company.");
                return Json(new { SUCCESS = true, ID = ID });
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

  

    }
}