using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Security;
using CustomMembership.Models;
using TaskTimer.Security;
using Resources.Models.Account;

namespace TaskTimer.Models
{

    public class ChangePasswordModel
    {
        [Required(ErrorMessageResourceType = typeof(ValidationStrings), ErrorMessageResourceName = "TheXFieldIsКequired")]
        [DataType(DataType.Password)]
        [Display(Name = "CurrentPassword", ResourceType = typeof(FieldsNames))]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationStrings), ErrorMessageResourceName = "TheXFieldIsКequired")]
        [StringLength(100, ErrorMessageResourceType = typeof(ValidationStrings), ErrorMessageResourceName = "PasswordsMustBeAtLeast", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "NewPassword", ResourceType = typeof(FieldsNames))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmNewPassword", ResourceType = typeof(FieldsNames))]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(ValidationStrings), ErrorMessageResourceName = "TheNewPasswordAndConfirmationPasswordDoNotMatch")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required(ErrorMessageResourceType = typeof(ValidationStrings), ErrorMessageResourceName = "TheXFieldIsКequired")]
        [Display(Name = "UserName", ResourceType = typeof(FieldsNames))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationStrings), ErrorMessageResourceName = "TheXFieldIsКequired")]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(FieldsNames))]
        public string Password { get; set; }

        [Display(Name = "RememberMe", ResourceType = typeof(FieldsNames))]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required(ErrorMessageResourceType = typeof(ValidationStrings), ErrorMessageResourceName = "TheXFieldIsКequired")]
        [Display(Name = "UserName", ResourceType = typeof(FieldsNames))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationStrings), ErrorMessageResourceName = "TheXFieldIsКequired")]
        [EmailAddress(ErrorMessage = null, ErrorMessageResourceType = typeof(ValidationStrings), ErrorMessageResourceName = "EmailAddressIsNotValid")]
        [Display(Name = "EmailAddress", ResourceType = typeof(FieldsNames))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationStrings), ErrorMessageResourceName = "TheXFieldIsКequired")]
        [StringLength(100, ErrorMessageResourceType = typeof(ValidationStrings), ErrorMessageResourceName = "PasswordsMustBeAtLeast", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(FieldsNames))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(FieldsNames))]
        [Compare("Password", ErrorMessageResourceType = typeof(ValidationStrings), ErrorMessageResourceName = "TheNewPasswordAndConfirmationPasswordDoNotMatch")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "ActivateAccountByEmail", ResourceType = typeof(FieldsNames))]
        public bool IsEmailRegConform { get; set; }
    }

    public interface IRoleService
    {
        bool AdminExists();
        void AddUsersToRoles(string[] usernames, string[] rolenames);
        void RemoveUsersFromRoles(string[] usernames, string[] rolenames);
        void CreateRole(string roleName);
    }
    public class AccountRoleService : IRoleService
    {
        private readonly RoleProvider _provider;
        private UserRepository userRepo = new UserRepository();
        public AccountRoleService()
            : this(null)
        {
        }

        public AccountRoleService(RoleProvider provider)
        {
            _provider = provider ?? new MyRoleProvider();
        }

        public bool AdminExists()
        {
            var users = _provider.GetUsersInRole("Admin");

            if (users.Length == 0)
                return false;

            return true;
        }

        //ADD
        public void AddUsersToRoles(string[] usernames, string[] rolenames)
        {
            _provider.AddUsersToRoles(usernames, rolenames);
        }

        public void RemoveUsersFromRoles(string[] usernames, string[] rolenames)
        {
            _provider.RemoveUsersFromRoles(usernames, rolenames);
        }

        public void CreateRole(string roleName)
        {
            _provider.CreateRole(roleName);
        }
    }
           

}
