using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.DTO
{
    public class ChatReplyViewModel
    {
        public string Reply { get; set; }

        public UserViewModel Replier { get; set; }

        //public ConversationViewModel Conversation { get; set; }

        public SemesterViewModel Semester { get; set; }

        public bool HasSeen { get; set; }
    }
}
