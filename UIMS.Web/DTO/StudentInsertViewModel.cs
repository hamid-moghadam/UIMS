using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.DTO
{
    public class StudentInsertViewModel:BaseInsertViewModel
    {
        [StringLength(14,MinimumLength =14)]
        public string StudentCode { get; set; }

    }
}
