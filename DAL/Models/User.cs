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
    /// Defining table for all the users
    /// </summary>
    public  class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public int Dni { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        [Required]
        public bool IsTeacher { get; set; }
        [Required]
        public string Email { get; set; }
        public string Password { get; set; } 

        public virtual ICollection<Course> Courses { get; set; } //A user could be enrolled in more than one course

        //This needs to be enhanced with data anotations for table constraints, example: lenght of strings, error messages, etc etc

        //this should use roles and actions table instead of isTeacher to determinate user actions.

    }
}
