using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Resources.Models.Account;

namespace TaskTimer.Models
{
    public class FAQModel
    {
        private string _Title;
        [Display(Name = "Title")]
        [StringLength(100, ErrorMessage = "The {0} must be at most {1} characters long.")]
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        private string _Question;
        [Required(ErrorMessageResourceType = typeof(ValidationStrings), ErrorMessageResourceName = "TheXFieldIsКequired")]
        [System.Web.Mvc.AllowHtml]
        [Display(Name = "Question")]
        [StringLength(10000, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 10)]
        public string Question
        {
            get { return _Question; }
            set { _Question = value; }
        }

        private string _Answer;
        [System.Web.Mvc.AllowHtml]
        [Display(Name = "Answer")]
        public string Answer
        {
            get { return _Answer; }
            set { _Answer = value; }
        }

        private DateTime _Date;
        [Display(Name = "Date:")]
        public DateTime Date
        {
            get { return _Date; }
            set { _Date = value; }
        }

        private string _QUser;
        [Display(Name = "Question of:")]
        public string QUser
        {
            get { return _QUser; }
            set { _QUser = value; }
        }

        private string _AUser;
        [Display(Name = "Answer of:")]
        public string AUser
        {
            get { return _AUser; }
            set { _AUser = value; }
        }

        private bool _NotifyByEmail;
        [Display(Name = "Notify by email")]
        public bool NotifyByEmail
        {
            get { return _NotifyByEmail; }
            set { _NotifyByEmail = value; }
        }

        private string _NotifyEmail;
        [EmailAddress(ErrorMessage = "Email address is not valid")]
        [Display(Name = "Email")]
        public string NotifyEmail
        {
            get { return _NotifyEmail; }
            set { _NotifyEmail = value; }
        }

        private string[] _ImageNames;
        public string[] ImageNames
        {
            get { return _ImageNames; }
            set { _ImageNames = value; }
        }
    }
}