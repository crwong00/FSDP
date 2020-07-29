using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FSDP.DATA.EF;

namespace FSDP.UI.MVC.Controllers
{
    public class LessonsController : Controller
    {
        private FSDPEntities db = new FSDPEntities();

        // GET: Lessons
        public ActionResult Index()
        {
            var lessons = db.Lessons.Include(l => l.Cours);
            return View(lessons.ToList());
        }

        // GET: Lessons/Details/5
        public ActionResult Details(int? id)
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

        [Authorize(Roles ="Admin")]
        // GET: Lessons/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName");
            return View();
        }

        // POST: Lessons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles ="Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LessonID,LessonName,CourseID,Introduction,PDFfilename,VideoURL,IsActive")] HttpPostedFileBase PDF, Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                string pdfFile = "noFile.pdf";

                if(PDF != null)
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

                return RedirectToAction("Index");
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
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName", lesson.CourseID);
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
            return RedirectToAction("Index");
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
