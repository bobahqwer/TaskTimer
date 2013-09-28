using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskTimer.Models;

namespace TaskTimer.Areas.Admin.Models
{
    public class AdminEfarganHelper
    {
        public static readonly string PageImagesPath = "Images/Page Images/";
        private static List<SelectListItem> siteLanguages;
        public static List<SelectListItem> SiteLanguages
        {
            get 
            {
                siteLanguages = new List<SelectListItem>();
                siteLanguages.Add(new SelectListItem() { 
                    Text = "---",
                    Value = "-1",
                    Selected = true
                });
                foreach (var item in new CustomMembershipDB().EfarganLanguage)
                {
                    siteLanguages.Add(new SelectListItem() {
                        Value = item.LanguageValue,
                        Text = item.LanguageName
                    });
                }
                return siteLanguages; 
            }
            set { AdminEfarganHelper.siteLanguages = value; }
        }



        public static List<SelectListItem> GetEfarganLanguageSelectListItem(int id)
        {
            var db = new CustomMembershipDB();
            return db.EfarganLanguage.Select(
                lng => new SelectListItem
                {
                    Text = lng.LanguageName,
                    Value = lng.LanguageValue,
                    Selected = id == lng.Id
                }).ToList();;
        }

        public static string SavePageImage(HttpPostedFileBase file)
        {
            var saveToDirectory = AppDomain.CurrentDomain.BaseDirectory + PageImagesPath;
            if (file.ContentLength > 0)
            {
                var saveFileName = Path.GetFileName(file.FileName);
                saveFileName = AdminHelper.GetUniqueFileName(saveToDirectory, saveFileName);
                string savedFileName = Path.Combine(
                  saveToDirectory,
                  saveFileName);
                file.SaveAs(savedFileName);
                return savedFileName;
            }
            return "";
        }

        //public static bool DeleteOldPageImage(EfarganPagesSliders file)
        //{



        //    return false;
        //}
    }
}