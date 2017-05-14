using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMS.DAL.Models
{
    /// <summary>
    /// for grouping course material
    /// </summary>
    public class Directory
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreationDate { get; set; }
        
        public virtual User CreatedBy { get; set; }
        public virtual Course Course { get; set; }
        public virtual ICollection<File> Files { get; set; }

    }
}
