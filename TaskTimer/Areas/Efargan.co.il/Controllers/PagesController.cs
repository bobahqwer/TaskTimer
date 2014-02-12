using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskTimer.Models;
using TaskTimer.UsefulClasses;

namespace TaskTimer.Areas.Efargan.co.il.Controllers
{
    public class PagesController : Controller
    {
        //
        // GET: /Efargan.co.il/Pages/

        public ActionResult Index(int? id, string lng)
        {
            ViewBag.PageLng = lng;
            var db = new CustomMembershipDB();
            var viewPage = new EfarganPages();
            if (id == null)
                id = 0;
            if (lng.Equals("HE"))
            {
                viewPage = db.EfarganPages.SingleOrDefault(p => p.Id == id);
                if (viewPage == null)
                    viewPage = db.EfarganPages.FirstOrDefault(p => p.Id == 0);
                ViewBag.SearchBtn = "חיפוש";
            }
            else //get page in different language
            {
                ViewBag.SearchBtn = "Search";
                viewPage = db.EfarganPages.SingleOrDefault(p => p.Id == id);
                if (viewPage == null)
                    viewPage = db.EfarganPages.FirstOrDefault(p => p.Id == 0);
                var curLngPage = viewPage.EfarganPagesLanguage.FirstOrDefault(pl => pl.EfarganLanguage.LanguageValue == lng);
                if (curLngPage != null)
                {
                    viewPage = new EfarganPages()
                    {
                        MetaTag = curLngPage.MetaTag,
                        TitleTag = curLngPage.TitleTag,
                        DescriptionTag = curLngPage.DescriptionTag,
                        TicketTitle = curLngPage.TicketTitle,
                        TicketLinkText = curLngPage.TicketLinkText,
                        TicketLinkURL = curLngPage.TicketLinkURL,
                        EfarganPagesSliders = curLngPage.EfarganPagesSliders,
                        EfarganPagesParagraphs = curLngPage.EfarganPagesParagraphs,
                        EfarganPagesTickers = curLngPage.EfarganPagesTickers
                    };

                }
            }
            return View(viewPage);
        }
        public ActionResult _MainMenu(string lng)
        {
            //var lng = (string)ViewBag.PageLng;
            var db = new CustomMembershipDB();
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            if (lng.Equals("HE"))
                dictionary = db.EfarganPages.ToDictionary(p => p.Id, p => p.MenuText);
            else
            {   //get all pages in different language
                var curLng = db.EfarganLanguage.FirstOrDefault(l => l.LanguageValue == lng);
                if (curLng != null)
                {
                    var pagesLng = db.EfarganPagesLanguage.Where(pl => pl.EfarganLanguage.Id == curLng.Id);
                    if (pagesLng.Count() > 0)
                    {
                        dictionary = pagesLng.ToDictionary(p => p.Id, p => p.MenuText);
                    }
                }
            }
            return PartialView("_MainMenu", dictionary);
        }

        [AjaxOnly, HttpPost]
        public ActionResult SiteSearch(string key, string lng)
        {
            CustomMembershipDB db = new CustomMembershipDB();
            // search in DB
            //if (lng.Equals("HE"))
            //{
            var searchResultsSliders = db.EfarganPagesSliders.Where(pr => pr.SliderText.Contains(key)
                || pr.SliderTitle.Contains(key)
                || pr.SliderLinkText.Contains(key));
            var searchResultsParagraphs = db.EfarganPagesParagraphs.Where(pr => pr.ParagraphText.Contains(key)
                || pr.ParagraphTitle.Contains(key)
                || pr.ParagraphLinkText.Contains(key));
            var searchResultsTickers = db.EfarganPagesTickers.Where(pr => pr.TickerText.Contains(key)
                || pr.TickerTitle.Contains(key)
                || pr.TickerLinkText.Contains(key));
            //}
            //else //search in different language
            //{
            //    var searchResultsSliders = db.EfarganPagesSliders.Where(pr => pr.SliderText.Contains(key)
            //                       || pr.SliderTitle.Contains(key)
            //                       || pr.SliderLinkText.Contains(key));
            //    var searchResultsParagraphs = db.EfarganPagesParagraphs.Where(pr => pr.ParagraphText.Contains(key)
            //        || pr.ParagraphTitle.Contains(key)
            //        || pr.ParagraphLinkText.Contains(key));
            //    var searchResultsTickers = db.EfarganPagesTickers.Where(pr => pr.TickerText.Contains(key)
            //        || pr.TickerTitle.Contains(key)
            //        || pr.TickerLinkText.Contains(key));
            //}

            //get output
            var responseHtml = "";
            switch (lng)
            {
                case "HE":
                    responseHtml += "<b>תוצאות החיפוש</b>";
                    if (searchResultsSliders.Count() == 0 &&
                        searchResultsParagraphs.Count() == 0)
                        responseHtml += "<b>לא נמצאו תוצאות עבור '" + key + "'</b>";
                    break;
                case "ENG":
                    responseHtml += "<b>Search results</b>";
                    if (searchResultsSliders.Count() == 0 &&
                        searchResultsParagraphs.Count() == 0)
                        responseHtml += "<b>No results found for '" + key + "'</b>";
                    break;
                case "RU":
                    responseHtml += "<b>Результат поиска</b>";
                    if (searchResultsSliders.Count() == 0 &&
                        searchResultsParagraphs.Count() == 0)
                        responseHtml += "<b>Нечего небыло найдено для '" + key + "'</b>";
                    break;
                default:
                    responseHtml += "<b>תוצאות החיפוש</b>";
                    break;
            }


            var keyArr = new string[1] { key };
            var keyDistinguished = "<span class='searchResponseDistinguished'>" + key + "</span>";
            foreach (var item in searchResultsSliders)
            {
                responseHtml += "<div class='searchResponse'>";
                responseHtml += "<div class='searchResponseTitle'>";
                var responseTitle = item.SliderTitle;
                if (item.SliderTitle.Contains(key))
                {
                    var responseTitleArr = responseTitle.Split(key.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    responseTitle = responseTitleArr[0] + key + responseTitleArr[1];
                }
                responseHtml += responseTitle;
                responseHtml += "</div>";

                //item.SliderText
                responseHtml += "</div>";
            }


            foreach (var item in searchResultsParagraphs)
            {
                responseHtml += "<div class='searchResponse'>";


                //title
                responseHtml += "<div class='searchResponseTitle'>";
                var responseTmp = item.ParagraphTitle;
                if (item.ParagraphTitle != null && item.ParagraphTitle.Contains(key))
                {
                    var responseTitleArr = responseTmp.Split(keyArr, StringSplitOptions.RemoveEmptyEntries);
                    responseTmp = responseTitleArr[0];
                    for (int i = 1; i < responseTitleArr.Length; i++)
                        responseTmp += keyDistinguished + responseTitleArr[i];
                }
                responseHtml += responseTmp;
                responseHtml += "</div>";

                //text
                responseHtml += "<div class='searchResponseText'>";
                responseTmp = item.ParagraphText;
                if (item.ParagraphText != null && item.ParagraphText.Contains(key))
                {
                    var responseTitleArr = responseTmp.Split(keyArr, StringSplitOptions.RemoveEmptyEntries);
                    responseTmp = responseTitleArr[0];
                    for (int i = 1; i < responseTitleArr.Length; i++)
                        responseTmp += keyDistinguished + responseTitleArr[i];
                }
                responseHtml += responseTmp;
                responseHtml += "</div>";

                //link
                responseHtml += "<div class='searchResponseLink'>";
                responseTmp = item.ParagraphLinkText;
                if (item.ParagraphLinkText != null && item.ParagraphLinkText.Contains(key))
                {
                    var responseTitleArr = responseTmp.Split(keyArr, StringSplitOptions.RemoveEmptyEntries);
                    responseTmp = responseTitleArr[0];
                    for (int i = 1; i < responseTitleArr.Length; i++)
                        responseTmp += keyDistinguished + responseTitleArr[i];
                }
                responseHtml += responseTmp;
                responseHtml += "</div>";


                responseHtml += "</div>";
            }


            return Content(responseHtml);
        }

    }
}
