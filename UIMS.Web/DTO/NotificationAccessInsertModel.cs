using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.DTO
{
    public class NotificationAccessInsertModel
    {
        [Required]
        public int? AppRoleId { get; set; }


        [Required]
        public int? NotificationTypeId { get; set; }
    }
}
