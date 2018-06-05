using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models;
using UIMS.Web.Models.Attributes;

namespace UIMS.Web.DTO
{
    public class CourseUpdateViewModel:BaseModel
    {

        [Number]
        [MaxLength(10)]
        public string Code { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

    }
}
