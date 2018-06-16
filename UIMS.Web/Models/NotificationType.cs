using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UIMS.Web.Models
{
    public class NotificationType:BaseModelTracker
    {
        [MaxLength(50)]
        public string Type { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }

    }
}
