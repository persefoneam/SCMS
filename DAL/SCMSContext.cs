using SCMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMS.DAL
{
    /// <summary>
    /// Defining the data context for the database
    /// </summary>
    public class SCMSContext: DbContext, ISCMSContext
    {
        public SCMSContext() : base(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SCMSTest;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False")
        {
            //InitializeDatabase();
            
        }

        public void InitializeDatabase()
        {
            if (!Database.Exists())
            {
                Database.Initialize(true);
                new SCMSInitializer().Seed(this, true);
  
            }      
        }

        public IDbSet<User> Users { get; set; }
        public IDbSet<Course> Courses { get; set; }
        public IDbSet<Directory> Directories { get; set; }
        public IDbSet<File> Files { get; set; }
        public IDbSet<EvaluatedActivity> EvaluatedActivities { get; set; }
        public IDbSet<UserEvaluationActivity> UserEvaluationActivities { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //removing plurals on the database, plurals in table names are anoying
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

       


    }
}
