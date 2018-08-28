using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.DTO
{
    public class AppRoleInsertViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string PersianName { get; set; }
    }
}
