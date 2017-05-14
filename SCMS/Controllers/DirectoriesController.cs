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
    //TODO:This shoul not allow show anything of ended courses
    //TODO:Teachers can edit anything
    public class DirectoriesController : BaseController
    {

        public DirectoriesController()
        {

        }
        //this is constructor dependency inyection, can make the app more testiable and objects less dependent of others objects
        public DirectoriesController(ISCMSContext context)
        {
            Database = context;
        }

        // GET: Directories for an specifies course, it verifies also if user is enrolled in this course
        public ActionResult Index(int? courseId)
        {
            if (Session["IsLogged"] == null || courseId == null)
                return View("~/Views/Home/Index.cshtml");

            Session["CurrentCourse"] = courseId;
            var userId = Int32.Parse(Session["User"].ToString());
           
            if (Database.Courses.Any(c => c.ID == courseId) && Database.Courses.FirstOrDefault(c => c.ID == courseId).Users.Any(u => u.ID == userId))
                    return View(Database.Directories.Where(d => d.Course.ID == courseId).ToList());
           
                //course does not exist
                return View("~/Views/Home/Index.cshtml");
           
        }

        // GET: Directories Same as above
        public ActionResult GetCourseDirectories(int? courseId)
        {
            if (Session["IsLogged"] == null || courseId == null)
                return View("~/Views/Home/Index.cshtml");

            var userId = Int32.Parse(Session["User"].ToString());

            if (Database.Courses.Any(c => c.ID == courseId) && Database.Courses.FirstOrDefault(c => c.ID == courseId).Users.Any(u => u.ID == userId))
            {
                Session["CurrentCourse"] = courseId;
                return View(Database.Directories.Where(d => d.Course.ID == courseId).ToList());

            }

            //course does not exist
            return View("~/Views/Home/Index.cshtml");

        }
        // GET: Directories/Details/5  Gets the details od an specific directory, it should also verify 
        //if user is enroled
        public ActionResult Details(int? id)
        {
            //TODO:Verify is user is erolled on course
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Directory directory = Database.Directories.Find(id);
            if (directory == null)
            {
                return HttpNotFound();
            }
            return View(directory);
        }

        // GET: Directories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Directories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Description")] Directory directory)
        {
            if (Session["IsLogged"] == null || Session["CurrentCourse"] == null)
                return View("~/Views/Home/Index.cshtml");

            var userId = Int32.Parse(Session["User"].ToString());
            var user = Database.Users.FirstOrDefault(u => u.ID == userId);
            if (ModelState.IsValid)
            {
                directory.CreationDate = DateTime.Now;
                directory.CreatedBy = user;
                var courseId = Int32.Parse(Session["CurrentCourse"].ToString());
                directory.Course = Database.Courses.FirstOrDefault(c => c.ID == courseId);
                Database.Directories.Add(directory);
                Database.SaveChanges();
                return RedirectToAction("GetCourseDirectories", new { courseId = Session["CurrentCourse"].ToString() });
            }

            return View(directory);
        }

        // GET: Directories/Edit/5
        //just creator can edit a directory
        public ActionResult Edit(int? id)
        {
            //TODO: Make teachers edit any directory of enrolled course
            if (Session["IsLogged"] == null || Session["CurrentCourse"] == null)
                return View("~/Views/Home/Index.cshtml");

            var userId = Int32.Parse(Session["User"].ToString());
            var user = Database.Users.FirstOrDefault(u => u.ID == userId);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Directory directory = Database.Directories.Find(id);
            if (directory == null)
            {
                return HttpNotFound();
            }

            if (directory.CreatedBy.ID != user.ID)
            {
                TempData["message"] = "Only the directory creator can edit it";
                return RedirectToAction("GetCourseDirectories", new { courseId = Session["CurrentCourse"].ToString()});
            }

            return View(directory);
        }

        // POST: Directories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Description,CreationDate")] Directory directory)
        {
            if (Session["IsLogged"] == null || Session["CurrentCourse"] == null)
                return View("~/Views/Home/Index.cshtml");

            if (ModelState.IsValid)
            {
                Database.Entry(directory).State = EntityState.Modified;
                Database.SaveChanges();
                return RedirectToAction("GetCourseDirectories", new { courseId = Session["CurrentCourse"].ToString() });
            }
            return View(directory);
        }

        // GET: Directories/Delete/5
        //Just creator can delete a directory
        public ActionResult Delete(int? id)
        {
            //TODO: Make teachers delete any directory of enrolled course
            if (Session["IsLogged"] == null || Session["CurrentCourse"] == null)
                return View("~/Views/Home/Index.cshtml");

            var userId = Int32.Parse(Session["User"].ToString());
            var user = Database.Users.FirstOrDefault(u => u.ID == userId);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Directory directory = Database.Directories.Find(id);
            if (directory == null)
            {
                return HttpNotFound();
            }

            if (directory.CreatedBy.ID != user.ID)
            {
                TempData["message"] = "Only the directory creator can edit it";
                return RedirectToAction("GetCourseDirectories", new { courseId = Session["CurrentCourse"].ToString() });
            }

            return View(directory);
        }

        // POST: Directories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["IsLogged"] == null || Session["CurrentCourse"] == null)
                return View("~/Views/Home/Index.cshtml");

            Directory directory = Database.Directories.Find(id);
            Database.Directories.Remove(directory);
            Database.SaveChanges();
            return RedirectToAction("GetCourseDirectories", new { courseId = Session["CurrentCourse"].ToString() });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
