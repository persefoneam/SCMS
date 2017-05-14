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
    public class CoursesController : BaseController
    {

        public CoursesController()
        {

        }
        //this is constructor dependency inyection, can make the app more testiable and objects less dependent of others objects
        public CoursesController(ISCMSContext context)
        {
            Database = context;
        }
        //TODO:This shoul not show ended courses
        // GET: Courses
        public ActionResult Index()
        {
            if (Session["IsLogged"] == null)
                return View("~/Views/Home/Index.cshtml");

            var userId = Int32.Parse(Session["User"].ToString());
            var user = Database.Users.FirstOrDefault(u => u.ID == userId);
            if (user == null)
                return View("~/Views/Home/Index.cshtml");

            var enrolledCourses = user.Courses;
            
            return View(enrolledCourses);
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = Database.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = id;
            return View(course);
        }

      

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
