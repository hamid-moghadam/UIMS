using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.DTO
{
    public class BuildingManagerInsertViewModel:BaseInsertViewModel
    {
        [Required]
        public int? BuildingManagerBuildingId { get; set; }

    }
}
