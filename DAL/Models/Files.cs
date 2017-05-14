using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web;

namespace SCMS.DAL.Models
{
    /// <summary>
    /// files uploaded, it can be on a directory or an user evaluated activity
    /// </summary>
    public class File
    {
        public int ID { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public DateTime? UploadDate { get; set; }
        [NotMapped]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase TempFile { get; set; } //just for MVC models, I'll not save the file in the database
        public virtual User UploadBy { get; set; }
        public virtual Directory Directory { get; set; }

        //This needs to be enhanced with data anotations for table constraints, example: lenght of strings.

    }
}
