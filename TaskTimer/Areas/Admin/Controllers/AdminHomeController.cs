using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskTimer.Areas.Admin.Models;
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
        public ActionResult SitePagesAddNew(int id)
        {
            if (id == -1) //new page
                return View(new SiteMainPages());
            var db = new CustomMembershipDB();
            var editPage = db.SiteMainPages.FirstOrDefault(p => p.Id == id);
            if (editPage == null)
                editPage = new SiteMainPages();
            return View(editPage);
        }
        [HttpPost]
        public ActionResult SitePagesAddNew(SiteMainPages newPage)
        {
            //upload files
            var saveToDirectory = AppDomain.CurrentDomain.BaseDirectory + "Images\\Site Pages\\";
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                if (hpf.ContentLength == 0)
                    continue;
                var saveFileName = Path.GetFileName(hpf.FileName);
                saveFileName = AdminHelper.GetUniqueFileName(saveToDirectory, saveFileName);
                string savedFileName = Path.Combine(
                  saveToDirectory,
                  saveFileName);
                hpf.SaveAs(savedFileName);
                if (file.Equals("TitleImage"))
                    newPage.TitleImage = saveFileName;
                else if (file.Equals("BodyImage"))
                    newPage.BodyImage = saveFileName;
                else  if (file.Equals("FooterImage"))
                    newPage.FooterImage = saveFileName;
            }
            var db = new CustomMembershipDB();
            db.SiteMainPages.Add(newPage);
            db.SaveChanges();
            return RedirectToAction("SitePages");
        }
        public ActionResult SitePagesDelete(int id)
        {
            var db = new CustomMembershipDB();
            var deletePage = db.SiteMainPages.FirstOrDefault(p => p.Id == id);
            if (deletePage != null)
            {
                db.SiteMainPages.Remove(deletePage);
                db.SaveChanges();
            }
            return RedirectToAction("SitePages");
        }
    }
}
