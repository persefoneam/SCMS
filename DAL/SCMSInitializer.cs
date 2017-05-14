using SCMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMS.DAL
{
    public class SCMSInitializer: System.Data.Entity.CreateDatabaseIfNotExists<SCMSContext>
    {
        /// <summary>
        /// overriding seed method, to upload data in the database when is created
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(SCMSContext context)
        {
            var course = new Course { Name = "Random Course", StartDate = DateTime.ParseExact("2017/01/01", "yyyy/MM/dd", CultureInfo.InvariantCulture), EndDate = DateTime.ParseExact("2017/12/31", "yyyy/MM/dd", CultureInfo.InvariantCulture) };

            var users = new List<User> 
            {
                //passwords should be encrypted with SHA256
                new User{ Name = "Maria", LastName = "Lopez", Email = "maria@randomu.com", Dni = 123, IsTeacher = false, Courses=new List<Course>{course}, Password = "1234"},
                new User{ Name = "Pablo", LastName = "Perez", Email = "pablo@randomu.com", Dni = 456, IsTeacher = false, Courses=new List<Course>{course}, Password = "1234"},
                new User{ Name = "Luis", LastName = "Teacher", Email = "luis@randomu.com", Dni = 789, IsTeacher = true, Courses=new List<Course>{course}, Password = "1234" }

            };

            var courses = new List<Course>();
            courses.Add(course);
            courses.ForEach(c => 
                {
                    context.Courses.Add(c);
                    context.Entry(c).State = EntityState.Added;
  
                });

            users.ForEach(u =>
                {
                    context.Users.Add(u);
                    context.Entry(u).State = EntityState.Added;
                });
            
           
            base.Seed(context);

        }

        /// <summary>
        /// calls the overrided seed method 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool Seed(SCMSContext context, bool state)
        {
            Seed(context);
            return true;
        }
    }
}
