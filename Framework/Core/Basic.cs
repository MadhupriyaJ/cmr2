using FramWork.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Core
{
    public class Basic
    {
        private static string SecretKey = "Symbiosis123#@!goingtoSchool11?";
        public static string getCurrentLanguage()
        {
            string url = HttpContext.Current.Request.RawUrl;
            RouteData route = RouteTable.Routes.GetRouteData(new HttpContextWrapper(System.Web.HttpContext.Current));
            UrlHelper urlHelper = new UrlHelper(new RequestContext(new HttpContextWrapper(System.Web.HttpContext.Current), route));

            var routeValueDictionary = urlHelper.RequestContext.RouteData.Values;

            return routeValueDictionary["lang"].ToString().ToLower();

        }


        public static string Encrypt(string clearText)
        {
            try
            {
                var encryptionKey = SecretKey;
                var clearBytes = Encoding.Unicode.GetBytes(clearText);
                using (var encryptor = Aes.Create())
                {
                    var pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        clearText = Convert.ToBase64String(ms.ToArray());
                    }
                }
                return clearText;
            }
            catch
            {

                return "";
            }
        }

        public static string Decrypt(string cipherText)
        {
            try
            {
                var encryptionKey = SecretKey;
                var cipherBytes = Convert.FromBase64String(cipherText);
                using (var encryptor = Aes.Create())
                {
                    var pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
            }
            catch
            {

                return "";
            }
        }


        public static Boolean WriteLog(string FunctionName, string ShortMsg, string Message)
        {

            try
            {

                string format = ConfigurationManager.AppSettings["LogType"];
                string splitSize = ConfigurationManager.AppSettings["LogSplitSize"];
                string isSplit = ConfigurationManager.AppSettings["LogSplit"];

                try
                {
                    if (isSplit != null && isSplit == "true")
                    {
                        var fileName = AppDomain.CurrentDomain.BaseDirectory + @"log/" + DateTime.Now.ToString("yyyyMMdd") + "." + format;
                        FileInfo fi = new FileInfo(fileName);
                        if (fi.Length > Convert.ToInt32(splitSize) && Convert.ToInt32(splitSize) > 0)
                            System.IO.File.Move(fileName, AppDomain.CurrentDomain.BaseDirectory + @"log/" + DateTime.Now.ToString("yyyyMMddHHmmss") + "." + format);
                    }
                }
                catch
                {

                }

                StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"log/" + DateTime.Now.ToString("yyyyMMdd") + "." + format, true);

                if (format == "txt")
                    sw.WriteLine("function name: " + FunctionName + Environment.NewLine + "Date: " + DateTime.Now + Environment.NewLine + "Short message:" + Environment.NewLine + ShortMsg + Environment.NewLine + "long message:" + Environment.NewLine + Message + Environment.NewLine + Environment.NewLine);
                else
                    sw.WriteLine("<message><functionname>" + FunctionName + "</functionname><date>" + DateTime.Now + "</date><shortmsg>" + ShortMsg + "</shortmsg></longmsg>" + Message + "</longmsg></message>");
                sw.Close();

                return true;
            }
            catch
            {
            }
            return false;

        }


        public static Boolean LogEntryToDb(LogTypes type, string Message)
        {
            try
            {
                FrameworkEntities Db = new FrameworkEntities();
                Logs data = new Logs();
                data.CreatedDate = DateTime.UtcNow;
                data.Description = Message;
                data.Type = type.ToString();

                if (Login.Id != 0)
                {
                    data.CreatedBy = Login.Id;
                }


                Db.Logs.Add(data);
                Db.SaveChanges();

                return true;
            }
            catch
            {
            }
            return false;

        }

        public static Boolean LogEntryToDb(string type, string Message)
        {
            try
            {
                FrameworkEntities Db = new FrameworkEntities();
                Logs data = new Logs();
                data.CreatedDate = DateTime.UtcNow;
                data.Description = Message;
                data.Type = type;

                if (Login.Id != 0)
                {
                    data.CreatedBy = Login.Id;
                }


                Db.Logs.Add(data);
                Db.SaveChanges();

                return true;
            }
            catch
            {

            }
            return false;

        }

        public static Boolean AddNotification(string title, string message, byte isSystemMessage, string url, int userId = 0)
        {
            try
            {
                FrameworkEntities Db = new FrameworkEntities();
                Db.Notifications.Add(new Notifications()
                {
                    Title = title,
                    ShortMsg = message,
                    Type = isSystemMessage,
                    CreatedDate = DateTime.UtcNow,
                    Status = true,
                    Url = url,
                    UserId = (userId == 0 ? (int?)null : userId)
                });
                Db.SaveChanges();

                return true;
            }
            catch
            {
            }

            return false;

        }

        static public void SendMessage(string toAddress, string subjectText, string bodyText)
        {
            if (string.IsNullOrEmpty(toAddress))
                return;

            try
            {
                //  FrameworkEntities Db = new FrameworkEntities();
                if (string.IsNullOrEmpty("") || string.IsNullOrEmpty("") || string.IsNullOrEmpty(""))
                    return;

                ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                MailMessage newMessage = new MailMessage();

                //Create addresses
                MailAddress senderAddress = new MailAddress("");
                MailAddress recipentAddress = new MailAddress(toAddress);

                //Now create the full message
                newMessage.To.Add(recipentAddress);
                newMessage.From = senderAddress;
                newMessage.Subject = subjectText;
                newMessage.Body = bodyText;

                //Create the SMTP Client object, which do the actual sending
                SmtpClient client = new SmtpClient("", Convert.ToInt32(0))
                {
                    Credentials = new NetworkCredential("", Basic.Decrypt("")),
                    EnableSsl = true
                };


                //now send the message
                client.Send(newMessage);
            }
            catch
            {


            }
        }

        public static string GetHashPassword(string Password, string salt)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(salt + Password + salt.Reverse()));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

        public static string GetHashPassword(string Password)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(Password));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

        public static string GetSalt(int saltSize = 40)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random(Guid.NewGuid().GetHashCode());
            var result = new string(Enumerable.Repeat(chars, saltSize).Select(s => s[random.Next(s.Length)]).ToArray());

            return result;
        }

        public static DateTime? UtcDisplay(DateTime? date)
        {
            return date;
        }

        public static void TimeLineDescription(string ControllerName, string ViewName, string Description)
        {

            using (FrameworkEntities dc=new FrameworkEntities())
            {
                TimeLine tbl = new TimeLine
                {
                    OrgId = Login.OrganizationId,
                    ControllerName = ControllerName,
                    Viewname = ViewName,
                    Description = Description,
                    CreateDate =DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    ModifiedBy=Login.Id

                };
                dc.TimeLine.Add(tbl);
                dc.SaveChanges();
            }

           
        }
  
    }

    public enum LogTypes
    {
        Add = 0,
        Modify = 1,
        Delete = 2,
        Login = 3,
        Logout = 4,
        System = 5
    }

    public enum DealStagedef
    {
        won = 1,
        lost = 2,
        dropped = 3,
        Lead = 4,
        Qualify = 5,
        backOffice = 6
    }

    public enum GoalType
    {
        Organisation_Level_Goal=1,
        Sales_Team_Level_Goal=2,
        Agent_Level_Goal=3
    }


}