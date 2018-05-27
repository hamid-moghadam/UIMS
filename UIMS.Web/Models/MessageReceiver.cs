using System;
using System.Collections.Generic;
using System.Text;

namespace UIMS.Web.Models
{
    public class MessageReceiver:BaseModelTracker
    {
        public bool HasSeen { get; set; }


        public virtual AppUser User { get; set; }

        public int UserId { get; set; }


        public virtual Message Message { get; set; }

        public int MessageId { get; set; }
    }
}
