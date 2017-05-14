using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCMS.DAL.Models
{
    /// <summary>
    /// Activities with score, the students must upload one file for each activity
    /// </summary>
    public  class EvaluatedActivity
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime? UploadDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public virtual User CreatedBy { get; set; }
        public virtual Course Course { get; set; }
        public virtual ICollection<UserEvaluationActivity> UsersActivities { get; set; }
        //This needs to be enhanced with data anotations for table constraints, example: lenght of strings.


    }
}
