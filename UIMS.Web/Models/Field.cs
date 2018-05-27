using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UIMS.Web.Models
{
    public class Field:BaseModelTracker
    {
        [MaxLength(200)]
        public string Name { get; set; }

        public int DegreeId { get; set; }

        public virtual Degree Degree { get; set; }


        public int GroupManagerId { get; set; }

        public virtual GroupManager GroupManager { get; set; }


        public virtual ICollection<CourseField> Courses { get; set; }
    }
}
