using System;
using System.Collections.Generic;
using System.Text;

namespace UIMS.Web.Models
{
    public class NotificationReceiver:BaseModelTracker
    {
        public bool HasSeen { get; set; }


        public virtual AppUser User { get; set; }

        public int UserId { get; set; }


        public virtual Notification Notification { get; set; }

        public int NotificationId { get; set; }
    }
}
