using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskTimer.Models;

namespace TaskTimer.Areas.Efargan.co.il.Controllers
{
    public class PagesController : Controller
    {
        //
        // GET: /Efargan.co.il/Pages/

        public ActionResult Index(int id)
        {
            var db = new CustomMembershipDB();
            var curPage = db.EfarganPages.SingleOrDefault(p => p.Id == id);
            if (curPage == null)
                return View(new EfarganPages());
            return View(curPage);
        }

    }
}
