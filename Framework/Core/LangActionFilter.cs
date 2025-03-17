using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Core
{
    public class LangActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (true)
            {

                string language = Basic.getCurrentLanguage();

                if (language == "ar")
                {
                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ar-AE");
                    Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

                }
                else if (language == "en")
                {
                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                    Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
                }
                else
                {
                    throw new HttpException(404, "Page Not Found");
                }
            }
        }

    }
}