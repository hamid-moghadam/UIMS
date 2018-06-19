using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models;
using UIMS.Web.Models.Attributes;

namespace UIMS.Web.DTO
{
    public class SemesterUpdateViewModel:BaseModel
    {
        [Semester]
        public string Name { get; set; }

        //public bool? Enable { get; set; }
    }
}
