using System;
using System.Drawing;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using PagedList;
using CustomMembership.Models;
using TaskTimer.Models;
using TaskTimer.UsefulClasses;
using System.IO;

namespace TaskTimer.Controllers
{
    public class FAQController : Controller
    {
        private string dirName
        {
            get { return Server.MapPath("~/Images/Uploaded/"); }
        }
            
        // GET: /FAQ/
        [AllowAnonymous]
        public ActionResult Index(int? page)
        {
            ViewBag.Message = "Questions list.";
            CustomMembershipDB db = new CustomMembershipDB();
            var listCustomer = db.FAQ.Where(f => f.Active == true).OrderByDescending(i => i.ID).ToList();
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(listCustomer.ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        public ActionResult Index(string answer, int id)
        {
            string _answer = HttpUtility.UrlDecode(answer, System.Text.Encoding.Default);
            CustomMembershipDB db = new CustomMembershipDB();
            var question = db.FAQ.SingleOrDefault(q => q.ID == id);
            if (question != null)
            {
                question.Answer = _answer;
                if (question.NotifyByEmail && !String.IsNullOrEmpty(question.NotifyEmail))
                {
                    var msgTitle = "dirot.somee.com. You got the answer on your question.";
                    var msgBody = "Hello. You got the answer to the question that you asked in www.dirot.somee.com." +
                        " Please follow the <a href='www.dirot.somee.com/FAQ' target='_blank'>link</a> to see the answer.";
                    UserRepository.SendEmailThroughGmail(msgTitle, msgBody, question.NotifyEmail);
                    question.NotifyByEmail = false;
                }
                db.SaveChanges();
            }
            else
            {
                //add error
            }
            //return Json(answer, "text/html");
            return Content(_answer, "text/html");
        }

        [AjaxOnly, HttpPost]
        public ActionResult Delete(int id)
        {
            CustomMembershipDB db = new CustomMembershipDB();
            var question = db.FAQ.SingleOrDefault(q => q.ID == id);
            if (question != null)
            {
                question.Active = false;
                db.SaveChanges();
            }
            return Content("");
        }
        [AjaxOnly, HttpPost]
        public ActionResult DeletePermanently(int id)
        {
            CustomMembershipDB db = new CustomMembershipDB();
            var question = db.FAQ.SingleOrDefault(q => q.ID == id);
            if (question != null)
            {
                var images = question.Images.ToList();
                for (int i = 0; i < images.Count; i++)
			    {
                    Helper.RemoveFile(dirName, images[i].Name); // remove image from server
                    db.Images.Remove(images[i]);                // remove image from db
			    }
                db.FAQ.Remove(question);
                db.SaveChanges();
            }
            return Content("");
        }

        [AllowAnonymous]
        public ActionResult AddNew()
        {
            ViewBag.Message = "Add new question.";
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult AddNew(FAQModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //prepere question
                    var question = HttpUtility.UrlDecode(model.Question, System.Text.Encoding.Default);
                    question = question.Replace("/Temporary", ""); //new file location
                    model.ImageNames = model.ImageNames[0].Split(',');

                    //add new FAQ to db
                    int? QUserId = null;
                    string NotifyEmail = model.NotifyEmail;
                    CustomMembershipDB db = new CustomMembershipDB();
                    var curUserId = db.Users.SingleOrDefault(u => u.UserName.Equals(User.Identity.Name));
                    if (curUserId != null)
                    {
                        QUserId = curUserId.UserId;
                        NotifyEmail = String.IsNullOrEmpty(model.NotifyEmail) ? curUserId.Email : model.NotifyEmail;
                    }
                    var newFAQ = new FAQ
                    {
                        QUserId = QUserId,
                        Date = DateTime.Now,
                        Title = model.Title,
                        Question = question,
                        NotifyByEmail = model.NotifyByEmail,
                        NotifyEmail = NotifyEmail,
                        Active = true
                    };
                    db.FAQ.Add(newFAQ);

                    //check if all uploaded images appear in question
                    Helper.CheckQuestionImages(dirName, question, model.ImageNames);
                    //add images data
                    foreach (var imageName in model.ImageNames)
                    {
                        db.Images.Add(new Images
                        {
                            FAQ = newFAQ,
                            Name = imageName
                        });
                    }
                    db.SaveChanges();

                    //work with files
                    Helper.MoveFiles(dirName, model.ImageNames);

                    ViewBag.AddQuestionStatus = "Question added.";
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "There was an error during save of question.");
                    return View();
                }
               
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// upload one file
        /// </summary>
        [HttpPost]
        public ContentResult Upload()
        {
            string imageWidth = "0", imageLink = "0", imageName = "";
            if (Request.Files.Count == 1) 
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                imageName = Helper.GetUniqueFileName(dirName, hpf.FileName);
                string savedFileName = Path.Combine(dirName + "Temporary", Path.GetFileName(imageName));
                hpf.SaveAs(savedFileName); // Save the file
                using (var im = Image.FromFile(savedFileName))
                {
                    imageWidth = im.Width.ToString();
                }
                imageLink = "/Images/Uploaded/Temporary/" + imageName;
            }
            return Content("{\"upload\": { \"image\": { \"width\":\"" + imageWidth + "\", \"name\":\"" + imageName + "\"}, \"links\":{ \"original\":\"" + imageLink + "\"} } }", "content-type:application/x-javascript");
        }

        /// <summary>
        /// upload multiple files
        /// </summary>
        /*
        public class ViewDataUploadFilesResult
        {
            public string Name { get; set; }
            public int Length { get; set; }
            public string Type { get; set; }
        }
        [HttpPost]
        public ContentResult Upload()
        {
            var r = new List<ViewDataUploadFilesResult>();

            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                if (hpf.ContentLength == 0)
                    continue;

                string savedFileName = Path.Combine(Server.MapPath("~/App_Data"), Path.GetFileName(hpf.FileName));
                hpf.SaveAs(savedFileName); // Save the file

                r.Add(new ViewDataUploadFilesResult()
                {
                    Name = hpf.FileName,
                    Length = hpf.ContentLength,
                    Type = hpf.ContentType
                });
            }
            // Returns json
            return Content("{\"name\":\"" + r[0].Name + "\",\"type\":\"" + r[0].Type + "\",\"size\":\"" + string.Format("{0} bytes", r[0].Length) + "\"}", "application/json");
            //return Content("qwer1");
        }
        */
    }
}