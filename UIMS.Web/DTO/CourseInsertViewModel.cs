using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models.Attributes;

namespace UIMS.Web.DTO
{
    public class CourseInsertViewModel
    {
        [Number]
        [Required]
        [MaxLength(10)]
        [MinLength(3)]
        public string Code { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        //public List<int> FieldIds { get; set; }

    }
}
