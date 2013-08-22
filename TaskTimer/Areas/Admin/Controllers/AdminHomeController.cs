using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskTimer.Models;

namespace TaskTimer.Areas.Admin.Controllers
{
    public class ViewDataUploadFilesResult
    {
        public string Name { get; set; }
        public int Length { get; set; }
    }
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
            var saveFileName = "";
            //upload files
            var r = new List<ViewDataUploadFilesResult>();
            var pathToDirectory = AppDomain.CurrentDomain.BaseDirectory + "Images\\Site Pages\\";
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                if (hpf.ContentLength == 0)
                    continue;

                saveFileName = Path.GetFileName(hpf.FileName);
                var saveFileCounter = 0;
                var saveFileIsNewName = false;
                do //check if file with same name already exist
                {
                    var allSitePageImages = Directory.GetFiles(pathToDirectory);
                    saveFileIsNewName = false;
                    foreach (var filePath in allSitePageImages)
                    {
                        if (filePath.Contains(saveFileName)) //search for new file name
                        {
                            if (saveFileName.Contains("(" + saveFileCounter + ")"))
                                saveFileName = saveFileName.Replace("(" + saveFileCounter + ")", "");
                            var saveFileNameArr = saveFileName.Split('.');
                            saveFileName = saveFileNameArr[0] + "(" + saveFileCounter + ")." + saveFileNameArr[1];
                            saveFileIsNewName = true;
                            break;
                        }
                    }
                    saveFileCounter++;
                } while (!saveFileIsNewName);
                string savedFileName = Path.Combine(
                  pathToDirectory,
                  saveFileName);
                hpf.SaveAs(savedFileName);
                if (file.Equals("TitleImage"))
                    newPage.TitleImage = saveFileName;
                else if (file.Equals("BodyImage"))
                    newPage.BodyImage = saveFileName;
                else  if (file.Equals("FooterImage"))
                    newPage.FooterImage = saveFileName;
                r.Add(new ViewDataUploadFilesResult()
                {
                    Name = savedFileName,
                    Length = hpf.ContentLength
                });
            }
            //return View("UploadedFiles", r);


            var db = new CustomMembershipDB();
            
            db.SiteMainPages.Add(newPage);
            //db.SaveChanges();

            return RedirectToAction("SitePages");
        }
    }
}
