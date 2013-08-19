using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskTimer.Models;

namespace TaskTimer.Areas.Admin.Controllers
{
    public class AdminHomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SitePages()
        {
            var db = new CustomMembershipDB();
            return View(db.SiteMainPages);
        }
        public ActionResult SitePagesAddNew()
        {
            return View(new SiteMainPages());
        }
        [HttpPost]
        public ActionResult SitePagesAddNew(SiteMainPages newPage)
        {
            var db = new CustomMembershipDB();
            db.SiteMainPages.Add(newPage);
            db.SaveChanges();
            var file = Request.Form["TitleImage"];
            return RedirectToAction("SitePages");
        }
    }
}
