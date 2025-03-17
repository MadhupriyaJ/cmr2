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
    public class GenderManagementController : Controller
    {
        // GET: GenderManagement
        #region Index
        [LoginAuthorize]
        public ActionResult Index()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                 this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into List of Gender Page.");
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
                var LstGender = client.GetGenderLst(Login.OrganizationId);

                totalData = LstGender.Count();
                if (!string.IsNullOrEmpty(search))
                    LstGender = LstGender.Where(a => a.Name.Contains(search)).ToArray();
                if (!string.IsNullOrEmpty(order))
                {
                    switch (order)
                    {
                        case "1":
                            LstGender = (orderBy == "asc" ? LstGender.OrderBy(a => a.Name) : LstGender.OrderByDescending(a => a.Name)).ToArray();
                            break;
                        case "2":
                            LstGender = (orderBy == "asc" ? LstGender.OrderBy(a => a.CreateDate) : LstGender.OrderByDescending(a => a.CreateDate)).ToArray();
                            break;
                        case "3":
                            LstGender = (orderBy == "asc" ? LstGender.OrderBy(a => a.ModifiedDate) : LstGender.OrderByDescending(a => a.ModifiedDate)).ToArray();
                            break;
                        default:
                            LstGender = (orderBy == "asc" ? LstGender.OrderBy(a => a.Id) : LstGender.OrderByDescending(a => a.Id)).ToArray();
                            break;
                    }
                }
                else
                    LstGender = LstGender = (orderBy == "asc" ? LstGender.OrderBy(a => a.Id) : LstGender.OrderByDescending(a => a.Id)).ToArray();
                if (!String.IsNullOrEmpty(page) && !string.IsNullOrEmpty(displaylimit) && displaylimit != "-1")
                {
                    try
                    {
                        LstGender = LstGender.Skip(Convert.ToInt32(!String.IsNullOrEmpty(page) ? page : "0")).Take(Convert.ToInt32(displaylimit)).ToArray();
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
                aa += "\"recordsTotal\": " + totalData + ", \"recordsFiltered\": " + LstGender.Count() + ",\"data\":[";
                int temp = 0;
                foreach (var item in LstGender)
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

        #region Create Gender
        [LoginAuthorize]
        public ActionResult CreateGender()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
     this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Create Gender Page.");
            return View();
        }
        #endregion

        #region Add Gender
        [LoginAuthorize]
        public bool AddGender(String Gender)
        {
            try
            {
                if (!String.IsNullOrEmpty(Gender))
                {
                    FramWork.HelloService.Gender tbl = new FramWork.HelloService.Gender();
                    tbl.Id = Guid.NewGuid();
                    tbl.Name = Gender;
                    tbl.ModifiedBy = Login.Id;
                    tbl.OrgId = Login.OrganizationId;
                    HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                    client.AddGender(tbl);
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
     this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Added Gender.");
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