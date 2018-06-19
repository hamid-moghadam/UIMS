using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.DTO
{
    public class PresentationProfessorViewModel
    {
        public string Code { get; set; }

        public string Start { get; set; }

        public string End { get; set; }

        public int Day { get; set; }


        public CourseFieldViewModel CourseField { get; set; }

        public BuildingClassViewModel BuildingClass { get; set; }

        public bool Enable { get; set; }
    }
}
