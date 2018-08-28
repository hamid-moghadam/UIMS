using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models.Interfaces;

namespace UIMS.Web.Models
{
    public class NotificationAccess : BaseModelTracker
    {
        public NotificationType NotificationType { get; set; }

        public int NotificationTypeId { get; set; }


        public AppRole AppRole { get; set; }

        public int AppRoleId { get; set; }

    }
}
