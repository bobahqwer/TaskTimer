using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskTimer.Models;

namespace TaskTimer.Areas.Admin.Models
{
    public class AdminEfarganHelper
    {
        private static EfarganLanguage siteLanguages;
        public static EfarganLanguage SiteLanguages
        {
            get 
            {
                var asd = new CustomMembershipDB();
                asd.EfarganPagesLanguage;
                siteLanguages = new CustomMembershipDB().EfarganLanguage.Select();
                return siteLanguages; 
            }
            set { AdminEfarganHelper.siteLanguages = value; }
        }


        AdminEfarganHelper() {
            var db = new CustomMembershipDB();
        }
    }
    
}