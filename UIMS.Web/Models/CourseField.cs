using System;
using System.Collections.Generic;
using System.Text;
using UIMS.Web.Models.Interfaces;

namespace UIMS.Web.Models
{
    public class CourseField:BaseModelTracker,IEnable
    {
        public virtual Course Course { get; set; }

        public int CourseId { get; set; }


        public virtual Field Field { get; set; }

        public int FieldId { get; set; }

        public virtual ICollection<Presentation> Presentations { get; set; }
        public bool Enable { get; set; }
    }
}
