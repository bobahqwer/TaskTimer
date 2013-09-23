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

        public ActionResult Index(int id)
        {
            var db = new CustomMembershipDB();
            var curPage = db.EfarganPages.SingleOrDefault(p => p.Id == id);
            if (curPage == null)
                return View(new EfarganPages());
            return View(curPage);
        }



        [AjaxOnly, HttpPost]
        public ActionResult SiteSearch(string key)
        {
            CustomMembershipDB db = new CustomMembershipDB();
            var searchResultsSliders = db.EfarganPagesSliders.Where(pr => pr.SliderText.Contains(key)
                || pr.SliderTitle.Contains(key)
                || pr.SliderLinkText.Contains(key));
            var searchResultsParagraphs = db.EfarganPagesParagraphs.Where(pr => pr.ParagraphText.Contains(key)
                || pr.ParagraphTitle.Contains(key)
                || pr.ParagraphLinkText.Contains(key));
            var searchResultsTickers = db.EfarganPagesTickers.Where(pr => pr.TickerText.Contains(key)
                || pr.TickerTitle.Contains(key)
                || pr.TickerLinkText.Contains(key));
            var responseHtml = "";
            responseHtml += "<b>תוצאות החיפוש</b>";

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
