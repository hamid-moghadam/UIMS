using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.DTO
{
    public class PresentationPartialViewModel
    {
        public string Code { get; set; }

        public string Start { get; set; }

        public string End { get; set; }

        public int Day { get; set; }


        public SemesterViewModel Semester { get; set; }


        public CourseFieldViewModel CourseField { get; set; }


        public ProfessorViewModel Professor { get; set; }


        public BuildingClassViewModel BuildingClass { get; set; }

        public bool Enable { get; set; }

    }
}
