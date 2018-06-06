using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.DTO
{
    public class CourseFieldInsertViewModel
    {
        [Required]
        public int? CourseId { get; set; }

        [Required]
        public int? FieldId { get; set; }
    }
}
