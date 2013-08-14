using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace TaskTimer
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //robots file
            routes.MapRoute("Robots",
                "robots.txt",
                new { controller = "Home", action = "Robots" });

            //site map
            routes.MapRoute("Sitemap",
                "Sitemap.xml",
                new { controller = "Home", action = "gSiteMap" });

            routes.MapRoute("OAuth",
                "oauth2callback",
                new { controller = "Configuration", action = "AuthorizationCheckAuto" });

            //routes.MapRoute("Mvc.sitemap",
            //   "Mvc.sitemap",
            //   new { controller = "Home", action = "Robots" });
            //activation
            routes.MapRoute(
                 "Activate",
                 "Account/Activate/{username}/{key}",
                 new
                 {
                     controller = "Account",
                     action = "Activate",
                     username = UrlParameter.Optional,
                     key = UrlParameter.Optional
                 });
            //Default
            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = "" });

//            routes.MapRoute(
//    "Default",
//   "{language}/{controller}/{action}/{id}",
//new { language = "en", controller = "Home", action = "Index", id = "" }
//);

           

            //routes.MapRoute(
            //      "Activate",
            //      "Account/Activate/{username}/{key}",
            //      new
            //      {
            //          controller = "Account",
            //          action = "Activate",
            //          username = UrlParameter.Optional,
            //          key = UrlParameter.Optional
            //      });
        }
    }
}