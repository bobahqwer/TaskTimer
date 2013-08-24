using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskTimer.Models;

namespace TaskTimer.Controllers
{
    public class GalleryController : Controller
    {
        //
        // GET: /Gallery/

        public ActionResult Index(int id)
        {
            var db = new CustomMembershipDB();
            var page = db.GalleryPages.FirstOrDefault(g => g.Id == id);
            if (page == null)
                page = new GalleryPages();
            return View(page);
        }

    }
}
