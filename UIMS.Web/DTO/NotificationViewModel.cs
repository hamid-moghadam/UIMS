using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models;

namespace UIMS.Web.DTO
{
    public class NotificationViewModel:BaseModel
    {
        public UserPartialViewModel Sender { get; set; }

        public int SenderId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public bool Enable { get; set; }


        public int SemesterId { get; set; }

        public SemesterViewModel Semester { get; set; }

        public ICollection<NotificationReceiverPartialViewModel> Receivers { get; set; }

    }
}
