using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskTimer.Models;

namespace TaskTimer.Areas.Admin.Models
{
    public class AdminMmgtaaboraController : Controller
    {
        //
        // GET: /Admin/AdminMmgtaabora/

        public ActionResult Pages()
        {
            return View((new CustomMembershipDB()).MmgtaaboraPages);
        }

        public ActionResult PagesAddNew()
        {
            //var newPage = new EfarganPages();
            //newPage.EfarganPagesSliders.Add(new EfarganPagesSliders());
            return View(new MmgtaaboraPages());
        }
        [HttpPost]
        public ActionResult PagesAddNew(MmgtaaboraPages newPage)
        {
            if (ModelState.IsValid)
            {
                var db = new CustomMembershipDB();
                var newSavedPage = db.MmgtaaboraPages.Add(newPage);
                db.SaveChanges();
                return RedirectToAction("PagesEdit", new { id = newSavedPage.Id });
            }
            return View();
        }

        public ActionResult PagesEdit() 
        {
            return View();
        }
    }
}
