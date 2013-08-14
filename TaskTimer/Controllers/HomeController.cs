using MvcSiteMapProvider.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaskTimer.Controllers
{
    //[Localize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Site features.";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "My contact page.";
            return View();
        }
        public ActionResult Welcome()
        {
            return View();
        }
        
        [Authorize(Roles = "Admin")]
        public ActionResult Admin()
        {
            ViewBag.Message = "Your Admin page.";
            return View();
        }

        #region-- Site Map --
        public ActionResult gSiteMap()
        {
            return new XmlSiteMapResult();
        }
        #endregion

        #region -- Robots() Method --
        public ActionResult Robots()
        {
            Response.ContentType = "text/plain";
            return View();
        }
        #endregion

        #region -- Site Map() Method --
        public ActionResult SiteMap()
        {
            ViewBag.Message = "Site map page.";
            return View();
        }
        #endregion

        #region -- Localisation --
        public ActionResult ChangeCulture(string lang, string returnUrl)
        {
            var langCookie = new HttpCookie("lang", lang)
            {
                HttpOnly = true
            };
            Response.AppendCookie(langCookie);
            return Redirect(returnUrl);
        }
        #endregion
        
    }
}
