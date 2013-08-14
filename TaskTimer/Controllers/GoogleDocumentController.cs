using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;
using System.Web.Mvc;
using Google.GData.Client;
using Google.GData.Spreadsheets;
using TaskTimer.Models;

namespace TaskTimer.Controllers
{
    public class GoogleDocumentController : Controller
    {
        //
        // GET: /GoogleDocument/
        private Timer timer = new Timer();

        public ActionResult Index(string errorMsg)
        {
            ViewBag.errorMsg = errorMsg;
            var model = ConfigurationController.GoogleDocuments;
            if (model != null && model.GoogleAPIAccess != null)
                return View(model);
            return View();
        }


        public ActionResult Document(string docName, string workName) 
        {
            var model = ConfigurationController.GoogleDocuments;
            ViewBag.Worksheet = workName;
            if (model != null && model.GoogleAPIAccess != null)
            {
                ViewBag.Message = docName;
                var document = model.Documents.SingleOrDefault(d => d.Title.Text.Equals(docName));
                if (document == null)
                    return RedirectToAction("Index", new { errorMsg = "Document was not found." });
                model.Document = (SpreadsheetEntry)document;
                model.Worksheets = model.Document.Worksheets.Entries;
                //get worksheet
                if (string.IsNullOrEmpty(workName))
                    model.Worksheet = (WorksheetEntry)model.Worksheets.FirstOrDefault();
                else
                {
                    var worksheet = (WorksheetEntry)model.Worksheets.SingleOrDefault(w => w.Title.Text.Equals(workName));
                    if (worksheet == null)
                        ViewBag.errorMsg = "Worksheet '" + workName + "' was not found in current document.";
                    else
                    {
                        model.Worksheet = worksheet;
                        GetDataTable();
                        if (model.TableSave == null)
                            GetSaveTable();
                    }
                }
                return View(model);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Document(int? rowNumber, string docName, string workName) 
        {
            var model = ConfigurationController.GoogleDocuments;
            if (model != null && model.GoogleAPIAccess != null)
            {
                GOAuth2RequestFactory requestFactory = new GOAuth2RequestFactory(null, "MySpreadsheetIntegration-v1", model.GoogleAPIAccess);
                SpreadsheetsService service = new SpreadsheetsService("MySpreadsheetIntegration-v1");
                service.RequestFactory = requestFactory;
                model.RowActive = rowNumber;
                if (rowNumber != null)
                {
                    //add row to save table
                    model.RowActiveSave = (ListEntry)service.Insert(model.TableSave, (ListEntry)model.Table[(int)rowNumber]);
                    model.RowActiveSave.Elements.Add(new ListEntry.Custom() { LocalName = "workdate", Value = DateTime.Now.ToString() });
                    model.RowActiveSave.Elements.Add(new ListEntry.Custom() { LocalName = "hours", Value = "0" });
                    model.RowActiveSave.Update();
                }
                else 
                {
                    GetSaveTable();
                    ListEntry lastRow = (ListEntry)model.TableSave.Entries.Last();
                    var lastRowCells = (ListEntry.CustomElementCollection)lastRow.Elements;
                    DateTime startDate = DateTime.Now;
                    foreach (ListEntry.Custom cell in lastRowCells)
	                {
                        if (cell.LocalName.Equals("workdate"))
                        {
                            startDate = DateTime.Parse(cell.Value);
                        }
                        if (cell.LocalName.Equals("hours")) 
                        {
                            cell.Value = ((double)(DateTime.Now - startDate).Seconds / 60.0 / 60.0).ToString();
                        }
                        
	                }
                    lastRow.Update();
                }
            //timer.Interval = 10000; //10 sec
            //timer.Elapsed += timer_Elapsed;
            //timer.Start();
            }
            return RedirectToAction("Document", new { docName = docName, workName = workName });
        }


        private void GetDataTable() 
        {
            var model = ConfigurationController.GoogleDocuments;
            //get table data
            // Define the URL to request the list feed of the worksheet.
            AtomLink listFeedLink = model.Worksheet.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);

            // Fetch the list feed of the worksheet.
            // Initialize the variables needed to make the request
            GOAuth2RequestFactory requestFactory =
                new GOAuth2RequestFactory(null, "MySpreadsheetIntegration-v1", model.GoogleAPIAccess);
            SpreadsheetsService service = new SpreadsheetsService("MySpreadsheetIntegration-v1");
            service.RequestFactory = requestFactory;
            ListQuery listQuery = new ListQuery(listFeedLink.HRef.ToString());
            model.Table = service.Query(listQuery).Entries;
        }
        private void GetSaveTable() 
        {
            var model = ConfigurationController.GoogleDocuments;
            //get save table
            WorksheetEntry worksheet = (WorksheetEntry)model.Worksheets.SingleOrDefault(w => w.Title.Text.Equals("Auto HOURS"));
            // Define the URL to request the list feed of the worksheet.
            AtomLink listFeedLink = worksheet.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);

            // Fetch the list feed of the worksheet.
            // Initialize the variables needed to make the request
            GOAuth2RequestFactory requestFactory =
                new GOAuth2RequestFactory(null, "MySpreadsheetIntegration-v1", model.GoogleAPIAccess);
            SpreadsheetsService service = new SpreadsheetsService("MySpreadsheetIntegration-v1");
            service.RequestFactory = requestFactory;
            ListQuery listQuery = new ListQuery(listFeedLink.HRef.ToString());
            model.TableSave = service.Query(listQuery);
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var model = ConfigurationController.GoogleDocuments;
            if (model != null)
            {
                int hoursIndex = model.RowActiveSave.Elements.IndexOf(new ListEntry.Custom() { LocalName = "hours" });
                double totalTime = 0;
                if (double.TryParse(model.RowActiveSave.Elements[hoursIndex].Value, out totalTime))
                {
                    //totalTime = totalTime * 60 * 60; //get in seconds
                    totalTime += 15; //add new 30 seconds
                    //totalTime = totalTime / 60 / 60;
                }
                model.RowActiveSave.Elements[hoursIndex].Value = totalTime.ToString();
                model.RowActiveSave.Elements[9].Value = "13";
                model.RowActiveSave.Update();
            }
        }

    }
}
