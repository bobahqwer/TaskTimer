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
                        curPage2.TicketTitle = curPage.TicketTitle;
                        curPage2.TicketLinkText = curPage.TicketLinkText;
                        curPage2.TicketLinkURL = curPage.TicketLinkURL;
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

                    //update / add to paragraphs
                    if (curPage.EfarganPagesParagraphs.Count > 0)
                    {

                        foreach (var paragraph in curPage.EfarganPagesParagraphs)
                        {
                            var sliderToUpdate = curPage2.EfarganPagesParagraphs.FirstOrDefault(sl => sl.Id == paragraph.Id);
                            if (sliderToUpdate != null)
                            {
                                sliderToUpdate.ParagraphTitle = paragraph.ParagraphTitle;
                                sliderToUpdate.ParagraphText = paragraph.ParagraphText;
                                sliderToUpdate.ParagraphLinkText = paragraph.ParagraphLinkText;
                                sliderToUpdate.ParagraphLinkURL = paragraph.ParagraphLinkURL;
                                sliderToUpdate.ParagraphImage = paragraph.ParagraphImage;
                            }
                            else if (paragraph.Id == -1 &&
                                (
                                !string.IsNullOrEmpty(paragraph.ParagraphTitle) ||
                                !string.IsNullOrEmpty(paragraph.ParagraphText) ||
                                !string.IsNullOrEmpty(paragraph.ParagraphLinkText) ||
                                !string.IsNullOrEmpty(paragraph.ParagraphLinkURL) ||
                                !string.IsNullOrEmpty(paragraph.ParagraphImage)
                                ))  //add new slider to list
                                curPage2.EfarganPagesParagraphs.Add(paragraph);
                        }
                    }

                    //update / add to tickets
                    if (curPage.EfarganPagesTickers.Count > 0)
                    {

                        foreach (var ticker in curPage.EfarganPagesTickers)
                        {
                            var sliderToUpdate = curPage2.EfarganPagesTickers.FirstOrDefault(sl => sl.Id == ticker.Id);
                            if (sliderToUpdate != null)
                            {
                                sliderToUpdate.TickerTitle = ticker.TickerTitle;
                                sliderToUpdate.TickerText = ticker.TickerText;
                                sliderToUpdate.TickerLinkText = ticker.TickerLinkText;
                                sliderToUpdate.TickerLinkURL = ticker.TickerLinkURL;
                            }
                            else if (ticker.Id == -1 &&
                                (
                                !string.IsNullOrEmpty(ticker.TickerTitle) ||
                                !string.IsNullOrEmpty(ticker.TickerText) ||
                                !string.IsNullOrEmpty(ticker.TickerLinkText) ||
                                !string.IsNullOrEmpty(ticker.TickerLinkURL)
                                ))  //add new slider to list
                                curPage2.EfarganPagesTickers.Add(ticker);
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
