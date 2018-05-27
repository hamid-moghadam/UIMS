using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.DTO
{
    public class EmployeeInsertViewModel:BaseInsertViewModel
    {
        [MaxLength(80)]
        public string Post { get; set; }
    }
}
