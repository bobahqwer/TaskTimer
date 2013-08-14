using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Google.GData.Client;
using Resources.Models.Account;

namespace TaskTimer.Models
{
    public class GoogleAPIAccess : OAuth2Parameters
    {
        [Required(ErrorMessageResourceType = typeof(ValidationStrings), ErrorMessageResourceName = "TheXFieldIsКequired")]
        [Display(Name = "Client ID")]
        public string _ClientID 
        {
            get
            {
                return this.ClientId;
            }
            set
            {
                this.ClientId = value;
            }
        }

        [Required(ErrorMessageResourceType = typeof(ValidationStrings), ErrorMessageResourceName = "TheXFieldIsКequired")]
        [Display(Name = "Client secret")]
        public string _ClientSecret 
        {
            get
            {
                return this.ClientSecret;
            }
            set
            {
                this.ClientSecret = value;
            }
        }
          
        [Display(Name = "Redirect URIs")]
        public string _RedirectURIs
        {
            get
            {
                return this.RedirectUri;
            }
            set
            {
                this.RedirectUri = value;
            }
        }

        [Display(Name = "SCOPE")]
        public string _Scope
        {
            get
            {
                return this.Scope;
            }
            set
            {
                this.Scope = value;
            }
        }

        [Display(Name = "Key of confirmation")]
        public string _AccessCode
        {
            get
            {
                return this.AccessCode;
            }
            set
            {
                this.AccessCode = value;
            }
        }
    }
}