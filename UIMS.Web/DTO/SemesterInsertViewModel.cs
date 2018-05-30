using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.DTO
{
    public class SemesterInsertViewModel
    {
        [StringLength(6, MinimumLength = 6)]
        public string Name { get; set; }
    }
}
