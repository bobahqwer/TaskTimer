using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using TaskTimer.Areas.Admin_old.Models;
using TaskTimer.Areas.Admin_old.UsefulClasses;
using TaskTimer.Models;
using TaskTimer.UsefulClasses;

namespace TaskTimer.Areas.Admin_old.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        //[Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var db = new CustomMembershipDB();
            List<SelectListItem> sl = new SelectList((IEnumerable<Roles>)db.Roles, "RoleName", "RoleName").ToList();
            sl.Insert(0, new SelectListItem { Text = "---", Value = "0" });
            ViewBag.Roles = sl;
            return View(ModelsHelper.GetAllUsersTable());
        }


        [HttpGet]
        public PartialViewResult Main()
        {
            return PartialView("_MainPartial");
        }

        [HttpGet]
        public PartialViewResult Users()
        {
            var db = new CustomMembershipDB();
            List<SelectListItem> sl = new SelectList((IEnumerable<Roles>)db.Roles, "RoleName", "RoleName").ToList();
            sl.Insert(0, new SelectListItem { Text = "---", Value = "0" });
            ViewBag.Roles = sl;
            return PartialView("_UsersPartial", ModelsHelper.GetAllUsersTable());
            //return PartialView("_UsersPartial", db.Users);
        }

        [HttpGet]
        public PartialViewResult Images()
        {
            CustomMembershipDB db = new CustomMembershipDB();
            return PartialView("_ImagesPartial", db.Images);
        }

        [HttpGet]
        public PartialViewResult Pages()
        {
            CustomMembershipDB db = new CustomMembershipDB();
            return PartialView("_PagesPartial", db.SiteMainPages);
        }

        [HttpGet]
        public PartialViewResult AddNewPage()
        {
            SiteMainPages newPage = new SiteMainPages();
            return PartialView("_PagesNewPartial", newPage);
        }

        [HttpPost]
        public void AddNewPage(SiteMainPages newPage)
        {
            var db = new CustomMembershipDB();
            db.SiteMainPages.Add(newPage);
            //db.SaveChanges();
            var file = Request.Form["TitleImage"];
            //RedirectToAction("Index", "AdminController");
            //return PartialView("_PagesPartial", db.SiteMainPages);
        }


        //[HttpPost]
        //public ActionResult AddNewPageAjax(SiteMainPages newPage)
        //{
        //    var db = new CustomMembershipDB();
        //    db.SiteMainPages.Add(newPage);
        //    //db.SaveChanges();
        //    var file = Request.Form["TitleImage"];
        //    return Content("contetnt result", "text/html");
        //    //RedirectToAction("Index", "AdminController");
        //    //return PartialView("_PagesPartial", db.SiteMainPages);
        //}


        [HttpPost]
        public ActionResult AddNewPageAjax(SiteMainPages formCollection)
        {
            var file = Request.Form["TitleImage"];
            return Content("contetnt result", "text/html");
            //RedirectToAction("Index", "AdminController");
            //return PartialView("_PagesPartial", db.SiteMainPages);
        }

        //public PartialViewResult AddNewPage() 
        //{
        //    return PartialView("_NewPagePartial");
        //}

        //[HttpPost]
        //public PartialViewResult Edit(TaskTimer.Models.Users model)//TaskTimer.Areas.Admin.Models.User model)
        //{
        //    return PartialView("_UserPartial", new TaskTimer.Areas.Admin.Models.User());
        //}
        //[HttpPost]
        //public PartialViewResult Edit(System.Data.Entity.DbSet<TaskTimer.Models.Users> model)//TaskTimer.Areas.Admin.Models.User model)
        //{

        //    return PartialView("_UserPartial", new TaskTimer.Areas.Admin.Models.User());
        //}
        //[HttpPost]
        //public PartialViewResult Edit(System.Data.Entity.DbSet<TaskTimer.Models.Users> users, System.Data.Entity.DbSet<TaskTimer.Models.Users> model)//TaskTimer.Areas.Admin.Models.User model)
        //{
        //    //var db = new CustomMembershipDB();
        //    //var user = db.Users.FirstOrDefault(u => u.UserId == id);
        //    //if (user != null)
        //    //{
        //    //    TryUpdateModel(user);
        //    //    return PartialView("_UserPartial", user);
        //    //}
        //    return PartialView("_UserPartial", new TaskTimer.Areas.Admin.Models.User());
        //}
        [HttpPost]
        public PartialViewResult Edit(FormCollection formCollection)
        {

            var userID = int.Parse(formCollection["item.UserId"]);
            var db = new CustomMembershipDB();
            var user = db.Users.SingleOrDefault(u => u.UserId == userID);
            if(user != null)
            {
                user.UserName = String.IsNullOrEmpty(formCollection["item.UserName"]) ? user.UserName : formCollection["item.UserName"];
                user.Email = String.IsNullOrEmpty(formCollection["item.Email"]) ? user.Email : formCollection["item.Email"];
                DateTime createdDate = user.CreatedDate;
                if (DateTime.TryParseExact(formCollection["item.CreatedDate"], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out createdDate))
                    user.CreatedDate = createdDate;
                user.IsActivated = String.IsNullOrEmpty(formCollection["item.IsActivated"]) ? user.IsActivated : formCollection["item.IsActivated"].Any(act => act == 't');
                user.IsLockedOut = String.IsNullOrEmpty(formCollection["item.IsLockedOut"]) ? user.IsLockedOut : formCollection["item.IsLockedOut"].Any(loc => loc == 't');
                user.LastLockedOutReason = formCollection["item.LastLockedOutReason"] == null ? user.LastLockedOutReason : formCollection["item.LastLockedOutReason"];
                DateTime lastLockedOutDate = user.LastLockedOutDate;
                if (DateTime.TryParseExact(formCollection["item.LastLockedOutDate"], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out lastLockedOutDate))
                    user.LastLockedOutDate = lastLockedOutDate;
                if (!formCollection["item.RoleName"].Equals("0") && !user.Roles.Any(r => r.RoleName == formCollection["item.RoleName"])) 
                {
                    var RoleService = new AccountRoleService();
                    RoleService.AddUsersToRoles(new string[] { user.UserName }, new string[] { formCollection["item.RoleName"] });
                }
                db.SaveChanges();
            }
            return PartialView("_UserPartial", ModelsHelper.GetUserFromTable(user));

          //  return PartialView("_UserPartial", new TaskTimer.Areas.Admin.Models.User());
        }
        [HttpPost]
        public PartialViewResult Delete(string id)
        {
            var userID = int.Parse(id);
            var db = new CustomMembershipDB();
            var user = db.Users.SingleOrDefault(u => u.UserId == userID);
            if (user != null)
            {
                db.Users.Remove(user);
                //db.SaveChanges();
            }
            List<SelectListItem> sl = new SelectList((IEnumerable<Roles>)db.Roles, "RoleName", "RoleName").ToList();
            sl.Insert(0, new SelectListItem { Text = "---", Value = "0" });
            ViewBag.Roles = sl;
            return PartialView("_UsersPartial", ModelsHelper.GetAllUsersTable());
        }
        //[HttpPost]
        //public PartialViewResult Edit(string userID)
        //{



        //    return PartialView("_UserPartial", new TaskTimer.Areas.Admin.Models.User());
        //}
        //[HttpPost]
        //public PartialViewResult Edit(string userID, TaskTimer.Models.Users model)
        //{



        //    return PartialView("_UserPartial", new TaskTimer.Areas.Admin.Models.User());
        //}


        //[AjaxOnly, HttpPost]
        //public JsonResult Users()
        //{


        //    List<User> usersResultModel = new List<User>();
	


        //    CustomMembershipDB db = new CustomMembershipDB();
        //    //var user = db.Users.FirstOrDefault();

        //    foreach (var user in db.Users)
        //    {
        //        User userResultModel = new User();
        //        userResultModel.UserId = user.UserId;
        //        userResultModel.UserName = user.UserName;
        //        userResultModel.Email = user.Email;
        //        userResultModel.Comments = user.Comments;
        //        userResultModel.CreatedDate = user.CreatedDate;
        //        userResultModel.LastModifiedDate = user.LastModifiedDate;
        //        userResultModel.LastLoginDate = user.LastLoginDate;
        //        userResultModel.LastLoginIp = user.LastLoginIp;



        //        usersResultModel.Add(userResultModel);
        //    }


        //    return Json(usersResultModel);
        //}


        //[AjaxOnly, HttpPost]
        //public ActionResult Images()
        //{
        //   /* CustomMembershipDB db = new CustomMembershipDB();
        //    var question = db.FAQ.SingleOrDefault(q => q.ID == id);
        //    if (question != null)
        //    {
        //        var images = question.Images.ToList();
        //        for (int i = 0; i < images.Count; i++)
        //        {
        //            Helper.RemoveFile(dirName, images[i].Name); // remove image from server
        //            db.Images.Remove(images[i]);                // remove image from db
        //        }
        //        db.FAQ.Remove(question);
        //        db.SaveChanges();
        //    }*/
        //    return Content("from Images");
        //}
    }
}
