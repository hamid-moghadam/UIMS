using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Models;

namespace UIMS.Web.DTO
{
    public class ChatViewModel:BaseModel
    {
        public UserViewModel FirstUser { get; set; }

        public UserViewModel SecondUser { get; set; }

        public bool Enable { get; set; }

        //public ICollection<ChatReplyViewModel> ConversationReplies { get; set; }
    }
}
