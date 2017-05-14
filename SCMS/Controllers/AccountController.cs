using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SCMS.Models;
using System.Web.Security;
using SCMS.DAL;

namespace SCMS.Controllers
{
    /// <summary>
    /// just for managing login, we can use this controller to change password, sen verification email, etc
    /// </summary>
    [Authorize]
    public class AccountController : BaseController
    {

        public AccountController()
        {

        }
        //this is constructor dependency inyection, can make the app more testiable and objects less dependent of others objects
        public AccountController(ISCMSContext context)
        {
            Database = context;
        }


        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl) //This could be done with signin manager default MVC, but i preffer manually do it for this test
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //The password in the BD is not encrypted, it shoul be with some encrypting algorith like for eg Sha256, and in the comparation we should encrypt users entry and then 
            var user = Database.Users.FirstOrDefault(u => u.Dni.ToString() == model.User && u.Password == model.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }
            else 
            {

                Session.Add("User", user.ID);
                Session.Add("IsTeacher", user.IsTeacher);
                Session.Add("Email", user.Email);
                Session.Add("IsLogged", true);
                Session.Add("UserName", user.Name);

                return RedirectToAction("Index", "Courses", Database.Courses);

            }

        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult LogOut(LoginViewModel model) //This could be done with signin manager default MVC and cookies, but i preffer manually do it wih sessions for this test
        {
            Session.Remove("IsLogged");
            Session.Remove("UserName");
            Session.Remove("User");
            Session.Remove("IsTeacher");
            Session.Remove("Email");

            return View("~/Views/Home/Index.cshtml", model);

        }



        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

      
    }
}