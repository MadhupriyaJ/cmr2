using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FramWork.HelloService;
using Core;
using System.IO;
using ExceptionLogging;

namespace FramWork.Controllers
{
    public class SignupController : Controller
    {

        #region Index
        // GET: Signup
        [LoginAuthorize]
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region AddOrganization
          [LoginAuthorize]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="TypeOfBusiness"></param>
        /// <param name="contactName"></param>
        /// <param name="ContactNumber"></param>
        /// <param name="website"></param>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="email"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public bool AddOrganization(String Name, String TypeOfBusiness, String contactName, String ContactNumber, String website,
            String firstname, String lastname, String email, String username, String password)
        {

            try
            {
                if (!string.IsNullOrEmpty(Name) && !String.IsNullOrEmpty(TypeOfBusiness) && !String.IsNullOrEmpty(contactName)
                    && !String.IsNullOrEmpty(ContactNumber) && !String.IsNullOrEmpty(website) && !String.IsNullOrEmpty(firstname) && !String.IsNullOrEmpty(lastname)
                    && !String.IsNullOrEmpty(email) && !String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password))
                {

                    HelloServiceClient client = new HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");

                    HelloService.Organization table = new HelloService.Organization();
                    table.Id = Guid.NewGuid();
                    table.OrgCrName = Name;
                    table.TypeOfBussiness = TypeOfBusiness;
                    table.PointOfContactName = contactName;
                    table.PointOfContactPhone = ContactNumber;
                    table.Website = website;
                    client.AddOrganization(table);
                    /*Add User */
                    HelloService.User usrtbl = new HelloService.User();
                    usrtbl.OrgId = table.Id;
                    usrtbl.FirstName = firstname;
                    usrtbl.LastName = lastname;
                    usrtbl.Email = email;
                    usrtbl.UserName = username;
                    usrtbl.Salt = Core.Basic.GetSalt();
                    usrtbl.Password = Core.Basic.GetHashPassword(password, usrtbl.Salt);
                    int userid = client.AddUser(usrtbl);
                    /*--------Assign Role To User(Here We Are giving Admin RIghts)*/

                    List<string> roles = new List<string>();
                    roles.Add("1");//this is hard coded has to change it

                    bool success = client.AssignRoleTo_User(userid, roles.ToArray());

                    Employee tbl = new Employee();
                    tbl.Id = Guid.NewGuid();
                    tbl.OrgId = table.Id;
                    tbl.UserId = userid;
                    client.Addemployee(tbl);
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
  
    }
}