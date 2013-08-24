using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskTimer.Areas.Admin.Models;
using TaskTimer.Models;

namespace TaskTimer.Areas.Admin.Controllers
{
    public class AdminGalleryController : Controller
    {
        //
        // GET: /Admin/GalleryPages/

        public ActionResult GalleryPages()
        {
            var db = new CustomMembershipDB();
            return View(db.GalleryPages);
        }
        public ActionResult GalleryPagesAddNew(int id)
        {
            if (id == -1) //new page
                return View(new GalleryPages());
            var db = new CustomMembershipDB();
            var editPage = db.GalleryPages.FirstOrDefault(p => p.Id == id);
            if (editPage == null)
                editPage = new GalleryPages();
            return View(editPage);
        }
        [HttpPost]
        public ActionResult GalleryPagesAddNew(GalleryPages newPage)
        {
            if (ModelState.IsValid)
            {
                //upload files
                var db = new CustomMembershipDB();
                var saveToDirectory = AppDomain.CurrentDomain.BaseDirectory + "Images/Gallery Pages/";
                var uploadedMultipleFiles = new List<string>();
                foreach (string file in Request.Files)
                {
                    var multipleFiles = Request.Files.GetMultiple(file);
                    if (multipleFiles.Count > 1 && !uploadedMultipleFiles.Contains(file))  //multiple uploaded files
                    {
                        var curGalleryId = 1;
                        if (db.GalleryPages.Count() > 0)
                            curGalleryId = db.GalleryPages.Max(p => p.Id) + 1;
                        var imagePath = "Gallery " + curGalleryId + "/";
                        var saveToDirectoryMult = saveToDirectory + imagePath;
                        foreach (var item in multipleFiles)
                        {
                            if (item.ContentLength == 0 || uploadedMultipleFiles.Contains(file))
                                continue;
                            var saveFileName = AdminHelper.ClearStrForDBSave(item.FileName);
                            if (!Directory.Exists(saveToDirectoryMult))
                                Directory.CreateDirectory(saveToDirectoryMult);
                            saveFileName = AdminHelper.GetUniqueFileName(saveToDirectoryMult, saveFileName);
                            string savedFileName = Path.Combine(
                              saveToDirectoryMult,
                              saveFileName);
                            item.SaveAs(savedFileName);
                            newPage.GalleryImages.Add(new GalleryImages()
                            {
                                Name = saveFileName,
                                Path = imagePath + saveFileName
                            });
                        }
                        uploadedMultipleFiles.Add(file); //add to list with multiple uploaded group names
                    }
                    else
                    {
                        HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                        if (hpf.ContentLength == 0 || uploadedMultipleFiles.Contains(file))
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
                        else if (file.Equals("FooterImage"))
                            newPage.FooterImage = saveFileName;
                    }
                }

                if (newPage.Id == null)
                    db.GalleryPages.Add(newPage);
                else
                    db.Entry(newPage).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("GalleryPages");
        }
        public ActionResult GalleryPagesDelete(int id)
        {
            var db = new CustomMembershipDB();
            var deletePage = db.GalleryPages.FirstOrDefault(p => p.Id == id);
            if (deletePage != null)
            {
                db.GalleryPages.Remove(deletePage);
                db.SaveChanges();
            }
            return RedirectToAction("GalleryPages");
        }
    }
}
