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
    public class ModuleManagmentController : Controller
    {
        // GET: ModuleManagment
        #region List Modules
        [LoginAuthorize]
        public ActionResult ListModules()
        {
            return View();
        }
        #endregion

        #region Create
        [LoginAuthorize]
        public ActionResult Create()
        {
            return View();
        }
        #endregion

        #region Add new Module
        [LoginAuthorize]
        public String AddnewModule(String modulename, String Controller_Method, String Actionname, String ddlgetpost, String ddlstatus)
        {
            try
            {
                if (!String.IsNullOrEmpty(modulename) && !String.IsNullOrEmpty(Controller_Method) &&
                    !String.IsNullOrEmpty(Actionname) && !String.IsNullOrEmpty(ddlgetpost) &&
                    !String.IsNullOrEmpty(ddlstatus))
                {
                    HelloServiceClient client = new FramWork.HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                    FramWork.HelloService.Module tbl = new Module();
                    tbl.Name = modulename;
                    tbl.Code = Controller_Method + "." + Actionname + "." + ddlgetpost;
                    tbl.Status = Convert.ToBoolean(ddlstatus);
                    bool success = client.AddModule(tbl);
                    if (success)
                    {
                        return "success";
                    }
                    else
                    {
                        return "failed";
                    }
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


        #region Role Assigned Module Action
        [LoginAuthorize]
        public ActionResult RoleAssignedModuleAction()
        {
            return View();
        }
        #endregion   

        #region List Of Modules
        [LoginAuthorize]
        public ActionResult Lisofmodules(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                HelloServiceClient client = new FramWork.HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                ViewBag.Module = client.GetModuleList();
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

        #region  Add modules to the Role
        [LoginAuthorize]
        public String Add_modules_to_theRole(String RoleId, List<String> modules)
        {
            try
            {
                if (!String.IsNullOrEmpty(RoleId) && modules.Count() != 0)
                {
                    HelloServiceClient client = new FramWork.HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                    client.AssignModuleTo_Role(Convert.ToInt32(RoleId), modules.ToArray());
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
    }
}