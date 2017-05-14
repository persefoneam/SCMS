using SCMS.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCMS.Controllers
{
    /// <summary>
    /// This abstract class is to avoid code for initialise dbcontext for every controller method. This could be a good example to use SINGLETON pattern to be sure of having just one instance of DB
    /// but this is considered as a BAD PRACTICE on MVC, because the DBContexts needs to be disposed. 
    /// </summary>
    public abstract class BaseController : Controller
    {
        // GET: Base
        protected ISCMSContext Database { get; set; }

        public BaseController()
        {
            Database = new SCMSContext();
        }

        public BaseController(ISCMSContext context)
        {
            Database = context;
        }

        protected override void Dispose(bool disposing)
        {
            if (Database is IDisposable)
            {
                ((IDisposable)Database).Dispose();
            }
            base.Dispose(disposing);
        }
    }
}