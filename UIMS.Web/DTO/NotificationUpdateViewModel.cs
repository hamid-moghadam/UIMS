using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models;

namespace UIMS.Web.DTO
{
    public class NotificationUpdateViewModel:BaseModel
    {
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(200)]
        public string Subtitle { get; set; }

        [MaxLength(1000)]
        public string Content { get; set; }

        public bool? Enable { get; set; }
    }
}
