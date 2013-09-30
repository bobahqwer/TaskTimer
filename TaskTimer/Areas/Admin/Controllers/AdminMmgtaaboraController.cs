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

            return View();
        }

        public ActionResult PagesEdit() 
        {
            return View();
        }
    }
}
