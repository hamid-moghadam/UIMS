using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UIMS.Web.Models.Interfaces;

namespace UIMS.Web.Models
{
    public class Notification:BaseModelTracker,IEnable
    {
        public virtual AppUser Sender { get; set; }

        public int SenderId { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Content { get; set; }

        public bool Enable { get; set; }


        public int SemesterId { get; set; }

        public virtual Semester Semester { get; set; }

        public int NotificationTypeId { get; set; }
        public virtual NotificationType NotificationType { get; set; }


        public virtual ICollection<NotificationReceiver> Receivers { get; set; }



    }
}
