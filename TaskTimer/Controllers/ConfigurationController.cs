using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Google.GData.Client;
using Google.GData.Spreadsheets;
using TaskTimer.Models;

namespace TaskTimer.Controllers
{
    public class ConfigurationController : Controller
    {
        static public GoogleDocumentModel GoogleDocuments = null;
        // GET: /Configuration/
        //[Authorize(Roles = "Admin, SuperUser")] 
        public ActionResult Index(GoogleAPIAccess model)
        {
            ViewBag.Message = "Add / Update / Remove Google documents";
            if (model.AccessCode != null)  //after authorization 
            {
                
                ////////////////////////////////////////////////////////////////////////////
                // STEP 5: Make an OAuth authorized request to Google
                ////////////////////////////////////////////////////////////////////////////

                // Initialize the variables needed to make the request
                GOAuth2RequestFactory requestFactory =
                    new GOAuth2RequestFactory(null, "TaskTimer", model);
                SpreadsheetsService service = new SpreadsheetsService("TaskTimer");
                service.RequestFactory = requestFactory;


                // Instantiate a SpreadsheetQuery object to retrieve spreadsheets.
                SpreadsheetQuery query = new SpreadsheetQuery();

                // Make a request to the API and get all spreadsheets.
                SpreadsheetFeed feed = service.Query(query);

                //GoogleDocuments = new GoogleDocumentModel();
                //GoogleDocuments.GoogleAPIAccess = model;
                GoogleDocuments.Documents = feed.Entries;
            }
            return View(GoogleDocuments);
        }

        //[HttpGet, ActionName("Index")]
        //public PartialViewResult IndexGet(GoogleAPIAccess model)
        //{
        //    ViewBag.Message = "Add / Update / Remove Google documents";
        //    if (model.AccessCode != null)  //after authorization 
        //    {

        //        ////////////////////////////////////////////////////////////////////////////
        //        // STEP 5: Make an OAuth authorized request to Google
        //        ////////////////////////////////////////////////////////////////////////////

        //        // Initialize the variables needed to make the request
        //        GOAuth2RequestFactory requestFactory =
        //            new GOAuth2RequestFactory(null, "TaskTimer", model);
        //        SpreadsheetsService service = new SpreadsheetsService("TaskTimer");
        //        service.RequestFactory = requestFactory;


        //        // Instantiate a SpreadsheetQuery object to retrieve spreadsheets.
        //        SpreadsheetQuery query = new SpreadsheetQuery();

        //        // Make a request to the API and get all spreadsheets.
        //        SpreadsheetFeed feed = service.Query(query);

        //        //GoogleDocuments = new GoogleDocumentModel();
        //        //GoogleDocuments.GoogleAPIAccess = model;
        //        GoogleDocuments.Documents = feed.Entries;
        //    }
            
            
        //    return PartialView("_IndexPartual", GoogleDocuments);
        //}

        public ActionResult AddNew(string errorMsg) 
        {
            ViewBag.Message = "Add new Google documents";
            if (!string.IsNullOrEmpty(errorMsg))
                ModelState.AddModelError("", errorMsg);
            return View();
        }

        [HttpPost]
        public ActionResult AddNew(GoogleAPIAccess model)
        {
            if (ModelState.IsValid)
            {
                model._Scope = "https://spreadsheets.google.com/feeds https://docs.google.com/feeds";
                GoogleDocuments = new GoogleDocumentModel();
                GoogleDocuments.GoogleAPIAccess = model;
                return RedirectToAction("AuthorizationCheck", model);
            }
            return View(model);
        }

        public ActionResult AuthorizationCheck(GoogleAPIAccess model)
        {
            if (model.AccessCode == null)
            {
                ////////////////////////////////////////////////////////////////////////////
                // STEP 3: Get the Authorization URL
                ////////////////////////////////////////////////////////////////////////////

                // Get the authorization url.  The user of your application must visit
                // this url in order to authorize with Google.  If you are building a
                // browser-based application, you can redirect the user to the authorization
                // url.
                model._RedirectURIs = "urn:ietf:wg:oauth:2.0:oob";
                ViewData["authorizationUrl"] = OAuthUtil.CreateOAuth2AuthorizationUrl(model);
                model._RedirectURIs = Url.Action("AuthorizationCheckAuto", "Configuration", new { }, "http");
                ViewData["authorizationUrlAuto"] = OAuthUtil.CreateOAuth2AuthorizationUrl(model);
                return View(model);
            }

            ////////////////////////////////////////////////////////////////////////////
            // STEP 4: Get the Access Token
            ////////////////////////////////////////////////////////////////////////////

            // Once the user authorizes with Google, the request token can be exchanged
            // for a long-lived access token.  If you are building a browser-based
            // application, you should parse the incoming request token from the url and
            // set it in OAuthParameters before calling GetAccessToken().
            model._RedirectURIs = "urn:ietf:wg:oauth:2.0:oob";
            OAuthUtil.GetAccessToken(model);
            return RedirectToAction("Index", model);
        }

        public ActionResult AuthorizationCheckAuto(string code)
        {
            try
            {
                if (!string.IsNullOrEmpty(code))
                {
                    var model = GoogleDocuments.GoogleAPIAccess;
                    model.AccessCode = code;
                    model.RedirectUri = Url.Action("AuthorizationCheckAuto", "Configuration", new { }, "http");
                    OAuthUtil.GetAccessToken(model);
                    return RedirectToAction("Index", model);
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("AddNew", new { errorMsg = ex.Message });
            }
            return View();
        }
    }
}


