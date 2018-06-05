using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models;

namespace UIMS.Web.DTO
{
    public class CourseViewModel:BaseModel
    {
        public string Code { get; set; }


        public string Name { get; set; }

        //public ICollection<FieldViewModel> Fields  { get; set; }

    }
}
