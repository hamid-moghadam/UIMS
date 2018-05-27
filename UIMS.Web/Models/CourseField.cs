using System;
using System.Collections.Generic;
using System.Text;

namespace UIMS.Web.Models
{
    public class CourseField:BaseModelTracker
    {
        public virtual Course Course { get; set; }

        public int CourseId { get; set; }


        public virtual Field Field { get; set; }

        public int FieldId { get; set; }

        public virtual ICollection<Presentation> Presentations { get; set; }
    }
}
