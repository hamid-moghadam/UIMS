using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models;

namespace UIMS.Web.DTO
{
    public class NotificationAccessViewModel:BaseModel
    {
        public AppRoleViewModel AppRole { get; set; }

        public NotificationTypeViewModel NotificationType { get; set; }
    }
}
