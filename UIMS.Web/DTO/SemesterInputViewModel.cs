using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models.Attributes;

namespace UIMS.Web.DTO
{
    public class SemesterInputViewModel
    {
        [Semester]
        public string Semester { get; set; }
    }
}
