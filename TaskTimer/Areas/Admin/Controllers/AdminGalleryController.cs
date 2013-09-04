using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskTimer.Areas.Admin.Models;
using TaskTimer.Models;
using TaskTimer.UsefulClasses;

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
                return View(new GalleryPages());
            return View(editPage);
            //var editPages = db.GalleryPages.Where(p => p.Id == id);
            //if (editPages.Count() == 0)
            //    return View(new GalleryPages());
            //return View(editPages.ToList());
            //var qwer11list = new List<qwer11>();
            //qwer11list.Add(new qwer11()
            //{
            //    alt = "alt1",
            //    date = DateTime.Now,
            //    name = "nameq"
            //});
            //qwer11list.Add(new qwer11()
            //{
            //    alt = "alt2",
            //    date = DateTime.Now,
            //    name = "nameq"
            //});
            //return View(qwer11list);
        }
        [HttpPost]
        public ActionResult GalleryPagesAddNew(GalleryPages newPage)
        {
            var db = new CustomMembershipDB();
            var form= Request.Form;
            var i = 0;
            while (form["GalleryImages[" + i + "].Id"] != null)
            {
                var imageId = int.Parse(form["GalleryImages[" + i + "].Id"]);
                var imageAlt = form["GalleryImages[" + i + "].Alt"];
                var imageBottomText = form["GalleryImages[" + i + "].BottomText"];
                var curGalleryImage = db.GalleryImages.SingleOrDefault(g => g.Id == imageId);
                if (curGalleryImage != null)
                {
                    curGalleryImage.Alt = imageAlt;
                    curGalleryImage.BottomText = imageBottomText;
                    newPage.GalleryImages.Add(curGalleryImage);
                }
                i++;
            }
            if (ModelState.IsValid)
            {
                //upload files
                
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
        [HttpGet]
        public PartialViewResult GalleryImages(int id)
        {
            //if (id == -1)
            //    return PartialView("_ImagesPartial", new GalleryImages());


            CustomMembershipDB db = new CustomMembershipDB();
            var galleryImages = db.GalleryPages.FirstOrDefault(p => p.Id == id);
            return PartialView("_GalleryImagesPartial", galleryImages.GalleryImages.ToList());
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
        [AjaxOnly, HttpPost]
        public ActionResult GalleryPagesDeleteAjax(int id)
        {
            CustomMembershipDB db = new CustomMembershipDB();
            var galleryPage = db.GalleryPages.SingleOrDefault(q => q.Id == id);
            if (galleryPage != null)
            {
                db.GalleryPages.Remove(galleryPage);
                //db.SaveChanges();
                return Content("OK");
            }
            return Content("Page was not found");
        }
    }
    public class qwer11 
    {
        public string alt { get; set; }
        public string name { get; set; }
        public DateTime date { get; set; }
    }
}
