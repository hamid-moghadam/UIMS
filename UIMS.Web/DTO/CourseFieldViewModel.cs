using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models;

namespace UIMS.Web.DTO
{
    public class CourseFieldViewModel:BaseModel
    {
        public CourseViewModel Course { get; set; }

        public FieldViewModel Field { get; set; }

        public bool Enable { get; set; }
    }
}
