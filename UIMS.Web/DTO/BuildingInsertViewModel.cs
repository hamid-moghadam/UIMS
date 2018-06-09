using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.DTO
{
    public class BuildingInsertViewModel
    {
        [MaxLength(100)]
        public string Name { get; set; }


        //public int? BuildingManagerId { get; set; }
    }
}
