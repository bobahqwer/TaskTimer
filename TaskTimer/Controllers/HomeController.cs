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
        public ActionResult Facebook()
        {
            return Redirect("https://apps.facebook.com/testappqwerqwer/?code=AQCpZEP7s8TZpfY17TIV4G2WDcq2gyVU1cCg17jdq-Yo6IjRTCau7VC_K4HOjFaw4gyiLvIuuCow2YjpKO4nkxQKDo27hAwrcEBqxhbbpw8Nwst4e6x5l3QBEt4kBh31jmp_kYPyQ00lgeJLo60hoUfKQ-s_RmiUEjYBHg0xuUFEHaqFPqDBsUpbAINYDfcRK9SVYDj7ZRTJBUx7QtrDPC5j2VtAksS8SZbGVRGRvO1T7buTxyLEq2iT4oHhpgsKpT9UBxLFjH9hH4TxJsvoERH_-Zad3fuTarxA0rAz_DTUS83uTUJARG5Ryl3FQd7w8jI#_=_");
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
