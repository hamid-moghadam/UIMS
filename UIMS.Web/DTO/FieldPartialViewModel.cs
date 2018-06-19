using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models;

namespace UIMS.Web.DTO
{
    public class FieldPartialViewModel:BaseModel
    {
        public string Name { get; set; }

        public DegreeViewModel Degree { get; set; }
    }
}
