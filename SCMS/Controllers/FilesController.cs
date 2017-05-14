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

/// <summary>
/// Files that teachers or students can upload, the controllers are just for directories file *(class material)
/// The evaluation files are controlled on UserEvaluatedActivity
/// </summary>
namespace SCMS.Controllers
{
    //TODO:Teachers can edit anything
    public class FilesController : BaseController
    {

        public FilesController()
        {

        }

        public FilesController(ISCMSContext context)
        {
            Database = context;
        }

        // GET: Directories
        /// <summary>
        /// Gets the files od an specific directory, it verify if user is enrolled in the current course
        /// </summary>
        /// <param name="dirId"></param>
        /// <returns></returns>
        public ActionResult GetDirectoryFiles(int? dirId)
        {
            if (Session["IsLogged"] == null || dirId == null || Session["CurrentCourse"] == null)
                return View("~/Views/Home/Index.cshtml");

            var userId = Int32.Parse(Session["User"].ToString());

            if (Database.Directories.Any(c => c.ID == dirId) && Database.Directories.FirstOrDefault(c => c.ID == dirId).Course.Users.Any(u => u.ID == userId))
            {
                Session["CurrentDirectory"] = dirId;
                return PartialView(Database.Directories.FirstOrDefault(d => d.ID == dirId).Files.ToList());

            }

            //course does not exist
            return View("~/Views/Home/Index.cshtml");

        }


        // GET: Files/Details/5
        //Detail of a file
        //TODO: verify user permissons for this file
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            File file = Database.Files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            return View(file);
        }


        // GET: DownloadFile
        //Can download a file, 
        //TODO: verify permissons
        public ActionResult DownloadFile(string path)
        {
            if (path == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            byte[] filedata = System.IO.File.ReadAllBytes(path);
            string contentType = MimeMapping.GetMimeMapping(path);

            //var cd = new System.Net.Mime.ContentDisposition
            //{
            //    FileName = filename,
            //    Inline = true,
            //};

            //Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filedata, contentType);

            
        }

        // GET: Files/Create
        public ActionResult Create()
        {
            if (Session["IsLogged"] == null || Session["CurrentCourse"] == null)
                return View("~/Views/Home/Index.cshtml");

            var userId = Int32.Parse(Session["User"].ToString());
            var user = Database.Users.FirstOrDefault(u => u.ID == userId);
            var courseId = Int32.Parse(Session["CurrentCourse"].ToString());
            var course = Database.Courses.FirstOrDefault(u => u.ID == courseId);
            var dirId = Int32.Parse(Session["CurrentDirectory"].ToString());
            var directory = Database.Directories.FirstOrDefault(u => u.ID == dirId);

            if (directory != null && directory.Course.Users.Any(u => u.ID == userId))
                return View();
            else
            {
                TempData["message"] = "You are not enrolled on this course";
                return RedirectToAction("Details", "Directories", new { id = Session["CurrentDirectory"].ToString() });
            }
        }

        // POST: Files/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,TempFile,Description")] File file)
        {
            if (Session["IsLogged"] == null || Session["CurrentCourse"] == null)
                return View("~/Views/Home/Index.cshtml");
            if (ModelState.IsValid)
            {
                var userId = Int32.Parse(Session["User"].ToString());
                var user = Database.Users.FirstOrDefault(u => u.ID == userId);
                var courseId = Int32.Parse(Session["CurrentCourse"].ToString());
                var course = Database.Courses.FirstOrDefault(u => u.ID == courseId);
                var dirId = Int32.Parse(Session["CurrentDirectory"].ToString());
                var directory = Database.Directories.FirstOrDefault(u => u.ID == dirId);

                if (directory!= null && directory.Course.Users.Any(u => u.ID == userId))
                {
                    var uploadfile = Request.Files[0];
                    if (uploadfile != null && uploadfile.ContentLength > 0)
                    {
                        var uploadDir = "~/uploads";
                        var filePath = System.IO.Path.Combine(Server.MapPath(uploadDir), uploadfile.FileName);
                        var fileUrl = System.IO.Path.Combine(uploadDir, uploadfile.FileName);
                        file.Path = filePath;
                        uploadfile.SaveAs(filePath);
                    }

                    file.UploadDate = DateTime.Now;
                    file.UploadBy = user;
                    file.Directory = directory;
                    file.TempFile = null;     
                    Database.Files.Add(file);
                    Database.SaveChanges();
                    return RedirectToAction("Details", "Directories", new { id = Session["CurrentDirectory"].ToString() });
                }
            }

            return View(file);
        }


        // GET: Files/Delete/5
        public ActionResult Delete(int? id)
        {
            //this should delete the file in folder for uploads
            if (Session["IsLogged"] == null || Session["CurrentCourse"] == null)
                return View("~/Views/Home/Index.cshtml");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userId = Int32.Parse(Session["User"].ToString());
            var user = Database.Users.FirstOrDefault(u => u.ID == userId);

            File file = Database.Files.Find(id);

            if (file == null)
            {
                return HttpNotFound();
            }

            if (file.UploadBy.ID != user.ID)
            {
                TempData["message"] = "Only the file creator can delete it";
                return RedirectToAction("Details", "Directories", new { id = Session["CurrentDirectory"].ToString() });
            }
            return View(file);
        }

        // POST: Files/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            File file = Database.Files.Find(id);
            Database.Files.Remove(file);
            Database.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
