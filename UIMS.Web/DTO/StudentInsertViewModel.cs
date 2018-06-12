using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models.Attributes;

namespace UIMS.Web.DTO
{
    public class StudentInsertViewModel:BaseInsertViewModel
    {
        [Number]
        [StringLength(14,MinimumLength =5)]
        public string StudentCode { get; set; }

    }
}
