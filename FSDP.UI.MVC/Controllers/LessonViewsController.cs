﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FSDP.DATA.EF;
using Microsoft.AspNet.Identity;
using PagedList;
using PagedList.Mvc;

namespace FSDP.UI.MVC.Controllers
{
    public class LessonViewsController : Controller
    {
        private FSDPEntities db = new FSDPEntities();

        // GET: LessonViews
        public ActionResult Index(int? id)
        {
            if(User.IsInRole("Employee"))
            {
                

                string student = User.Identity.GetUserId();
                ViewBag.id = id;

                var lessonViews = db.LessonViews.Include(l => l.Lesson).Include(l => l.UserDetail).Where(l => l.UserID == student).OrderBy(l => l.LessonID);


                return View(lessonViews.ToList());
            }
            else if(User.IsInRole("Admin") || User.IsInRole("Manager"))
            {

                var completed = db.LessonViews.OrderBy(l => l.UserDetail.UserID).ToList();

                return View(completed);
            }
            else
            {
                return RedirectToAction("TileView", "Cours");
            }
        }

        // GET: LessonViews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LessonView lessonView = db.LessonViews.Find(id);
            if (lessonView == null)
            {
                return HttpNotFound();
            }
            return View(lessonView);
        }

        [Authorize(Roles = "Admin")]
        // GET: LessonViews/Create
        public ActionResult Create()
        {
            ViewBag.LessonID = new SelectList(db.Lessons, "LessonID", "LessonName");
            ViewBag.UserID = new SelectList(db.UserDetails, "UserID", "FirstName");
            return View();
        }

        // POST: LessonViews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LessonViewID,UserID,LessonID,DateViewed")] LessonView lessonView)
        {
            if (ModelState.IsValid)
            {
                db.LessonViews.Add(lessonView);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LessonID = new SelectList(db.Lessons, "LessonID", "LessonName", lessonView.LessonID);
            ViewBag.UserID = new SelectList(db.UserDetails, "UserID", "FirstName", lessonView.UserID);
            return View(lessonView);
        }

        [Authorize(Roles = "Admin")]
        // GET: LessonViews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LessonView lessonView = db.LessonViews.Find(id);
            if (lessonView == null)
            {
                return HttpNotFound();
            }
            ViewBag.LessonID = new SelectList(db.Lessons, "LessonID", "LessonName", lessonView.LessonID);
            ViewBag.UserID = new SelectList(db.UserDetails, "UserID", "FirstName", lessonView.UserID);
            return View(lessonView);
        }

        // POST: LessonViews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LessonViewID,UserID,LessonID,DateViewed")] LessonView lessonView)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lessonView).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LessonID = new SelectList(db.Lessons, "LessonID", "LessonName", lessonView.LessonID);
            ViewBag.UserID = new SelectList(db.UserDetails, "UserID", "FirstName", lessonView.UserID);
            return View(lessonView);
        }

        // GET: LessonViews/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LessonView lessonView = db.LessonViews.Find(id);
            if (lessonView == null)
            {
                return HttpNotFound();
            }
            return View(lessonView);
        }

        // POST: LessonViews/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LessonView lessonView = db.LessonViews.Find(id);
            db.LessonViews.Remove(lessonView);
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
