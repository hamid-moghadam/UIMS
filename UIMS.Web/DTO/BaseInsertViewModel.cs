using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models;
using UIMS.Web.Models.Attributes;

namespace UIMS.Web.DTO
{
    public class BaseInsertViewModel:BaseModel
    {
        [Required]
        [Number]
        [StringLength(10,MinimumLength =10)]
        public string MelliCode { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(80)]
        public string Family { get; set; }

    }
}
