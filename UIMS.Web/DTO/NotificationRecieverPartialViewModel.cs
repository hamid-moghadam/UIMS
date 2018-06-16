using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.DTO
{
    public class NotificationReceiverPartialViewModel
    {
        public bool HasSeen { get; set; }


        public UserPartialViewModel User { get; set; }

        public int UserId { get; set; }

    }
}
