using Core;
using ExceptionLogging;
using FramWork.HelloService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using PagedList.Mvc;
//using PagedList;

namespace FramWork.Controllers
{
    public class UserManagmentController : Controller
    {

        #region Index
        // GET: UserManagment
        [LoginAuthorize]
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region Roles
        [LoginAuthorize]
        public ActionResult Roles()
        {
            return View();
        }
        #endregion

        #region Add Roles
        [LoginAuthorize]
        public String AddRoles(String RoleName, String Description)
        {
            try
            {
                if (!String.IsNullOrEmpty(RoleName))
                {
                    FramWork.HelloService.Role tbl = new FramWork.HelloService.Role();
                    tbl.OrgId = Login.OrganizationId;
                    tbl.Name = RoleName;
                    tbl.Description = Description;
                    HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                    bool Success = client.Addrole(tbl);
                    if (Success == true)
                    {
                        return "success";
                    }
                    else
                    {
                        return "failed";
                    }
                }
                else
                {

                }
                return "";
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

        #region MyRegion
        [LoginAuthorize]
        public ActionResult ListRoles(string sortOrder, string currentFilter, string searchString, int? page)
        {
            HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
            ViewBag.Roles = client.ListRoles(Login.OrganizationId);
            return View();
        }
        #endregion

  
        #region Add Roles To User
        [LoginAuthorize]
        public ActionResult AssignRolesTouser()
        {
              return View();
        }

        #region  Add ROles to the user
        [LoginAuthorize]
        public String Assign_Role_to_User(String UserId, List<String> roles)
        {
            try
            {
                if (!String.IsNullOrEmpty(UserId) && roles.Count() != 0)
                {
                    HelloServiceClient client = new FramWork.HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                  bool success= client.AssignRoleTo_User(Convert.ToInt32(UserId), roles.ToArray());
                    if(success)
                    {
                        return "success";
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
        #endregion


    }
}

   
