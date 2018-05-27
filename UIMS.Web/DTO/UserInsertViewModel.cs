using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models;

namespace UIMS.Web.DTO
{
    public class UserInsertViewModel:BaseModel
    {
        [MaxLength(10)]
        public string MelliCode { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(80)]
        public string Family { get; set; }

        [MinLength(6)]
        public string Password { get; set; }

        public string Type { get; set; } = "admin";

    }
}
