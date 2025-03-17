using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Core
{
    public class Login
    {
        public static int Id
        {
            get { return (IsLogin() == true ? Convert.ToInt32(HttpContext.Current.Session["AdminId"]) : 0); }
            private set { HttpContext.Current.Session["AdminId"] = value; }
        }

        public static Guid OrganizationId
        {
            get { return (IsLogin() == true ? new Guid(HttpContext.Current.Session["OrgId"].ToString()) : new Guid()); }
            private set { HttpContext.Current.Session["OrgId"] = value; }
        }
        public static string FirstName
        {
            get { return (IsLogin() == true ? HttpContext.Current.Session["AdminFname"].ToString() : ""); }
            private set { HttpContext.Current.Session["AdminFname"] = value; }
        }
        public static string LastName
        {
            get { return (IsLogin() == true ? HttpContext.Current.Session["AdminLname"].ToString() : ""); }
            private set { HttpContext.Current.Session["AdminLname"] = value; }
        }

        public static string Username
        {
            get { return (IsLogin() == true ? HttpContext.Current.Session["AdminUsername"].ToString() : ""); }
            private set { HttpContext.Current.Session["AdminUsername"] = value; }
        }

        public static bool IsLogin()
        {
            if (HttpContext.Current.Session["AdminId"] != null && HttpContext.Current.Session["AdminUsername"] != null)
            {
                return true;
            }
            else return false;
        }

        public static int SessionTimeout
        {
            get { return (IsLogin() == true ? Convert.ToInt32(HttpContext.Current.Session["AdminSession"]) : 0); }
            private set { HttpContext.Current.Session["AdminSession"] = value; }
        }

        public static bool Logout()
        {
            HttpContext.Current.Session.Clear();
            //HttpContext.Current.Session.Abandon();
            if (HttpContext.Current.Session["AdminId"] == null)
                return true;
            else return false;
        }

        public static void LoginUser(int Id, Guid OrganizationId, string Username, string FirstName, string LastName, int SessionTimeout = 0)
        {
            Login.Id = Id;
            Login.OrganizationId = OrganizationId;
            Login.FirstName = FirstName;
            Login.LastName = LastName;
            Login.Username = Username;
            Login.SessionTimeout = SessionTimeout;

        }
        

    }

}
