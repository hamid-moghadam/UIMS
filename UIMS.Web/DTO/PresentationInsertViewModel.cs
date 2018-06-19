using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models.Attributes;

namespace UIMS.Web.DTO
{
    public class PresentationInsertViewModel
    {
        [MaxLength(10)]
        [MinLength(3)]
        public string Code { get; set; }

        [MaxLength(5)]
        public string Start { get; set; }

        [MaxLength(5)]
        public string End { get; set; }

        [EnumDataType(typeof(DayOfWeek))]
        [Required]
        public DayOfWeek? Day { get; set; }


        [Required]
        public int? CourseFieldId { get; set; }

        [Required]
        public int? ProfessorId { get; set; }

        [Required]
        public int? BuildingClassId { get; set; }

    }
}
