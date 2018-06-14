using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.DTO
{
    public class GroupManagerAddFieldViewModel
    {
        [Required]
        public int? FieldId { get; set; }

        [Required]
        public int? GroupManagerId { get; set; }

    }
}
