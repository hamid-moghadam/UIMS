using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models;

namespace UIMS.Web.DTO
{
    public class FieldUpdateViewModel:BaseModel
    {
        [MaxLength(200)]
        public string Name { get; set; }

        public int? DegreeId { get; set; }

        public int? GroupManagerId { get; set; }

    }
}
