using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TaskTimer.Models;

namespace TaskTimer.Areas.Admin_old.Models
{
    public class User
    {
        [Display(Name = "User Id")]
        public int UserId { get; set; }
        [Display(Name = "User Name")]
        [Required]
        public string UserName { get; set; }
        [Display(Name = "Email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Comments")]
        public string Comments { get; set; }
        [Display(Name = "Created")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Last Modified")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? LastModifiedDate { get; set; }
        [Display(Name = "Last Login")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LastLoginDate { get; set; }
        [Display(Name = "Last Login Ip")]
        public string LastLoginIp { get; set; }
        [Display(Name = "Is Activated")]
        public bool IsActivated { get; set; }
        [Display(Name = "Is Locked Out")]
        public bool IsLockedOut { get; set; }
        [Display(Name = "Last Locked Out")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LastLockedOutDate { get; set; }
        [Display(Name = "Last Locked Out Reason")]
        public string LastLockedOutReason { get; set; }
        [Display(Name = "New Password Key")]
        public string NewPasswordKey { get; set; }
        [Display(Name = "New Password Requested")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NewPasswordRequested { get; set; }
        [Display(Name = "New Email")]
        public string NewEmail { get; set; }
        [Display(Name = "New Email Key")]
        public string NewEmailKey { get; set; }
        [Display(Name = "New Email Requested")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NewEmailRequested { get; set; }

        [Display(Name = "User roles")]
        public List<Roles> Roles { get; set; }

        public string errorMsg { get; set; }

    }

    public class Image
    {

    }
}