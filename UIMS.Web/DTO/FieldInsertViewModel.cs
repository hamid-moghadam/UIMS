using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.DTO
{
    public class FieldInsertViewModel
    {
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        public int? DegreeId { get; set; }


        public int? GroupManagerId { get; set; }

    }
}
