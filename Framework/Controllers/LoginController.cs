using Core;
using ExceptionLogging;
using FramWork.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace FramWork.Controllers
{
    public class LoginController : Controller
    {
        FrameworkEntities db = new FrameworkEntities();

        // GET: Login

        #region Index Get

        // GET: Login
        public ActionResult Index()
        {

            try
            {
                CoreSettings settings = db.CoreSettings.First();

                if (settings.LoginType == 2)
                {
                    ViewBag.IsWindows = 1;
                }
            }
            catch (Exception ex)
            {
                Core.Basic.WriteLog("Login_Index_43", ex.Message, ex.ToString());
                ViewBag.Msg = new string[] { "0", Resources.Resources.GeneralMsgPleaseTryLater };
                return View();
            }

            if (TempData["msg"] != null)
            {
                ViewBag.Msg = TempData["msg"];
            }

            return View();
        }


            
        #endregion

        #region Index Post
    
        [ValidateAntiForgeryToken, HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            string username = (collection["username"] != null ? collection["username"] : "");
            string password = (collection["password"] != null ? collection["password"] : "");


            Users user;
            IEnumerable<UserSessions> session;


            if (Core.Login.IsLogin())
                return RedirectToAction("Index", "");

            try
            {
                CoreSettings settings = db.CoreSettings.First();

                if (settings.LoginType == 2)
                {
                    ViewBag.IsWindows = 1;
                }

                var browser = Request.Browser;


                if (settings.LoginType == 0) // application 
                {
                    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                    {
                        ViewBag.Msg = new string[] { "0", Resources.Resources.LoginError };
                        return View();
                    }

                    user = db.Users.FirstOrDefault(a => a.UserName == username);
                    if (user != null)
                    {
                        DateTime expiryDate = Convert.ToDateTime(user.LastPasswordChanged).AddDays(Convert.ToDouble(user.PasswordExpiry));
                        if (user.PasswordExpiry == null || user.PasswordExpiry == 0 || user.LastPasswordChanged == null || expiryDate < DateTime.UtcNow)
                        {
                            TempData["userid"] = user.Id;
                            Core.Basic.LogEntryToDb(Core.LogTypes.Login, "Login attempt for user name: " + user.UserName + " was unsuccessful due to password change required" + Environment.NewLine +
                                "From Ip: " + Request.UserHostAddress + Environment.NewLine +
                                "Browser: " + browser.Type + " " + browser.Browser + " " + browser.Version + Environment.NewLine +
                                "Platform: " + browser.Platform);

                            TempData["msg"] = new string[] { "2", Resources.Resources.LoginChangePasswordWarning };

                            return RedirectToAction("ChangePassword");
                        }
                        if (user.MaxNoOfAttempts != null && user.MaxNoOfAttempts != 0 && user.MaxNoOfAttempts <= user.Attempts)  // checks if the max no of attempts are greater then attempts (0 mean don't check it)
                        {
                            Core.Basic.LogEntryToDb(Core.LogTypes.Login, "Login attempt for user name: " + user.UserName + " was unsuccessful due to max no of attempts exhausted" + Environment.NewLine +
                                "From Ip: " + Request.UserHostAddress + Environment.NewLine +
                                "Browser: " + browser.Type + " " + browser.Browser + " " + browser.Version + Environment.NewLine +
                                "Platform: " + browser.Platform);
                            if (user.Attempts == null)
                                user.Attempts = 0;
                            user.Attempts = user.Attempts + 1;
                            db.SaveChanges();
                            ViewBag.Msg = new string[] { "0", Resources.Resources.LoginMaxNoOfAttempts };
                            return View();
                        }

                        string pass = Core.Basic.GetHashPassword(password, user.Salt);
                        if (user.Password != pass)
                        {
                            if (user.Attempts == null)
                                user.Attempts = 0;

                            user.Attempts = user.Attempts + 1;
                            user.ModifiedDate = DateTime.UtcNow;
                            db.SaveChanges();

                            Core.Basic.LogEntryToDb(Core.LogTypes.Login, "Login attempt for user name: " + user.UserName + " was unsuccessful due to incorrect password" + Environment.NewLine +
                                "From Ip: " + Request.UserHostAddress + Environment.NewLine +
                                "Browser: " + browser.Type + " " + browser.Browser + " " + browser.Version + Environment.NewLine +
                                "Platform: " + browser.Platform);

                            ViewBag.Msg = new string[] { "0", Resources.Resources.LoginUserDoesNotExist };
                            return View();
                        }
                    }
                    else
                    {
                        Core.Basic.LogEntryToDb(Core.LogTypes.Login, "Login attempt for user name: " + user + " was unsuccessful due to incorrect user name" + Environment.NewLine +
                                "From Ip: " + Request.UserHostAddress + Environment.NewLine +
                                "Browser: " + browser.Type + " " + browser.Browser + " " + browser.Version + Environment.NewLine +
                                "Platform: " + browser.Platform);

                        ViewBag.Msg = new string[] { "0", Resources.Resources.LoginUserDoesNotExist };
                        return View();
                    }


                }
                else if (settings.LoginType == 1) // active directory 
                {
                    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                    {
                        ViewBag.Msg = new string[] { "0", Resources.Resources.LoginError };
                        return View();
                    }

                    string domain = "";
                    try
                    {
                        domain = WebConfigurationManager.AppSettings["DomainName"];
                        if (string.IsNullOrEmpty(domain))
                        {
                            domain = "";
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
                    using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, domain))
                    {
                        if (pc.ValidateCredentials(username, password) == false)
                        {
                            Core.Basic.LogEntryToDb(Core.LogTypes.Login, "Login attempt for user name: " + username + " (domain level) was unsuccessful due to incorrect user name or password" + Environment.NewLine +
                               "From Ip: " + Request.UserHostAddress + Environment.NewLine +
                               "Browser: " + browser.Type + " " + browser.Browser + " " + browser.Version + Environment.NewLine +
                               "Platform: " + browser.Platform);

                            ViewBag.Msg = new string[] { "0", Resources.Resources.LoginUserDoesNotExistOrDisabled };
                            return View();
                        }
                    }

                    user = db.Users.FirstOrDefault(a => a.UserName == username);
                    if (user == null)
                    {
                        Core.Basic.LogEntryToDb(Core.LogTypes.Login, "Login attempt for user name: " + user.UserName + " was unsuccessful as account does not exist" + Environment.NewLine +
                              "From Ip: " + Request.UserHostAddress + Environment.NewLine +
                              "Browser: " + browser.Type + " " + browser.Browser + " " + browser.Version + Environment.NewLine +
                              "Platform: " + browser.Platform);

                        ViewBag.Msg = new string[] { "0", Resources.Resources.LoginUserDoesNotExist };
                        return View();
                    }

                }
                else // windows authentication
                {
                    username = User.Identity.Name;
                    if (username != null)
                        user = db.Users.FirstOrDefault(a => a.UserName == username);
                    else
                        user = null;

                }

                if (user != null)
                {

                    if (user.Status != 1)
                    {

                        Core.Basic.LogEntryToDb(Core.LogTypes.Login, "Login attempt for user name: " + user.UserName + " was unsuccessful due to account status not enabled" + Environment.NewLine +
                              "From Ip: " + Request.UserHostAddress + Environment.NewLine +
                              "Browser: " + browser.Type + " " + browser.Browser + " " + browser.Version + Environment.NewLine +
                              "Platform: " + browser.Platform);

                        ViewBag.Msg = new string[] { "0", Resources.Resources.LoginStatusNotEnabled };
                        return View();
                    }
                    session = db.UserSessions.Where(a => a.UserId == user.Id && a.Status != true && a.SessionCode != Session.SessionID);
                    if (session != null && session.Count() > 0 && settings.IsMultiLogin == false) //checks if session tracking is open and single login is active
                    {
                        if (settings.IsSessionExpire == true && user.SessionExpireTime != 0) //if session expiry is enabled for user
                            session = session.Where(a => a.SessionActiveDate > DateTime.UtcNow);

                        if (session != null && session.Count() > 0)
                        {

                            TempData["userIDTemp"] = user.Id; //this is for session() control

                            ViewBag.Msg = new string[] { "0", Resources.Resources.LoginSessionExist };

                            if (settings.IsSessionSignOffWithUser == true) //check if user is allowed to kill previous session
                                ViewBag.IswithUser = 1;

                            Core.Basic.LogEntryToDb(Core.LogTypes.Login, "Login attempt for user name: " + user.UserName + " was unsuccessful due to active session(s)" + Environment.NewLine +
                            "From Ip: " + Request.UserHostAddress + Environment.NewLine +
                            "Browser: " + browser.Type + " " + browser.Browser + " " + browser.Version + Environment.NewLine +
                            "Platform: " + browser.Platform);

                            return View();
                        }
                    }

                    db.UserSessions.Add(new UserSessions()
                    {
                        SessionCode = Session.SessionID,
                        SessionActiveDate = DateTime.UtcNow.AddMinutes(Convert.ToDouble(user.SessionExpireTime)),
                        Status = false,
                        CreatedDate = DateTime.UtcNow,
                        UserId = user.Id,
                        SessionInfo = ("Ip: " + Request.UserHostAddress + Environment.NewLine +
                            "Browser: " + browser.Type + " " + browser.Browser + " " + browser.Version + Environment.NewLine +
                            "Platform: " + browser.Platform)
                    });

                    if (user.Attempts > 0)
                    {
                        user.Attempts = 0;
                        TempData["msg"] = new string[] { "0", (Resources.Resources.IsLeft == "1" ? Resources.Resources.LoginTriedToLogin + Basic.UtcDisplay(user.ModifiedDate) : Basic.UtcDisplay(user.ModifiedDate) + Resources.Resources.LoginTriedToLogin) };

                    }

                    db.SaveChanges();

                    Core.Login.LoginUser(user.Id, user.Organizations.Id, user.UserName, user.FirstName, user.LastName, Convert.ToInt32(user.SessionExpireTime));

                    Core.Basic.LogEntryToDb(Core.LogTypes.Login, "User name: " + user.UserName + " was successfully logged in with " + Environment.NewLine +
                        "Session Id: " + Session.SessionID + Environment.NewLine +
                        "From Ip: " + Request.UserHostAddress + Environment.NewLine +
                        "Browser: " + browser.Type + " " + browser.Browser + " " + browser.Version + Environment.NewLine +
                        "Platform: " + browser.Platform);


                    var red = Request.QueryString["redirect"];

                    if (red == null)
                    {
                        //FCONFIGEntities fc = new FCONFIGEntities();
                        //bool exists = fc.FConfigInsertStatus.Any(x => x.UserId == Login.Id);
                        //if (!exists)
                        //{
                        //   //Added Default Master Configurations for the Login UserId and Orgid                       
                        //        var returnint = fc.FrameWorkConfig(Login.OrganizationId, Login.Id);
                        //        FConfigInsertStatu data = new FConfigInsertStatu();
                        //        data.UserId = Login.Id;
                        //        data.Status = true;
                        //        data.ModifiedDate = DateTime.Now;
                        //        fc.FConfigInsertStatus.Add(data);
                        //        fc.SaveChanges();
                        //        db.SaveChanges();

                        //}


                        //to be removed after proper code implemenation
                        var UserRole = db.Roles.Where(x => x.OrgId == Login.OrganizationId && x.Name.Contains("Admin")).FirstOrDefault();
                        var userroles = db.AssignRoletoUsers;
                        var Admin = userroles.Where(a => a.userId == user.Id && a.RoleId == UserRole.Id).FirstOrDefault();
                        if (Admin != null)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        var Customer_Service_Agent = userroles.Where(a => a.userId == user.Id && a.RoleId == 7).FirstOrDefault();
                        if (Customer_Service_Agent != null)
                        {
                            return RedirectToAction("Index", "CaseManagment");
                        }
                        var Customer_Service_Manager = userroles.Where(a => a.userId == user.Id && a.RoleId == 8).FirstOrDefault();
                        if (Customer_Service_Manager != null)
                        {
                            return RedirectToAction("Index", "CaseManagment");
                        }
                        var Sales_Representative = userroles.Where(a => a.userId == user.Id && a.RoleId == 9).FirstOrDefault();
                        if (Sales_Representative != null)
                        {
                            return RedirectToAction("Index", "CaseManagment");
                        }
                        var Sales_Manager = userroles.Where(a => a.userId == user.Id && a.RoleId == 10).FirstOrDefault();
                        if (Sales_Manager != null)
                        {
                            return RedirectToAction("Index", "CaseManagment");
                        }
                        var Campaign_Manager = userroles.Where(a => a.userId == user.Id && a.RoleId == 11).FirstOrDefault();
                        if (Campaign_Manager != null)
                        {
                            return RedirectToAction("CampaignList", "CampaignManagment");
                        }

                        return RedirectToAction("Index", "Home");
                    }
                    else
                        return Redirect(red);

                }
                else
                    ViewBag.Msg = new string[] { "0", Resources.Resources.GeneralMsgPleaseTryLater };


            }
            catch (Exception ex)
            {
                Core.Basic.WriteLog("Login_Index_43", ex.Message, ex.ToString());
                ViewBag.Msg = new string[] { "0", Resources.Resources.GeneralMsgPleaseTryLater };
                return View();
            }

            return View();
        }
        #endregion 
  
        #region Permission
        public ActionResult Permission()
        {

            return View();
        }
        #endregion

        #region Keep
     
        public string Keep()
        {
            try
            {
                FrameworkEntities db = new FrameworkEntities();
                var user = db.Users.Find(Login.Id);
                var session = db.UserSessions.FirstOrDefault(a => a.UserId == Login.Id && a.SessionCode == Session.SessionID && a.SessionActiveDate > DateTime.UtcNow && a.Status == false);
                if (session != null)
                {

                    session.SessionActiveDate = session.SessionActiveDate.AddMinutes(Convert.ToDouble(user.SessionExpireTime));
                }
                else
                    return "nooo";

                db.SaveChanges();

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

            return "ok";
        }
        #endregion

        #region Sessions
       
        public ActionResult Sessions()
        {
            try
            {
                CoreSettings settings = db.CoreSettings.First();

                if (TempData["userIDTemp"] != null && settings.IsSessionSignOffWithUser == true)
                {
                    int Id = Convert.ToInt32(TempData["userIDTemp"]);
                    if (Id > 0)
                    {
                        var data = db.UserSessions.Where(a => a.UserId == Id && a.Status != true);
                        foreach (var item in data)
                        {
                            item.Status = true;
                        }
                        db.SaveChanges();
                        TempData["msg"] = new string[] { "1", Resources.Resources.LoginSessionKilled };
                        return RedirectToAction("Index");
                    }
                }

            }
            catch (Exception ex)
            {
                Core.Basic.WriteLog("Login_Session_227", ex.Message, ex.ToString());
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion
            }

            TempData["msg"] = new string[] { "0", Resources.Resources.LoginSessionKilledFailed };
            return RedirectToAction("Index");
        }
        #endregion

        #region Change Password get
         
        public ActionResult ChangePassword()
        {
            try
            {
                if (TempData["msg"] != null)
                    ViewBag.Msg = TempData["msg"];

                if (TempData["userid"] != null)
                {
                    var user = TempData["userid"];
                    TempData["userids"] = user;
                }
                else
                {
                    TempData["msg"] = new string[] { "0", Resources.Resources.GeneralMsgPleaseTryLater };
                    return RedirectToAction("Index");
                }

                ViewBag.PasswordChange = 1;
                return View("Index");
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
            return RedirectToAction("Index");
        }
        #endregion

           #region Change Password Post
          

           [ValidateAntiForgeryToken, HttpPost]
           public ActionResult ChangePassword(FormCollection collection)
           {
               var password = collection["password"];

               ViewBag.PasswordChange = 1;

               if (TempData["msg"] != null)
                   ViewBag.Msg = TempData["msg"];

               if (TempData["userids"] != null)
               {
                   var user = TempData["userids"];
                   try
                   {
                       CoreSettings settings = db.CoreSettings.First();

                       var data = db.Users.Find(Convert.ToInt32(user));
                       if (data != null)
                       {

                           if (settings.NoOfPasswordHistroy != null && settings.NoOfPasswordHistroy != 0)
                           {
                               IQueryable<UserPasswordHistories> history = db.UserPasswordHistories.Where(a => a.UserId == data.Id).OrderBy(a => a.Id);

                               if (history != null && history.Count() > 0)
                               {

                                   history = history.Skip(Math.Max(0, history.Count() - Convert.ToInt32(settings.NoOfPasswordHistroy)));

                                   foreach (var item in history)
                                   {
                                       if (item.Password == Basic.GetHashPassword(password, item.Salt))
                                       {
                                           TempData["userids"] = user;
                                           ViewBag.Msg = new string[] { "0", Resources.Resources.LoginPasswordAlreadyUsed };
                                           return View("Index");
                                       }
                                   }

                               }
                           }
                           data.Salt = Basic.GetSalt();
                           data.Password = Basic.GetHashPassword(password, data.Salt);
                           data.LastPasswordChanged = DateTime.UtcNow;
                           data.Status = 1;
                           data.PasswordExpiry = data.PasswordExpiry == null ? settings.PasswordExpiry : data.PasswordExpiry;
                           data.ModifiedDate = DateTime.UtcNow;
                           db.UserPasswordHistories.Add(new UserPasswordHistories() { CreatedDate = DateTime.UtcNow, Password = data.Password, Salt = data.Salt, UserId = Convert.ToInt32(user) });
                           db.SaveChanges();
                           TempData["msg"] = new string[] { "1", Resources.Resources.LoginPasswordChanged };
                           ViewBag.PasswordChange = 0;
                           return RedirectToAction("Index");
                       }

                   }
                   catch (Exception ex)
                   {
                       TempData["userids"] = user;
                       ViewBag.PasswordChange = 1;
                       Core.Basic.WriteLog("Login_ChangePassword_427", ex.Message, ex.ToString());
                       ViewBag.Msg = new string[] { "0", Resources.Resources.GeneralMsgPleaseTryLater };
                       #region Exception Logging
                       var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                       var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                       if (!Directory.Exists(dir))  // if it doesn't exist, create
                           Directory.CreateDirectory(dir);
                       System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                       #endregion
                   }

               }
               else
               {
                   TempData["msg"] = new string[] { "0", Resources.Resources.GeneralMsgPleaseTryLater };
                   return RedirectToAction("Index");
               }


               return View("Index");
           }
           #endregion



       

    }
}