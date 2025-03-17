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
    public class DepartmentManagementController : Controller
    {
        #region Index
        [LoginAuthorize]
        public ActionResult Index()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                 this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into List Of Department Page.");
            return View();
        }
        #endregion
        // GET: DepartmentManagement

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
                var LstDept = client.GetDeptLst(Login.OrganizationId);

                totalData = LstDept.Count();
                if (!string.IsNullOrEmpty(search))
                    LstDept = LstDept.Where(a => a.Name.Contains(search)).ToArray();
                if (!string.IsNullOrEmpty(order))
                {
                    switch (order)
                    {
                        case "1":
                            LstDept = (orderBy == "asc" ? LstDept.OrderBy(a => a.Name) : LstDept.OrderByDescending(a => a.Name)).ToArray();
                            break;
                        case "2":
                            LstDept = (orderBy == "asc" ? LstDept.OrderBy(a => a.CreatedDate) : LstDept.OrderByDescending(a => a.CreatedDate)).ToArray();
                            break;
                        case "3":
                            LstDept = (orderBy == "asc" ? LstDept.OrderBy(a => a.ModifiedDate) : LstDept.OrderByDescending(a => a.ModifiedDate)).ToArray();
                            break;
                        default:
                            LstDept = (orderBy == "asc" ? LstDept.OrderBy(a => a.Id) : LstDept.OrderByDescending(a => a.Id)).ToArray();
                            break;
                    }
                }
                else
                    LstDept = LstDept = (orderBy == "asc" ? LstDept.OrderBy(a => a.Id) : LstDept.OrderByDescending(a => a.Id)).ToArray();
                if (!String.IsNullOrEmpty(page) && !string.IsNullOrEmpty(displaylimit) && displaylimit != "-1")
                {
                    try
                    {
                        LstDept = LstDept.Skip(Convert.ToInt32(!String.IsNullOrEmpty(page) ? page : "0")).Take(Convert.ToInt32(displaylimit)).ToArray();
                    }
                    catch { }
                }


                string aa = "{";
                aa += "\"recordsTotal\": " + totalData + ", \"recordsFiltered\": " + LstDept.Count() + ",\"data\":[";
                int temp = 0;
                foreach (var item in LstDept)
                {
                    if (temp > 0)
                        aa += ",";
                    temp++;
                    aa += string.Format("[\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\"]",
                     item.Id, item.Name, Convert.ToDateTime(item.CreatedDate).ToString("dd/MM/yyyy"), Convert.ToDateTime(item.ModifiedDate).ToString("dd/MM/yyyy"), "<a class='btn default btn-xs blue' href='" + Url.Action("Edit") + "/" + item.Id + "'><i class='fa fa-edit'></i> Edit </a><a class='btn default btn-xs black' href='" + Url.Action("Delete") + "/" + item.Id + "'><i class='fa fa-trash-o'></i> Delete </a>");
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

        #region Create Department
        [LoginAuthorize]
        public ActionResult CreateDepartment()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Department Create Page.");
            return View();
        }
        #endregion

        #region Add Department
        [LoginAuthorize]
        public bool AddDepartment(String Department)
        {
            try
            {
                if (!String.IsNullOrEmpty(Department))
                {
                    FramWork.HelloService.Department tbl = new FramWork.HelloService.Department();
                    tbl.Id = Guid.NewGuid();
                    tbl.Name = Department;
                    tbl.ModifiedBy = Login.Id;
                    tbl.OrgId = Login.OrganizationId;
                    HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                    client.AddDepartment(tbl);
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                     this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Added Department.");
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