using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using FSDP.DATA.EF;
using Microsoft.AspNet.Identity;

namespace FSDP.UI.MVC.Controllers
{
    public class LessonsController : Controller
    {
        private FSDPEntities db = new FSDPEntities();

        // GET: Lessons
        public ActionResult Index(int? id)
        {
            Cours lesson = db.Courses.Find(id);
            var lessons = db.Lessons.Where(l => l.CourseID == lesson.CourseID);
            ViewBag.id = id;

            return View(lessons.ToList());
        }

        // GET: Lessons/Details/5
        public ActionResult Details(int? id, bool? completed)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = db.Lessons.Find(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            if (lesson.VideoURL != null)
            {
                var v = lesson.VideoURL.IndexOf("v=");
                var amp = lesson.VideoURL.IndexOf("&", v);
                string vid;
                //if the video id is the last value in the url
                if (amp == -1)
                {
                    vid = lesson.VideoURL.Substring(v + 2);
                    // if there are other parameters after the video id in the url
                }
                else
                {
                    vid = lesson.VideoURL.Substring(v + 2, amp - (v + 2));
                }
                ViewBag.VideoID = vid;
            }

            if (User.IsInRole("Employee") && completed == true)
            {
                LessonView viewed = new LessonView
                {
                    UserID = User.Identity.GetUserId(),
                    DateViewed = DateTime.Now,
                    LessonID = (int)id
                };
                db.LessonViews.Add(viewed);
                db.SaveChanges();
                //if(db.LessonViews.Where(l => l.UserID == viewed.UserID && l.Lesson.CourseID == lesson.CourseID).Count() == db.Lessons.Where(l => l.CourseID == lesson.CourseID).Count())
                //{
                //    CourseCompletion comp = new CourseCompletion
                //    {
                //        Userid = User.Identity.GetUserId(),
                //        DateCompleted = DateTime.Now,
                //        CourseID = lesson.CourseID                        
                //    };

                //    db.CourseCompletions.Add(comp);
                //    db.SaveChanges();
                //}
                if (db.LessonViews.Where(l => l.UserID == viewed.UserID && l.Lesson.CourseID == lesson.CourseID).Count() == db.Lessons.Where(l => l.CourseID == lesson.CourseID).Count())
                {
                    CourseCompletion comp = new CourseCompletion
                    {
                        Userid = User.Identity.GetUserId(),
                        DateCompleted = DateTime.Now,
                        CourseID = lesson.CourseID
                    };
                    db.CourseCompletions.Add(comp);
                    db.SaveChanges();

                    // 1) Build the email message body (content for the email)
                    string message = $"A course has been completed, check course completion page for updated information.<br>" +
                        $"";

                    // 2) Create the MailMessage object, and customize
                    MailMessage msg = new MailMessage(
                        //FROM - your domain email (admin@yourdomain.com)
                        "admin@thewongpage.com",
                        //TO - where the email lands (should be sent to your personal email)
                        "crwong00@outlook.com",
                        //subject
                        "Completed Course",
                        //Body
                        message
                        );

                    //allow HTML formatting
                    msg.IsBodyHtml = true;
                    msg.Priority = MailPriority.Normal;//cna change priority
                                                     //CC  or BCC other recipients


                    // 3) Create the SmtpClient that will send the email.
                    //the client will need info from the host to route the email
                    SmtpClient client = new SmtpClient("mail.TheWongpage.com");
                    client.Credentials = new NetworkCredential("admin@TheWongPage.com", "Smokes#1");


                    // 4) Attempt sending email.
                    try
                    {
                        client.Send(msg);
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ErrorMessage = $"Sorry, something went wrong. Pelase try again later or review the stacktrace <br>{ex.StackTrace}";
                        return View();
                    }

                    //if the email gets send, we will send the user to a confirmation view.
                    return RedirectToAction("TileView", "Cours");
                };

                return RedirectToAction("Index", new { id = lesson.CourseID});
            }

            return View(lesson);


        }


        //public ActionResult Test(int? id, bool? completed)
        //{
        //    if (completed == true)
        //    {
        //        CourseCompletion done = new CourseCompletion
        //        {
        //            Userid = User.Identity.GetUserId(),
        //            DateCompleted = DateTime.Now,
        //            CourseID = (int)id
        //        };
        //        db.CourseCompletions.Add(done);
        //        db.SaveChanges();
        //    }

        //        ViewBag.Course = new SelectList(db.Courses, "CourseID", "CourseName");
        //        return RedirectToAction("TileView", "Cours");
            
        //}

        [Authorize(Roles = "Admin")]
        // GET: Lessons/Create
        [HttpGet]
        public ActionResult Create(int? id)
        {
            if (id != null)
            {
                ViewBag.id = id;
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName");
            return View();
        }

        // POST: Lessons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LessonID,LessonName,CourseID,Introduction,PDFfilename,VideoURL,IsActive")] HttpPostedFileBase PDF, Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                string pdfFile = "noFile.pdf";

                if (PDF != null)
                {
                    pdfFile = PDF.FileName;

                    string ext = pdfFile.Substring(pdfFile.LastIndexOf('.'));

                    string goodExts = ".pdf";

                    if (goodExts.Contains(ext.ToLower()))
                    {
                        //save file to webserver
                        PDF.SaveAs(Server.MapPath("~/Content/images/pdf/" + pdfFile));
                    }

                }

                else
                {
                    //otherwise default back to the default image.
                    pdfFile = "noFile.pdf";
                }


                lesson.PDFfilename = pdfFile;

                db.Lessons.Add(lesson);
                db.SaveChanges();

                return RedirectToAction("Index", new { id = lesson.CourseID });
            }

            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", lesson.CourseID);
            return View(lesson);
        }

        [Authorize(Roles = "Admin")]
        // GET: Lessons/Edit/5
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = db.Lessons.Find(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", lesson.CourseID);
            return View(lesson);
        }

        // POST: Lessons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LessonID,LessonName,CourseID,Introduction,VideoURL,PDFfilename,IsActive")] HttpPostedFileBase PDF, Lesson lesson)
        {
            if (ModelState.IsValid)
            {

                if (PDF != null)
                {
                    string pdfFile = PDF.FileName;

                    string ext = pdfFile.Substring(pdfFile.LastIndexOf('.'));

                    string goodExts = ".pdf";

                    if (goodExts.Contains(ext.ToLower()))
                    {
                        //save file to webserver
                        PDF.SaveAs(Server.MapPath("~/Content/images/pdf/" + pdfFile));


                    }

                    lesson.PDFfilename = pdfFile;
                }

                db.Entry(lesson).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = new SelectList(db.Courses, "Course", "CourseName", lesson.CourseID);
            return View(lesson);
        }

        [Authorize(Roles = "Admin")]
        // GET: Lessons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = db.Lessons.Find(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            return View(lesson);
        }

        [Authorize(Roles = "Admin")]
        // POST: Lessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lesson lesson = db.Lessons.Find(id);
            db.Lessons.Remove(lesson);
            db.SaveChanges();
            return RedirectToAction("TileView", "Cours");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
