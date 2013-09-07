using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskTimer.Models;
using TaskTimer.UsefulClasses;

namespace TaskTimer.Areas.Admin.Controllers
{
    public class AdminEfarganController : Controller
    {
        //
        // GET: /Admin/AdminEfargan/

        public ActionResult EfarganPages()
        {
            return View((new CustomMembershipDB()).EfarganPages);
        }

        public ActionResult EfarganPagesAddNew()
        {
            //var newPage = new EfarganPages();
            //newPage.EfarganPagesSliders.Add(new EfarganPagesSliders());
            return View(new EfarganPages());
        }
        [HttpPost]
        public ActionResult EfarganPagesAddNew(EfarganPages newPage)
        {
            if (ModelState.IsValid)
            {
                var db = new CustomMembershipDB();
                var newSavedPage = db.EfarganPages.Add(newPage);
                db.SaveChanges();
                return RedirectToAction("EfarganPagesEdit", new { id = newSavedPage.Id });
            }
            return View();
        }

        public ActionResult EfarganPagesEdit(int id)
        {
            var curPage = (new CustomMembershipDB()).EfarganPages.SingleOrDefault(ef => ef.Id == id);
            if (curPage == null)
                return RedirectToAction("EfarganPages");
            //curPage.EfarganPagesSliders.Add(new EfarganPagesSliders());
            return View(curPage);
        }
        [HttpPost]
        public ActionResult EfarganPagesEdit(EfarganPages curPage, int id)
        {
            if (ModelState.IsValid)
            {
                var db = new CustomMembershipDB();
                var curPage2 = db.EfarganPages.SingleOrDefault(ef => ef.Id == id);
                if (curPage2 == null)
                {
                    //db.EfarganPages.Add(curPage);
                    return RedirectToAction("EfarganPages");
                }
                else
                {
                    //update page data
                    if (curPage.EfarganPagesParagraphs.Count == 0 && curPage.EfarganPagesSliders.Count == 0 && curPage.EfarganPagesTickers.Count == 0) 
                    {
                        curPage2.TitleTag = curPage.TitleTag;
                        curPage2.MetaTag = curPage.MetaTag;
                        curPage2.DescriptionTag = curPage.DescriptionTag;
                    }
                    //update / add to sliders
                    if (curPage.EfarganPagesSliders.Count > 0) 
                    {

                        foreach (var slider in curPage.EfarganPagesSliders)
                        {
                            var sliderToUpdate = curPage2.EfarganPagesSliders.FirstOrDefault(sl => sl.Id == slider.Id);
                            if (sliderToUpdate != null)
                            {
                                sliderToUpdate.SliderTitle = slider.SliderTitle;
                                sliderToUpdate.SliderText = slider.SliderText;
                                sliderToUpdate.SliderLinkText = slider.SliderLinkText;
                                sliderToUpdate.SliderLinkURL = slider.SliderLinkURL;
                                sliderToUpdate.SliderImage = slider.SliderImage;
                            }
                            else if (slider.Id == -1 &&
                                (
                                !string.IsNullOrEmpty(slider.SliderTitle) ||
                                !string.IsNullOrEmpty(slider.SliderText) ||
                                !string.IsNullOrEmpty(slider.SliderLinkText) ||
                                !string.IsNullOrEmpty(slider.SliderLinkURL) ||
                                !string.IsNullOrEmpty(slider.SliderImage)
                                ))  //add new slider to list
                                curPage2.EfarganPagesSliders.Add(slider);
                        }
                    }
                }
                db.Entry(curPage2).State = EntityState.Modified;
                db.SaveChanges();
                ModelState.Clear();
                return View(curPage2);
            }
            //var qwer1 = Request.Form["EfarganPagesSliders[0].SliderText"].ToString();
            return RedirectToAction("EfarganPages");
        }

        [AjaxOnly]
        public ActionResult EfarganPagesSaveSlider(EfarganPages curPage, int id)
        {
            if (ModelState.IsValid)
            {
                var db = new CustomMembershipDB();
                var curPage2 = db.EfarganPages.SingleOrDefault(ef => ef.Id == id);
                if (curPage2 == null)
                {
                    //db.EfarganPages.Add(curPage);
                    return RedirectToAction("EfarganPages");
                }
                else 
                {
                    foreach (var slider in curPage.EfarganPagesSliders)
                    {
                        var sliderToUpdate = curPage2.EfarganPagesSliders.FirstOrDefault(sl => sl.Id == slider.Id);
                        if (sliderToUpdate != null)
                        {
                            sliderToUpdate.SliderTitle = slider.SliderTitle;
                            sliderToUpdate.SliderText = slider.SliderText;
                            sliderToUpdate.SliderLinkText = slider.SliderLinkText;
                            sliderToUpdate.SliderLinkURL = slider.SliderLinkURL;
                            sliderToUpdate.SliderImage = slider.SliderImage;
                        }
                        else if (slider.Id == -1 &&
                            (
                            !string.IsNullOrEmpty(slider.SliderTitle) ||
                            !string.IsNullOrEmpty(slider.SliderText) ||
                            !string.IsNullOrEmpty(slider.SliderLinkText) ||
                            !string.IsNullOrEmpty(slider.SliderLinkURL) ||
                            !string.IsNullOrEmpty(slider.SliderImage)
                            ))  //add new slider to list
                            curPage2.EfarganPagesSliders.Add(slider);
                    }
                }
                db.Entry(curPage2).State = EntityState.Modified;
                db.SaveChanges();
                ModelState.Clear();
                return View(curPage2);
            }
            //var qwer1 = Request.Form["EfarganPagesSliders[0].SliderText"].ToString();
            return RedirectToAction("EfarganPages");
        }
        [AjaxOnly]
        public string EfarganPagesAjaxSaveSliderData(EfarganPagesSliders data)
        {
            return "qwer1";
        }
    }
}
