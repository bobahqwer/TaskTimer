using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaskTimer.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult AccessDenied()
        {
            return View("AccessDenied");
        }

        public ActionResult InvalidRequest()
        {
            return View("InvalidRequest");
        }

        public ActionResult NotFound()
        {
            return View("NotFound");
        }

        public ActionResult ServerError()
        {
            return View("ServerError");
        }
    }
}
