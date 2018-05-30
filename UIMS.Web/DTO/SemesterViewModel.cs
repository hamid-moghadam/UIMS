using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models;

namespace UIMS.Web.DTO
{
    public class SemesterViewModel:BaseModel
    {
        [StringLength(6,MinimumLength =6)]
        public string Name { get; set; }

        public bool Enable { get; set; }
    }
}
