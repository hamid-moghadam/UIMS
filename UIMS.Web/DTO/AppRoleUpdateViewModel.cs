using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models;

namespace UIMS.Web.DTO
{
    public class AppRoleUpdateViewModel:BaseModel
    {
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(30)]
        public string PersianName { get; set; }
    }
}
