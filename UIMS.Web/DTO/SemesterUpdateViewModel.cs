using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models;

namespace UIMS.Web.DTO
{
    public class SemesterUpdateViewModel:BaseModel
    {
        public string Name { get; set; }

        public bool? Enable { get; set; }
    }
}
