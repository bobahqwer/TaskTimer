using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskTimer.Models;

namespace TaskTimer.Controllers
{
    public class SitePagesController : Controller
    {
        //
        // GET: /SitePages/

        public ActionResult Index(int id)
        {
            var db = new CustomMembershipDB();
            var page = db.SiteMainPages.FirstOrDefault(p => p.Id == id);
            if (page == null)
                page = new SiteMainPages();
            return View(page);
        }

    }
}
