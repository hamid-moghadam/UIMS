using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models;

namespace UIMS.Web.DTO
{
    public class PresentationUpdateViewModel:BaseModel
    {
        [MaxLength(10)]
        public string Code { get; set; }

        [MaxLength(5)]
        public string Start { get; set; }

        [MaxLength(5)]
        public string End { get; set; }

        //[Required]
        public DayOfWeek? Day { get; set; }

        public bool? Enable { get; set; }


        //public int? CourseFieldId { get; set; }

        //public int? ProfessorId { get; set; }

        public int? BuildingClassId { get; set; }
    }
}
