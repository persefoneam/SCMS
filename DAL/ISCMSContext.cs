using SCMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMS.DAL
{
    /// <summary>
    /// to make the application testeable and scalable
    /// </summary>
    public interface ISCMSContext
    {
         IDbSet<User> Users { get; set; }
         IDbSet<Course> Courses { get; set; }
         IDbSet<Directory> Directories { get; set; }
         IDbSet<File> Files { get; set; }
         IDbSet<EvaluatedActivity> EvaluatedActivities { get; set; }
         IDbSet<UserEvaluationActivity> UserEvaluationActivities { get; set; }
         int SaveChanges();
         DbEntityEntry Entry(object entity);
         DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

    }
}
