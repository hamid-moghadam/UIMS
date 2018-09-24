using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UIMS.Web.Models.Interfaces;

namespace UIMS.Web.Models
{
    public class Presentation : BaseModelTracker,IEnable
    {

        [MaxLength(10)]
        public string Code { get; set; }

        [MaxLength(5)]
        public string Start { get; set; }

        [MaxLength(5)]
        public string End { get; set; }

        public DayOfWeek? Day { get; set; }


        public int SemesterId { get; set; }

        public virtual Semester Semester { get; set; }


        public int CourseFieldId { get; set; }

        public virtual CourseField CourseField { get; set; }


        public int? ProfessorId { get; set; }

        public virtual Professor Professor { get; set; }


        public int? BuildingClassId { get; set; }

        public virtual BuildingClass BuildingClass { get; set; }


        public virtual ICollection<StudentPresentation> Students { get; set; }
        public bool Enable { get; set ; }
    }
}
