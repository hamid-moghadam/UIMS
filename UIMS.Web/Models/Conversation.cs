using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models.Interfaces;

namespace UIMS.Web.Models
{
    public class Conversation:BaseModelTracker, IEnable
    {

        public virtual AppUser FirstUser { get; set; }
        public int FirstUserId { get; set; }

        public virtual AppUser SecondUser { get; set; }
        public int SecondUserId { get; set; }

        public ICollection<ConversationReply> ConversationReplies { get; set; }

        public bool Enable { get; set; }
    }
}
