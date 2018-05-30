using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.DTO
{
    public class EmployeeUpdateViewModel:BaseUpdateViewModel
    {
        [MaxLength(80)]
        public string EmployeePost { get; set; }
    }
}
