using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskTimer.Areas.Admin.Models;
using TaskTimer.Models;

namespace TaskTimer.Areas.Admin.UsefulClasses
{
    public class ModelsHelper
    {
        public static User GetUserFromTable(TaskTimer.Models.Users user)
        {
            return new User
                {
                    Comments = user.Comments,
                    CreatedDate = user.CreatedDate,
                    Email = user.Email,
                    IsActivated = user.IsActivated,
                    IsLockedOut = user.IsLockedOut,
                    LastLockedOutDate = user.LastLockedOutDate,
                    LastLockedOutReason = user.LastLockedOutReason,
                    LastLoginDate = user.LastLoginDate,
                    LastLoginIp = user.LastLoginIp,
                    LastModifiedDate = user.LastModifiedDate,
                    NewEmail = user.NewEmail,
                    NewEmailKey = user.NewEmailKey,
                    NewEmailRequested = user.NewEmailRequested,
                    NewPasswordKey = user.NewPasswordKey,
                    NewPasswordRequested = user.NewPasswordRequested,
                    Roles = user.Roles.ToList(),
                    UserId = user.UserId,
                    UserName = user.UserName
                };
        }
        public static List<User> GetAllUsersTable() 
        {
            var users = new List<User>();
            var db = new CustomMembershipDB();
            foreach (var user in db.Users)
                users.Add(GetUserFromTable(user));
            return users;
        }



    }
}