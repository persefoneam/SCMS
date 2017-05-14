using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SCMS.DAL;
using SCMS.DAL.Models;

namespace SCMS.Controllers
{
    //An evaluated activitie acts like the directory where students will uplod their works
    public class EvaluatedActivitiesController : BaseController
    {
        public EvaluatedActivitiesController()
        {

        }
        //this is constructor dependency inyection, can make the app more testiable and objects less dependent of others objects
        public EvaluatedActivitiesController(ISCMSContext context)
        {
            Database = context;
        }

        // GET: EvaluatedActivities by course
        public ActionResult GetCourseActivities(int? courseId)
        {
            if (Session["IsLogged"] == null || courseId == null)
                return View("~/Views/Home/Index.cshtml");

            var userId = Int32.Parse(Session["User"].ToString());

            if (base.Database.Courses.Any(c => c.ID == courseId) && base.Database.Courses.FirstOrDefault(c => c.ID == courseId).Users.Any(u => u.ID == userId))
            {
                Session["CurrentCourse"] = courseId;
                return View(base.Database.EvaluatedActivities.Where(d => d.Course.ID == courseId).ToList());

            }

            //course does not exist
            return View("~/Views/Home/Index.cshtml");

        }

        // GET: EvaluatedActivities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EvaluatedActivity evaluatedActivity = Database.EvaluatedActivities.Find(id);
            if (evaluatedActivity == null)
            {
                return HttpNotFound();
            }
            return View(evaluatedActivity);
        }

        // GET: EvaluatedActivities/Create
        //Just teachers can create 
        public ActionResult Create()
        {
            //TODO: Verify if teacher is enrolled on this course
            if (Session["IsLogged"] == null || Session["CurrentCourse"] == null)
                return View("~/Views/Home/Index.cshtml");

            var userId = Int32.Parse(Session["User"].ToString());
            var user = Database.Users.FirstOrDefault(u => u.ID == userId);
            if (!user.IsTeacher)
            {
                TempData["message"] = "Only teachers can create an activity";
                return RedirectToAction("GetCourseActivities", new { courseId = Session["CurrentCourse"].ToString() });
            }
            return View();
        }

        // POST: EvaluatedActivities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Description,Name,UploadDate,EndDate")] EvaluatedActivity evaluatedActivity)
        {
            if (Session["IsLogged"] == null || Session["CurrentCourse"] == null)
                return View("~/Views/Home/Index.cshtml");

            var userId = Int32.Parse(Session["User"].ToString());
            var user = Database.Users.FirstOrDefault(u => u.ID == userId);
            if (ModelState.IsValid)
            {
                if (user.IsTeacher)
                {
                    evaluatedActivity.StartDate = DateTime.Now;
                    evaluatedActivity.CreatedBy = user;
                    var courseId = Int32.Parse(Session["CurrentCourse"].ToString());
                    evaluatedActivity.Course = Database.Courses.FirstOrDefault(c => c.ID == courseId);
                    Database.EvaluatedActivities.Add(evaluatedActivity);
                    Database.SaveChanges();
                    return RedirectToAction("GetCourseActivities", new { courseId = Session["CurrentCourse"].ToString() });
                }
            }

            return View(evaluatedActivity);
        }

        // GET: EvaluatedActivities/Edit/5
        //Just teachers can edit. 
        //TODO: Verify of teacher is enrolled 
        public ActionResult Edit(int? id)
        {
            if (Session["IsLogged"] == null || Session["CurrentCourse"] == null)
                return View("~/Views/Home/Index.cshtml");

            var userId = Int32.Parse(Session["User"].ToString());
            var user = Database.Users.FirstOrDefault(u => u.ID == userId);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EvaluatedActivity evaluatedActivity = Database.EvaluatedActivities.Find(id);
            if (evaluatedActivity == null)
            {
                return HttpNotFound();
            }
            if (!user.IsTeacher)
            {
                TempData["message"] = "Only teachers can edit an activity";
                return RedirectToAction("GetCourseActivities", new { courseId = Session["CurrentCourse"].ToString() });
            }
            return View(evaluatedActivity);
        }

        // POST: EvaluatedActivities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Description,Name,UploadDate,StartDate,EndDate")] EvaluatedActivity evaluatedActivity)
        {
            //TODO:Validate if user is enrolled to the course 
            if (Session["IsLogged"] == null || Session["CurrentCourse"] == null)
                return View("~/Views/Home/Index.cshtml");

            var userId = Int32.Parse(Session["User"].ToString());
            var user = Database.Users.FirstOrDefault(u => u.ID == userId);

            if (ModelState.IsValid && user.IsTeacher)
            {
                Database.Entry(evaluatedActivity).State = EntityState.Modified;
                Database.SaveChanges();
                return RedirectToAction("GetCourseActivities", new { courseId = Session["CurrentCourse"].ToString() });
            }
            return View(evaluatedActivity);
        }

        // GET: EvaluatedActivities/Delete/5
        public ActionResult Delete(int? id)
        {
            //TODO:Validate if user is enrolled to the course 
            if (Session["IsLogged"] == null || Session["CurrentCourse"] == null)
                return View("~/Views/Home/Index.cshtml");

            var userId = Int32.Parse(Session["User"].ToString());
            var user = Database.Users.FirstOrDefault(u => u.ID == userId);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EvaluatedActivity evaluatedActivity = Database.EvaluatedActivities.Find(id);
            if (evaluatedActivity == null)
            {
                return HttpNotFound();
            }
            if (!user.IsTeacher)
            {
                TempData["message"] = "Only teachers can delete an activity";
                return RedirectToAction("GetCourseActivities", new { courseId = Session["CurrentCourse"].ToString() });
            }
            return View(evaluatedActivity);
        }

        // POST: EvaluatedActivities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //TODO:Validate if user is enrolled to the course 
            if (Session["IsLogged"] == null || Session["CurrentCourse"] == null)
                return View("~/Views/Home/Index.cshtml");

            var userId = Int32.Parse(Session["User"].ToString());
            var user = Database.Users.FirstOrDefault(u => u.ID == userId);

            EvaluatedActivity evaluatedActivity = Database.EvaluatedActivities.Find(id);
            if (evaluatedActivity != null && user.IsTeacher)
            {
                Database.EvaluatedActivities.Remove(evaluatedActivity);
                Database.SaveChanges();
                return RedirectToAction("GetCourseActivities", new { courseId = Session["CurrentCourse"].ToString() });
            }

            return View(evaluatedActivity);
        }

        protected override void Dispose(bool disposing)
        {
            
            base.Dispose(disposing);
        }
    }
}
