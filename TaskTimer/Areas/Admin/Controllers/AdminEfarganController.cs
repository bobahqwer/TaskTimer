using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskTimer.Areas.Admin.Models;
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
        [ValidateInput(false)]
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
                    if (curPage.EfarganPagesParagraphs.Count == 0 
                        && curPage.EfarganPagesSliders.Count == 0 
                        && curPage.EfarganPagesTickers.Count == 0)
                    {
                        curPage2.TitleTag = curPage.TitleTag;
                        curPage2.MetaTag = curPage.MetaTag;
                        curPage2.DescriptionTag = curPage.DescriptionTag;
                        curPage2.MenuText = curPage.MenuText;
                        curPage2.TicketTitle = curPage.TicketTitle;
                        curPage2.TicketLinkText = curPage.TicketLinkText;
                        curPage2.TicketLinkURL = curPage.TicketLinkURL;
                    }
                    //update / add to sliders
                    if (curPage.EfarganPagesSliders.Count > 0)
                    {
                        foreach (var slider in curPage.EfarganPagesSliders)
                        {
                            //get file data
                            var file = Request.Files["SliderImage_" + slider.Id];
                            var savedFileName = "";
                            if (file != null && file.ContentLength > 0) //need to save a new file
                            {
                                //delete old file if file already exist
                                if (slider.SliderImageURL != null && !string.IsNullOrEmpty(slider.SliderImageURL)
                                    && slider.SliderImageFileName != null && !string.IsNullOrEmpty(slider.SliderImageFileName))
                                {
                                    var oldFileName = slider.SliderImageURL.Split('/').Last();
                                    var savedToDirectory = AppDomain.CurrentDomain.BaseDirectory + AdminEfarganHelper.PageImagesPath;
                                    var savedOldFilePath = savedToDirectory + "/" + oldFileName;
                                    System.IO.File.Delete(savedOldFilePath);
                                }
                                savedFileName = AdminEfarganHelper.SavePageImage(file);
                            }    

                            //add / update slider data
                            var sliderToUpdate = curPage2.EfarganPagesSliders.FirstOrDefault(sl => sl.Id == slider.Id);
                            if (sliderToUpdate != null)
                            {
                                sliderToUpdate.SliderTitle = slider.SliderTitle;
                                sliderToUpdate.SliderText = slider.SliderText;
                                sliderToUpdate.SliderLinkText = slider.SliderLinkText;
                                sliderToUpdate.SliderLinkURL = slider.SliderLinkURL;
                                if (file != null && file.ContentLength > 0) //need to save a new file
                                {
                                    sliderToUpdate.SliderImageURL = AdminEfarganHelper.PageImagesPath + savedFileName.Split('/').Last();
                                    sliderToUpdate.SliderImageFileName = file.FileName;
                                }
                            }
                            else if (slider.Id == -1 &&
                                (
                                !string.IsNullOrEmpty(slider.SliderTitle) ||
                                !string.IsNullOrEmpty(slider.SliderText) ||
                                !string.IsNullOrEmpty(slider.SliderLinkText) ||
                                !string.IsNullOrEmpty(slider.SliderLinkURL) ||
                                !string.IsNullOrEmpty(slider.SliderImageFileName)
                                ))  //add new slider to list
                            {
                                if (file != null && file.ContentLength > 0) //need to save a new file
                                {
                                    slider.SliderImageURL = AdminEfarganHelper.PageImagesPath + savedFileName.Split('/').Last();
                                    slider.SliderImageFileName = file.FileName;
                                }
                                curPage2.EfarganPagesSliders.Add(slider);
                            }
                        }
                    }

                    //update / add to paragraphs
                    if (curPage.EfarganPagesParagraphs.Count > 0)
                    {
                        foreach (var paragraph in curPage.EfarganPagesParagraphs)
                        {
                            //get file data
                            var file = Request.Files["ParagraphImage_" + paragraph.Id];
                            var savedFileName = "";
                            if (file != null && file.ContentLength > 0) //need to save a new file
                            {
                                //delete old file if file already exist
                                if (paragraph.ParagraphImageURL != null && !string.IsNullOrEmpty(paragraph.ParagraphImageURL)
                                    && paragraph.ParagraphImageFileName != null && !string.IsNullOrEmpty(paragraph.ParagraphImageFileName))
                                {
                                    var oldFileName = paragraph.ParagraphImageURL.Split('/').Last();
                                    var savedToDirectory = AppDomain.CurrentDomain.BaseDirectory + AdminEfarganHelper.PageImagesPath;
                                    var savedOldFilePath = savedToDirectory + "/" + oldFileName;
                                    System.IO.File.Delete(savedOldFilePath);
                                }
                                savedFileName = AdminEfarganHelper.SavePageImage(file);
                            }

                            //add / update paragraph data
                            var paragraphToUpdate = curPage2.EfarganPagesParagraphs.FirstOrDefault(sl => sl.Id == paragraph.Id);
                            if (paragraphToUpdate != null)
                            {
                                paragraphToUpdate.ParagraphTitle = paragraph.ParagraphTitle;
                                paragraphToUpdate.ParagraphText = paragraph.ParagraphText;
                                paragraphToUpdate.ParagraphLinkText = paragraph.ParagraphLinkText;
                                paragraphToUpdate.ParagraphLinkURL = paragraph.ParagraphLinkURL;
                                paragraphToUpdate.ParagraphImageFileName = paragraph.ParagraphImageFileName;
                                if (file != null && file.ContentLength > 0) //need to save a new file
                                {
                                    paragraphToUpdate.ParagraphImageURL = AdminEfarganHelper.PageImagesPath + savedFileName.Split('/').Last();
                                    paragraphToUpdate.ParagraphImageFileName = file.FileName;
                                }
                            }
                            else if (paragraph.Id == -1 &&
                                (
                                !string.IsNullOrEmpty(paragraph.ParagraphTitle) ||
                                !string.IsNullOrEmpty(paragraph.ParagraphText) ||
                                !string.IsNullOrEmpty(paragraph.ParagraphLinkText) ||
                                !string.IsNullOrEmpty(paragraph.ParagraphLinkURL) ||
                                !string.IsNullOrEmpty(paragraph.ParagraphImageFileName)
                                ))  //add new slider to list
                            {
                                if (file != null && file.ContentLength > 0) //need to save a new file
                                {
                                    paragraph.ParagraphImageURL = AdminEfarganHelper.PageImagesPath + savedFileName.Split('/').Last();
                                    paragraph.ParagraphImageFileName = file.FileName;
                                }
                                curPage2.EfarganPagesParagraphs.Add(paragraph);
                            }
                        }
                    }

                    //update / add to tickets
                    if (curPage.EfarganPagesTickers.Count > 0)
                    {
                        foreach (var ticker in curPage.EfarganPagesTickers)
                        {
                            var tickerToUpdate = curPage2.EfarganPagesTickers.FirstOrDefault(sl => sl.Id == ticker.Id);
                            if (tickerToUpdate != null)
                            {
                                tickerToUpdate.TickerTitle = ticker.TickerTitle;
                                tickerToUpdate.TickerText = ticker.TickerText;
                                tickerToUpdate.TickerLinkText = ticker.TickerLinkText;
                                tickerToUpdate.TickerLinkURL = ticker.TickerLinkURL;
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

                    //update / add to page language
                    if (curPage.EfarganPagesLanguage.Count > 0)
                    {
                        int i = 0;
                        foreach (var pagesLanguage in curPage.EfarganPagesLanguage)
                        {
                            var pagesLanguageToUpdate = curPage2.EfarganPagesLanguage.FirstOrDefault(lng => lng.Id == pagesLanguage.Id);
                            if (pagesLanguageToUpdate != null)
                            {
                                pagesLanguageToUpdate.TitleTag = pagesLanguage.TitleTag;
                                pagesLanguageToUpdate.MetaTag = pagesLanguage.MetaTag;
                                pagesLanguageToUpdate.DescriptionTag = pagesLanguage.DescriptionTag;
                                pagesLanguageToUpdate.MenuText = pagesLanguage.MenuText;
                                pagesLanguageToUpdate.TicketTitle = pagesLanguage.TicketTitle;
                                pagesLanguageToUpdate.TicketLinkText = pagesLanguage.TicketLinkText;
                                pagesLanguageToUpdate.TicketLinkURL = pagesLanguage.TicketLinkURL;
                                pagesLanguageToUpdate.EfarganLanguage = pagesLanguage.EfarganLanguage;

                                //get page language
                                var LanguageValue = Request.Form["EfarganPagesLanguage[" + i + "].SiteLanguages"];
                                var curLang = db.EfarganLanguage.FirstOrDefault(lng => lng.LanguageValue == LanguageValue);
                                if (curLang != null)
                                    pagesLanguageToUpdate.EfarganLanguage = curLang;
                                i++;
                            }
                            else if (pagesLanguage.Id == -1 &&
                                (
                                !string.IsNullOrEmpty(pagesLanguage.TitleTag) ||
                                !string.IsNullOrEmpty(pagesLanguage.MetaTag) ||
                                !string.IsNullOrEmpty(pagesLanguage.DescriptionTag) ||
                                !string.IsNullOrEmpty(pagesLanguage.MenuText) ||
                                !string.IsNullOrEmpty(pagesLanguage.TicketTitle) ||
                                !string.IsNullOrEmpty(pagesLanguage.TicketLinkText) ||
                                !string.IsNullOrEmpty(pagesLanguage.TicketLinkURL)
                                ))  //add new slider to list
                            {
                                //get page language
                                var LanguageValue = Request.Form["EfarganPagesLanguage[" + i + "].SiteLanguages"];
                                var curLang = db.EfarganLanguage.FirstOrDefault(lng => lng.LanguageValue == LanguageValue);
                                if (curLang != null)
                                    pagesLanguage.EfarganLanguageRef = curLang.Id;
                                curPage2.EfarganPagesLanguage.Add(pagesLanguage);
                                i++;
                            }
                        }
                    }
                }
                db.Entry(curPage2).State = EntityState.Modified;
                db.SaveChanges();
                ModelState.Clear();
                return View(curPage2);
            }
            return RedirectToAction("EfarganPages");
        }

        [HttpPost]
        public ActionResult EfarganPagesLanguageEdit(EfarganPages curPage, int id, string itemType, int itemCount)
        {
            var db = new CustomMembershipDB();
            var curPage2 = db.EfarganPagesLanguage.SingleOrDefault(efl => efl.Id == id);
            if (curPage2 == null)
            {
                //db.EfarganPages.Add(curPage);
                return RedirectToAction("EfarganPages");
            }
            else
            {
                for (int i = 0; i < itemCount; i++)
                {
                    switch (itemType)
                    {
                        case "Sliders":
                            //get slider data
                            var SliderId = int.Parse(Request.Form["EfarganPagesLanguage.item.EfarganPagesSliders[" + i + "].Id"]);
                            var SliderTitle = Request.Form["EfarganPagesLanguage.item.EfarganPagesSliders[" + i + "].SliderTitle"];
                            var SliderText = Request.Form["EfarganPagesLanguage.item.EfarganPagesSliders[" + i + "].SliderText"];
                            var SliderLinkText = Request.Form["EfarganPagesLanguage.item.EfarganPagesSliders[" + i + "].SliderLinkText"];
                            var SliderLinkURL = Request.Form["EfarganPagesLanguage.item.EfarganPagesSliders[" + i + "].SliderLinkURL"];
                            var SliderImageFileName = Request.Form["EfarganPagesLanguage.item.EfarganPagesSliders[" + i + "].SliderImageFileName"];
                            var SliderImageURL = Request.Form["EfarganPagesLanguage.item.EfarganPagesSliders[" + i + "].SliderImageURL"];

                            //get file data
                            var file = Request.Files["SliderImage_" + SliderId];
                            var savedFileName = "";
                            if (file != null && file.ContentLength > 0) //need to save a new file
                            {
                                //delete old file if file already exist
                                if (!string.IsNullOrEmpty(SliderImageFileName) && !string.IsNullOrEmpty(SliderImageURL))
                                {
                                    var oldFileName = SliderImageURL.Split('/').Last();
                                    var savedToDirectory = AppDomain.CurrentDomain.BaseDirectory + AdminEfarganHelper.PageImagesPath;
                                    var savedOldFilePath = savedToDirectory + "/" + oldFileName;
                                    System.IO.File.Delete(savedOldFilePath);
                                }
                                savedFileName = AdminEfarganHelper.SavePageImage(file);
                            }    

                            //add / update slider data
                            if (!string.IsNullOrEmpty(SliderTitle) ||
                                !string.IsNullOrEmpty(SliderText) ||
                                !string.IsNullOrEmpty(SliderLinkText) ||
                                !string.IsNullOrEmpty(SliderLinkURL))
                            {
                                EfarganPagesSliders curSlider;
                                if (SliderId == -1)
                                {
                                    curSlider = new EfarganPagesSliders();
                                    curPage2.EfarganPagesSliders.Add(curSlider);
                                }
                                else
                                    curSlider = curPage2.EfarganPagesSliders.FirstOrDefault(s => s.Id == SliderId);
                                curSlider.SliderTitle = SliderTitle;
                                curSlider.SliderText = SliderText;
                                curSlider.SliderLinkText = SliderLinkText;
                                curSlider.SliderLinkURL = SliderLinkURL;
                                if (file != null && file.ContentLength > 0) //need to save a new file
                                {
                                    curSlider.SliderImageURL = AdminEfarganHelper.PageImagesPath + savedFileName.Split('/').Last();
                                    curSlider.SliderImageFileName = file.FileName;
                                }
                            }
                            break;
                        case "Paragraphs":
                            //get paragraph data
                            var ParagraphId = int.Parse(Request.Form["EfarganPagesLanguage.item.EfarganPagesParagraphs[" + i + "].Id"]);
                            var ParagraphTitle = Request.Form["EfarganPagesLanguage.item.EfarganPagesParagraphs[" + i + "].ParagraphTitle"];
                            var ParagraphText = Request.Form["EfarganPagesLanguage.item.EfarganPagesParagraphs[" + i + "].ParagraphText"];
                            var ParagraphLinkText = Request.Form["EfarganPagesLanguage.item.EfarganPagesParagraphs[" + i + "].ParagraphLinkText"];
                            var ParagraphLinkURL = Request.Form["EfarganPagesLanguage.item.EfarganPagesParagraphs[" + i + "].ParagraphLinkURL"];
                            var ParagraphImageFileName = Request.Form["EfarganPagesLanguage.item.EfarganPagesParagraphs[" + i + "].ParagraphImageFileName"];
                            var ParagraphImageURL = Request.Form["EfarganPagesLanguage.item.EfarganPagesParagraphs[" + i + "].ParagraphImageURL"];

                            //get file data
                            var file2 = Request.Files["ParagraphImage_" + ParagraphId];
                            var savedFileName2 = "";
                            if (file2 != null && file2.ContentLength > 0) //need to save a new file
                            {
                                //delete old file if file already exist
                                if (!string.IsNullOrEmpty(ParagraphImageFileName) && !string.IsNullOrEmpty(ParagraphImageURL))
                                {
                                    var oldFileName = ParagraphImageURL.Split('/').Last();
                                    var savedToDirectory = AppDomain.CurrentDomain.BaseDirectory + AdminEfarganHelper.PageImagesPath;
                                    var savedOldFilePath = savedToDirectory + "/" + oldFileName;
                                    System.IO.File.Delete(savedOldFilePath);
                                }
                                savedFileName2 = AdminEfarganHelper.SavePageImage(file2);
                            }    

                            if (!string.IsNullOrEmpty(ParagraphTitle) ||
                                !string.IsNullOrEmpty(ParagraphText) ||
                                !string.IsNullOrEmpty(ParagraphLinkText) ||
                                !string.IsNullOrEmpty(ParagraphLinkURL))
                            {
                                EfarganPagesParagraphs curParagraph;
                                if (ParagraphId == -1)
                                {
                                    curParagraph = new EfarganPagesParagraphs();
                                    curPage2.EfarganPagesParagraphs.Add(curParagraph);
                                }
                                else
                                    curParagraph = curPage2.EfarganPagesParagraphs.FirstOrDefault(p => p.Id == ParagraphId);
                                curParagraph.ParagraphTitle = ParagraphTitle;
                                curParagraph.ParagraphText = ParagraphText;
                                curParagraph.ParagraphLinkText = ParagraphLinkText;
                                curParagraph.ParagraphLinkURL = ParagraphLinkURL;
                                if (file2 != null && file2.ContentLength > 0) //need to save a new file
                                {
                                    curParagraph.ParagraphImageURL = AdminEfarganHelper.PageImagesPath + savedFileName2.Split('/').Last();
                                    curParagraph.ParagraphImageFileName = file2.FileName;
                                }
                            }
                            break;
                        case "Tickers":
                            var TickerId = int.Parse(Request.Form["EfarganPagesLanguage.item.EfarganPagesTickers[" + i + "].Id"]);
                            var TickerTitle = Request.Form["EfarganPagesLanguage.item.EfarganPagesTickers[" + i + "].TickerTitle"];
                            var TickerText = Request.Form["EfarganPagesLanguage.item.EfarganPagesTickers[" + i + "].TickerText"];
                            var TickerLinkText = Request.Form["EfarganPagesLanguage.item.EfarganPagesTickers[" + i + "].TickerLinkText"];
                            var TickerLinkURL = Request.Form["EfarganPagesLanguage.item.EfarganPagesTickers[" + i + "].TickerLinkURL"];

                            if (!string.IsNullOrEmpty(TickerTitle) ||
                                !string.IsNullOrEmpty(TickerText) ||
                                !string.IsNullOrEmpty(TickerLinkText) ||
                                !string.IsNullOrEmpty(TickerLinkURL))
                            {
                                EfarganPagesTickers curTicker;
                                if (TickerId == -1)
                                {
                                    curTicker = new EfarganPagesTickers();
                                    curPage2.EfarganPagesTickers.Add(curTicker);
                                }
                                else
                                    curTicker = curPage2.EfarganPagesTickers.FirstOrDefault(p => p.Id == TickerId);
                                curTicker.TickerTitle = TickerTitle;
                                curTicker.TickerText = TickerText;
                                curTicker.TickerLinkText = TickerLinkText;
                                curTicker.TickerLinkURL = TickerLinkURL;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            db.Entry(curPage2).State = EntityState.Modified;
            db.SaveChanges();
            ModelState.Clear();
            return RedirectToAction("EfarganPagesEdit", new { curPage = curPage, id = curPage2.EfarganPages.Id });
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
                            sliderToUpdate.SliderImageFileName = slider.SliderImageFileName;
                        }
                        else if (slider.Id == -1 &&
                            (
                            !string.IsNullOrEmpty(slider.SliderTitle) ||
                            !string.IsNullOrEmpty(slider.SliderText) ||
                            !string.IsNullOrEmpty(slider.SliderLinkText) ||
                            !string.IsNullOrEmpty(slider.SliderLinkURL) ||
                            !string.IsNullOrEmpty(slider.SliderImageFileName)
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


        #region Blog

        public ActionResult EfarganPagesBlog()
        {
            return View();
        }
        #endregion
    }
}
