﻿using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FramWork
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //if (true)
            //{
            //    routes.MapRoute(
            //                 name: "language",
            //                 url: "{lang}/{controller}/{action}/{id}",
            //                 defaults: new { lang = "en", controller = "Default", action = "Index", id = UrlParameter.Optional }
            //             );
            //}
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}