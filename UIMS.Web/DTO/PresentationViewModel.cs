﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models;

namespace UIMS.Web.DTO
{
    public class PresentationViewModel:BaseModel
    {
        public string Code { get; set; }

        public string Start { get; set; }

        public string End { get; set; }

        public string Day { get; set; }


        public SemesterViewModel Semester { get; set; }


        public CourseFieldViewModel CourseField { get; set; }


        public ProfessorViewModel Professor { get; set; }


        public BuildingClassViewModel BuildingClass { get; set; }


        //public virtual ICollection<StudentPresentation> Students { get; set; }
        public bool Enable { get; set; }
    }
}
