using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models.Attributes;

namespace UIMS.Web.DTO
{
    public class StudentUpdateViewModel:BaseUpdateViewModel
    {
        [Number]
        [StringLength(14, MinimumLength = 14)]
        public string StudentCode { get; set; }
    }
}
