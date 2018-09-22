using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.DTO
{
    public class NotificationTypeInsertViewModel
    {
        [Required]
        public string Type { get; set; }

        [Required]
        public int Priority { get; set; }

        public bool ShowInMainPage { get; set; } = false;
    }
}
