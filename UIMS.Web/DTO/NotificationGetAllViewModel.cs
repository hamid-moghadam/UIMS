using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.DTO
{
    public class NotificationGetAllViewModel:Pagination
    {
        public string Semester { get; set; }

        [Required]
        public int? NotificationTypeId { get; set; }
    }
}
