using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.DTO
{
    public class BuildingClassInsertViewModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public int? BuildingId { get; set; }
    }
}
