using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCMS.DAL.Models
{
    /// <summary>
    /// users interaction with evaluated activities
    /// </summary>
    public class UserEvaluationActivity
    {
        public int ID { get; set; }
        public DateTime? CreationDate { get; set; }
        public int Score { get; set; }

        public virtual User EvaluatedUSer { get; set; }
        public virtual EvaluatedActivity EvaluatedActivity { get; set; }
        public virtual File File { get; set; }

        //This needs to be enhanced with data anotations for table constraints, example: lenght of strings.
    }
}
