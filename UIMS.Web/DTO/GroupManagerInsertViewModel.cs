using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.DTO
{
    public class GroupManagerInsertViewModel:BaseInsertViewModel
    {
        [Required]
        public int? GroupManagerFieldId { get; set; }
    }
}
