using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FSDP.DATA.EF;
using Microsoft.AspNet.Identity;

namespace FSDP.UI.MVC.Controllers
{
    public class CoursController : Controller
    {
        private FSDPEntities db = new FSDPEntities();

        // GET: Cours
        public ActionResult Index()
        {
            return View(db.Courses.ToList());
        }

        //Get: Cours tiles view
        public ActionResult TileView()
        {
            return View(db.Courses.ToList());
        }

        // GET: Cours/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cours cours = db.Courses.Find(id);
            if (cours == null)
            {
                return HttpNotFound();
            }
            //return View(lessons.ToList());

            return View(cours);
        }

        [Authorize(Roles ="Admin")]
        // GET: Cours/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cours/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles ="Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseID,CourseName,CourseDescription,isActive,Photo")] HttpPostedFileBase IMG, Cours cours)
        {
            if (ModelState.IsValid)
            {
                string pic = "noFile.pdf";

                if(IMG != null)
                {
                    pic = IMG.FileName;

                    string ext = pic.Substring(pic.LastIndexOf('.'));

                    string[] goodExts = {".png",".jpg",".jpeg" };

                    if (goodExts.Contains(ext.ToLower()))
                    {
                        //save file to webserver
                        IMG.SaveAs(Server.MapPath("~/Content/images/fulls/" + pic));
                    }

                }

                else
                {
                    //otherwise default back to the default image.
                    pic = "noimg.png";
                }


                cours.Photo = pic;

                db.Courses.Add(cours);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cours);
        }

        [Authorize(Roles ="Admin")]
        // GET: Cours/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cours cours = db.Courses.Find(id);
            if (cours == null)
            {
                return HttpNotFound();
            }
            return View(cours);
        }

        // POST: Cours/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles ="Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseID,CourseName,CourseDescription,isActive,Photo")] HttpPostedFileBase IMG, Cours cours)
        {
            if (ModelState.IsValid)
            {

                if(IMG != null)
                {
                    string pic = IMG.FileName;

                    string ext = pic.Substring(pic.LastIndexOf('.'));

                    string[] goodExts = {".jpg", ".jpeg", ".png" };

                    if (goodExts.Contains(ext.ToLower()))
                    {
                        //save file to webserver
                        IMG.SaveAs(Server.MapPath("~/Content/images/fulls/" + pic));


                    }

                    cours.Photo = pic;
                }

                db.Entry(cours).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cours);
        }

        [Authorize(Roles ="Admin")]
        // GET: Cours/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cours cours = db.Courses.Find(id);
            if (cours == null)
            {
                return HttpNotFound();
            }
            return View(cours);
        }

        [Authorize(Roles ="Admin")]
        // POST: Cours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cours cours = db.Courses.Find(id);
            db.Courses.Remove(cours);
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
