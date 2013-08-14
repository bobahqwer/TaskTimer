using MvcSiteMapProvider.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using TaskTimer.Controllers;
using TaskTimer.Models;
using TaskTimer.UsefulClasses;

namespace TaskTimer
{
    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            XmlSiteMapController.RegisterRoutes(RouteTable.Routes); // <-- register sitemap.xml, add this line of code
            //RegisterRoutes(RouteTable.Routes);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Helper.CheckDirectorysOnServer(new string[] { Server.MapPath("~/Images/Uploaded/"), Server.MapPath("~/Images/Uploaded/Temporary/") });
            Application["onlinevisitors"] = 0;
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
            Session["start"] = DateTime.Now;
            Application.Lock();
            Application["onlinevisitors"] = Convert.ToInt32(Application["onlinevisitors"]) + 1;
            Application.UnLock();

            // get total visits
            var appID = Guid.Parse(ConfigurationManager.AppSettings["ApplicationId"]);
            using (var db = new CustomMembershipDB())
            {
                var curApp = db.Applications.FirstOrDefault(app => app.ApplicationId == appID);
                if (curApp != null) 
                {
                    curApp.UserCounter++;
                    db.SaveChanges();
                    Application["totalvisitors"] = curApp.UserCounter;
                    Application["totalusers"] = db.Users.Count();
                }
            } 
        }

        void Application_Error(object sender, EventArgs e)
        {
            GlobalErrorHandler.HandleError(((HttpApplication)sender).Context, Server.GetLastError(), new ErrorController());
        }
        
        void Session_End(object sender, EventArgs e)
        {
            //FormsAuthentication.SignOut();
        }
        
        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            var langCookie = HttpContext.Current.Request.Cookies["lang"];
            if (langCookie != null)
            {
                var ci = new CultureInfo(langCookie.Value);
                //Checking first if there is no value in session 
                //and set default language 
                //this can happen for first user's request
                if (ci == null)
                {
                    //Sets default culture to english invariant
                    string langName = "en";

                    //Try to get values from Accept lang HTTP header
                    if (HttpContext.Current.Request.UserLanguages != null && HttpContext.Current.Request.UserLanguages.Length != 0)
                    {
                        //Gets accepted list 
                        langName = HttpContext.Current.Request.UserLanguages[0].Substring(0, 2);
                    }

                    langCookie = new HttpCookie("lang", langName)
                    {
                        HttpOnly = true
                    };
                    HttpContext.Current.Response.AppendCookie(langCookie);
                }

                //Finally setting culture for each request
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
            }
        }
        public static void RegisterRoutes(RouteCollection routes)
        {
            // ... your routes registration here
        }
    }

}