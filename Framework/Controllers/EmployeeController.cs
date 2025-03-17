﻿
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
    public class EmployeeController : Controller
    {
        // GET: Employee

        #region Index
        [LoginAuthorize]
        public ActionResult Index()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
              this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Employee List Page");
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
                var LstEmployee = client.GetEmployeeLst(Login.OrganizationId);

                totalData = LstEmployee.Count();
                if (!string.IsNullOrEmpty(search))
                    LstEmployee = LstEmployee.Where(a => (a.User.FirstName + " " + a.User.LastName).ToString().Contains(search)).ToArray();
                if (!string.IsNullOrEmpty(order))
                {
                    switch (order)
                    {
                        case "1":
                            LstEmployee = (orderBy == "asc" ? LstEmployee.OrderBy(a => (a.User.FirstName + " " + a.User.LastName).ToString()) : LstEmployee.OrderByDescending(a => (a.User.FirstName + " " + a.User.LastName).ToString())).ToArray();
                            break;
                        case "2":
                            LstEmployee = (orderBy == "asc" ? LstEmployee.OrderBy(a => a.CreateDate) : LstEmployee.OrderByDescending(a => a.CreateDate)).ToArray();
                            break;
                        case "3":
                            LstEmployee = (orderBy == "asc" ? LstEmployee.OrderBy(a => a.ModifiedDate) : LstEmployee.OrderByDescending(a => a.ModifiedDate)).ToArray();
                            break;
                        case "4":
                            LstEmployee = (orderBy == "asc" ? LstEmployee.OrderBy(a => a.State_Provience_County.Name) : LstEmployee.OrderByDescending(a => a.State_Provience_County.Name)).ToArray();
                            break;
                        default:
                            LstEmployee = (orderBy == "asc" ? LstEmployee.OrderBy(a => a.Id) : LstEmployee.OrderByDescending(a => a.Id)).ToArray();
                            break;
                    }
                }
                else
                    LstEmployee = LstEmployee = (orderBy == "asc" ? LstEmployee.OrderBy(a => a.Id) : LstEmployee.OrderByDescending(a => a.Id)).ToArray();
                if (!String.IsNullOrEmpty(page) && !string.IsNullOrEmpty(displaylimit) && displaylimit != "-1")
                {
                    try
                    {
                        LstEmployee = LstEmployee.Skip(Convert.ToInt32(!String.IsNullOrEmpty(page) ? page : "0")).Take(Convert.ToInt32(displaylimit)).ToArray();
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
                aa += "\"recordsTotal\": " + totalData + ", \"recordsFiltered\": " + LstEmployee.Count() + ",\"data\":[";
                int temp = 0;
                foreach (var item in LstEmployee)
                {
                    if (temp > 0)
                        aa += ",";
                    temp++;

                    //var LstState = client.GetStateLst(Login.OrganizationId);
                    //var state = LstState.Where(x => x.Id == item.State_Provience_County.Id).SingleOrDefault();
                    aa += string.Format("[\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\"]",
                        item.Id, (item.User.FirstName + " " + item.User.LastName).ToString(), Convert.ToDateTime(item.CreateDate).ToString("dd/MM/yyyy"), Convert.ToDateTime(item.ModifiedDate).ToString("dd/MM/yyyy"), "<a class='btn default btn-xs blue' href='" + Url.Action("Edit") + "/" + item.Id + "'><i class='fa fa-edit'></i> Edit </a><a class='btn default btn-xs black' href='" + Url.Action("Delete") + "/" + item.Id + "'><i class='fa fa-trash-o'></i> Delete </a>");
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

        #region Create Employee
        [LoginAuthorize]
        public ActionResult CreateEmployee()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
             this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Create Employee Page");
            return View();
        }
        #endregion

        #region Add Employee
        [LoginAuthorize]
        public bool AddEmployee(User usertbl, Employee emptbl)
        {
            try
            {
                if (!usertbl.Equals(null) && !emptbl.Equals(null))
                {
                    HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                    usertbl.OrgId = Login.OrganizationId;
                    usertbl.Salt = Core.Basic.GetSalt();
                    usertbl.Password = Core.Basic.GetHashPassword(usertbl.Password, usertbl.Salt);
                    usertbl.Status = 1;
                    usertbl.Attempts = 0;
                    int Id = client.AddUser(usertbl);

                    emptbl.Id = Guid.NewGuid();
                    emptbl.OrgId = Login.OrganizationId;
                    emptbl.UserId = Id;
                    emptbl.ModifiedBy = Login.Id;
                    client.Addemployee(emptbl);
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                      this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Added New Employee");
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

        #region Load State
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

        #region Load City
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

    }
}