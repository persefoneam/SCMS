using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMS.DAL.Models
{
    /// <summary>
    /// Defining table for all the courses
    /// </summary>
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }

     
        public DateTime? StartDate { get; set; }
      
        public DateTime? EndDate { get; set; }

        public virtual ICollection<User> Users { get; set; } //one course have more than one user enrolled
        public virtual ICollection<Directory> Directories { get; set; }
        //This needs to be enhanced with data anotations for table constraints, example: lenght of strings.

    }
}
