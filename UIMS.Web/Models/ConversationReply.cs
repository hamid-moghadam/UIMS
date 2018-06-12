using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.Models
{
    public class ConversationReply:BaseModelTracker
    {
        [StringLength(1000)]
        public string Reply { get; set; }

        public virtual AppUser Replier { get; set; }
        public int ReplierId { get; set; }

        public virtual Conversation Conversation { get; set; }
        public int ConversationId { get; set; }

        public virtual Semester Semester { get; set; }
        public int SemesterId { get; set; }

        public bool HasSeen { get; set; }


    }
}
