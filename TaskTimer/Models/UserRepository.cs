﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Security.Cryptography;
using TaskTimer.Models;
using System.Net;
using System.Net.Mail;
using TaskTimer.UsefulClasses;

namespace CustomMembership.Models
{
    public class UserRepository
    {
        private CustomMembershipDB _db;
        public CustomMembershipDB db
        {
            get
            {
                if (_db == null) _db = new CustomMembershipDB();
                //_db = new CustomMembershipDB();
                return _db;
            }
        }

        public MembershipUser CreateUser(string username, string password, string email, bool IsEmailConformation)
        {
            var user = db.Users.Add(new Users
            {
                UserName = username,
                Email = email,
                PasswordSalt = CreateSalt(),
                CreatedDate = DateTime.Now,
                IsActivated = false,
                IsLockedOut = false,
                LastLockedOutDate = DateTime.Now,
                LastLoginDate = DateTime.Now,
                NewEmailKey = GenerateKey()
            });
            user.Password = CreatePasswordHash(password, user.PasswordSalt);

            if (IsEmailConformation)    //send email
            {
                string activationLink = Helper.ResolveServerUrl("/Account/Activate/", false);
                activationLink += user.UserName + "/" + user.NewEmailKey;
                SendEmailThroughGmail("Account activation", activationLink, user.Email);
            }
            else 
            {
                user.IsActivated = true;
            }

            db.SaveChanges();
            return GetUser(username);
        }
        
        public string GetUserNameByEmail(string email)
        {
            var user = db.Users.FirstOrDefault(u => u.Email == email);
            return (user != null) ? user.UserName : string.Empty;
        }

        public MembershipUser GetUser(string username)
        {
            var dbuser = db.Users.FirstOrDefault( u => u.UserName == username);
            if (dbuser != null)
            {
                string _username = dbuser.UserName;
                int _providerUserKey = dbuser.UserId;
                string _email = dbuser.Email;
                string _passwordQuestion = string.Empty;
                string _comment = dbuser.Comments;
                bool _isApproved = dbuser.IsActivated;
                bool _isLockedOut = dbuser.IsLockedOut;
                DateTime _creationDate = dbuser.CreatedDate;
                DateTime _lastLoginDate = dbuser.LastLoginDate;
                DateTime _lastActivityDate = DateTime.Now;
                DateTime _lastPasswordChangedDate = DateTime.Now;
                DateTime _lastLockedOutDate = dbuser.LastLockedOutDate;

                MembershipUser user = new MembershipUser("CustomMembershipProvider",
                                                          _username,
                                                          _providerUserKey,
                                                          _email,
                                                          _passwordQuestion,
                                                          _comment,
                                                          _isApproved,
                                                          _isLockedOut,
                                                          _creationDate,
                                                          _lastLoginDate,
                                                          _lastActivityDate,
                                                          _lastPasswordChangedDate,
                                                          _lastLockedOutDate);
                return user;
            }
            return null;
        }

        public void UpdateUser(MembershipUser username)
        { 
            var dbuser = db.Users.FirstOrDefault( u => u.UserName == username.UserName);
            if (dbuser != null)
            {
                dbuser.UserName = username.UserName;
                dbuser.Email = username.Email;
                dbuser.Comments = username.Comment;
                dbuser.IsActivated = username.IsApproved;
                dbuser.IsLockedOut = username.IsLockedOut;
                dbuser.LastModifiedDate = DateTime.Now;
                db.SaveChanges();
            }
        }
        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            var dbuser = db.Users.FirstOrDefault( u => u.UserName == username);
            if (dbuser != null && dbuser.Password == CreatePasswordHash(oldPassword, dbuser.PasswordSalt))
            {
                dbuser.Password = CreatePasswordHash(newPassword, dbuser.PasswordSalt);
                db.SaveChanges();
                return true;
            }
            return false;
        }
        //password + salt
        private static string CreateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[32];
            rng.GetBytes(buff);

            return Convert.ToBase64String(buff);
        }
        private static string CreatePasswordHash(string pwd, string salt)
        {
            string saltAndPwd = String.Concat(pwd, salt);
            string hashedPwd =
                    FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPwd, "md5");
            return hashedPwd;
        }
        public bool ValidateUser(string username, string password)
        {
            var dbuser = db.Users.FirstOrDefault(x => x.UserName == username);
            return dbuser != null && dbuser.Password == CreatePasswordHash(password, dbuser.PasswordSalt) && dbuser.IsActivated;
        }

        //for email conformation
      /*  public bool ActivateUser(string username, string key)
        {
            var dbuser = db.Users.FirstOrDefault(x => x.UserName == username);
            if (dbuser != null && dbuser.NewEmailKey == key)
            {
                dbuser.IsActivated = true;
                dbuser.LastModifiedDate = DateTime.Now;
                dbuser.NewEmailKey = null;
                db.SaveChanges();
                return true;
            }
            return false;
        }*/

        //roles provider
        public Users GetDBUser(string username)
        {
            return db.Users.SingleOrDefault(x => x.UserName == username);
        }
        public TaskTimer.Models.Roles GetRole(string name)
        {
            return db.Roles.SingleOrDefault(x => x.RoleName == name);
        }
        public List<Users> GetAllUsers()
        {
            return db.Users.ToList();
        }
        public void AddUsersToRoles(string[] usernames, string[] rolenames)
        {
            foreach (var username in usernames)
            {
                var user = GetDBUser(username);
                if (user != null)
                {
                    foreach (var rolename in rolenames)
                    {
                        var role = GetRole(rolename);
                        if (role != null)
                            if (!user.Roles.Contains(role))
                                user.Roles.Add(role);
                    }
                }
            }
            db.SaveChanges();
        }
        public void CreateRole(string roleName)
        {
            if (GetRole(roleName) == null)
            {
                db.Roles.Add(new TaskTimer.Models.Roles { RoleId = Guid.NewGuid(), RoleName = roleName });
                db.SaveChanges();
            }
        }

        //help functions
        private static string GenerateKey()
        {
            return Guid.NewGuid().ToString();
        }
        public static void SendEmailThroughGmail(string messageTitle, string messageBody, string emailTo)
        {
            SmtpClient client = new SmtpClient();
            NetworkCredential basicAuthenticationInfo =
                new NetworkCredential("email.for.smtp2@gmail.com", "smtpsmtp");
            client.Host = "smtp.gmail.com";
            client.UseDefaultCredentials = false;
            client.Credentials = basicAuthenticationInfo;
            client.EnableSsl = true;

            MailAddress to = new MailAddress(emailTo);
            MailAddress from = new MailAddress("email.for.smtp2@gmail.com", messageTitle, System.Text.Encoding.UTF8);

            MailMessage message = new MailMessage(from, to);
            message.Body = messageBody;
            message.IsBodyHtml = true;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Subject = "Account activation";
            message.SubjectEncoding = System.Text.Encoding.UTF8;

            client.Send(message);
        }
    }
}