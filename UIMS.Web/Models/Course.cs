using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UIMS.Web.Models
{
    public class Course:BaseModelTracker
    {
        [MaxLength(10)]
        public string Code { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public virtual ICollection<CourseField> Fields { get; set; }
    }
}
