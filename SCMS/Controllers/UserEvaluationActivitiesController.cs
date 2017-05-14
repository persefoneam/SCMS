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
    public class UserEvaluationActivitiesController : BaseController
    {

        public UserEvaluationActivitiesController()
        {

        }

        public UserEvaluationActivitiesController(ISCMSContext context)
        {
            Database = context;
        }

        // GET: UserEvaluationActivities
        public ActionResult Index()
        {
            return View(Database.UserEvaluationActivities.ToList());
        }

        // GET: Directories
        public ActionResult GetUserEvaluationActivities(int? evalId)
        {
            if (Session["IsLogged"] == null || evalId == null || Session["CurrentCourse"] == null)
                return View("~/Views/Home/Index.cshtml");

            var userId = Int32.Parse(Session["User"].ToString());
            var user = Database.Users.FirstOrDefault(u => u.ID == userId);

            if (base.Database.EvaluatedActivities.Any(c => c.ID == evalId) && base.Database.EvaluatedActivities.FirstOrDefault(c => c.ID == evalId).Course.Users.Any(u => u.ID == userId))
            {
                Session["CurrentEvaluation"] = evalId;
                if (!user.IsTeacher)
                     return PartialView(base.Database.EvaluatedActivities.FirstOrDefault(d => d.ID == evalId).UsersActivities.Where(ua=>ua.EvaluatedUSer.ID == userId).ToList());

                return PartialView(base.Database.EvaluatedActivities.FirstOrDefault(d => d.ID == evalId).UsersActivities.ToList());
            }

            //course does not exist
            return View("~/Views/Home/Index.cshtml");

        }
        // GET: DownloadFile
        public ActionResult DownloadFile(string path)
        {
            if (path == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            byte[] filedata = System.IO.File.ReadAllBytes(path);
            string contentType = MimeMapping.GetMimeMapping(path);

            return File(filedata, contentType);
        }

        // GET: UserEvaluationActivities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserEvaluationActivity userEvaluationActivity = Database.UserEvaluationActivities.Find(id);
            if (userEvaluationActivity == null)
            {
                return HttpNotFound();
            }
            return View(userEvaluationActivity);
        }

        // GET: UserEvaluationActivities/Create
        public ActionResult Create()
        {
            if (Session["IsLogged"] == null || Session["CurrentCourse"] == null || Session["CurrentEvaluation"] == null)
                return View("~/Views/Home/Index.cshtml");

            var userId = Int32.Parse(Session["User"].ToString());
            var user = Database.Users.FirstOrDefault(u => u.ID == userId);
            var evalId = Int32.Parse(Session["CurrentEvaluation"].ToString());
            var activity = Database.EvaluatedActivities.FirstOrDefault(u => u.ID == evalId);
            if (user.IsTeacher)
            {
                TempData["message"] = "A teacher can't upload evaluation activities";
                return RedirectToAction("Details", "EvaluatedActivities", new { id = Session["CurrentEvaluation"].ToString() });
            }
            if (Database.EvaluatedActivities.FirstOrDefault(ev=> ev.ID == evalId).UsersActivities.Any(u=> u.EvaluatedUSer == user))
            {
                TempData["message"] = "You have already upload your activity";
                return RedirectToAction("Details", "EvaluatedActivities", new { id = Session["CurrentEvaluation"].ToString() });
            }


            return View();
        }

        // POST: UserEvaluationActivities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserEvaluationActivity userEvaluationActivity)
        {
            
            if (ModelState.IsValid)
            {
                var userId = Int32.Parse(Session["User"].ToString());
                var user = Database.Users.FirstOrDefault(u => u.ID == userId);
                var courseId = Int32.Parse(Session["CurrentCourse"].ToString());
                var course = Database.Courses.FirstOrDefault(u => u.ID == courseId);
                var evalId = Int32.Parse(Session["CurrentEvaluation"].ToString());
                var evaluation = Database.EvaluatedActivities.FirstOrDefault(u => u.ID == evalId);
                
                    var uploadfile = Request.Files[0];
                    if (uploadfile != null && uploadfile.ContentLength > 0)
                    {
                        File file = new File();
                        file.UploadBy = user;
                        file.UploadDate = DateTime.Now;
                        var uploadDir = "~/uploads";
                        var filePath = System.IO.Path.Combine(Server.MapPath(uploadDir), uploadfile.FileName);
                        var fileUrl = System.IO.Path.Combine(uploadDir, uploadfile.FileName);
                        file.Path = filePath;
                        userEvaluationActivity.File = file;
                        uploadfile.SaveAs(filePath);
                    }
                    userEvaluationActivity.EvaluatedUSer = user;
                    userEvaluationActivity.CreationDate = DateTime.Now;
                    userEvaluationActivity.EvaluatedActivity = evaluation;
                    Database.UserEvaluationActivities.Add(userEvaluationActivity);
                    Database.SaveChanges();
                    return RedirectToAction("Details", "EvaluatedActivities", new { id = Session["CurrentEvaluation"].ToString() });
                
            }

            return View(userEvaluationActivity);
        }

        // GET: UserEvaluationActivities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["IsLogged"] == null || Session["CurrentCourse"] == null || Session["CurrentEvaluation"] == null)
                return View("~/Views/Home/Index.cshtml");

            var userId = Int32.Parse(Session["User"].ToString());
            var user = Database.Users.FirstOrDefault(u => u.ID == userId);
            var evalId = Int32.Parse(Session["CurrentEvaluation"].ToString());
           
            if (!user.IsTeacher)
            {
                TempData["message"] = "Just teachers can evaluate activities";
                return RedirectToAction("Details", "EvaluatedActivities", new { id = Session["CurrentEvaluation"].ToString() });
            }
            if (!Database.EvaluatedActivities.FirstOrDefault(ev => ev.ID == evalId).Course.Users.Any(u => u.ID == user.ID))
            {
                TempData["message"] = "You are not a theacher of this cuourse";
                return RedirectToAction("Details", "EvaluatedActivities", new { id = Session["CurrentEvaluation"].ToString() });
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserEvaluationActivity userEvaluationActivity = Database.UserEvaluationActivities.Find(id);
            if (userEvaluationActivity == null)
            {
                return HttpNotFound();
            }
            return View(userEvaluationActivity);
        }

        // POST: UserEvaluationActivities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CreationDate,Score")] UserEvaluationActivity userEvaluationActivity)
        {
            if (ModelState.IsValid)
            {
                var userId = Int32.Parse(Session["User"].ToString());
                var user = Database.Users.FirstOrDefault(u => u.ID == userId);
                var evalId = Int32.Parse(Session["CurrentEvaluation"].ToString());

                if (user.IsTeacher && Database.EvaluatedActivities.FirstOrDefault(ev => ev.ID == evalId).Course.Users.Any(u => u.ID == user.ID))
                Database.Entry(userEvaluationActivity).State = EntityState.Modified;
                Database.SaveChanges();
                return RedirectToAction("Details", "EvaluatedActivities", new { id = Session["CurrentEvaluation"].ToString() });
            }
            return View(userEvaluationActivity);
        }

        // GET: UserEvaluationActivities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserEvaluationActivity userEvaluationActivity = Database.UserEvaluationActivities.Find(id);
            if (userEvaluationActivity == null)
            {
                return HttpNotFound();
            }
            return View(userEvaluationActivity);
        }

        // POST: UserEvaluationActivities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserEvaluationActivity userEvaluationActivity = Database.UserEvaluationActivities.Find(id);
            Database.UserEvaluationActivities.Remove(userEvaluationActivity);
            Database.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            
            base.Dispose(disposing);
        }
    }
}
