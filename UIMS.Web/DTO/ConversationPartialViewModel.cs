using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.DTO
{
    public class ConversationPartialViewModel
    {
        public UserViewModel FirstUser { get; set; }

        public UserViewModel SecondUser { get; set; }

        public bool Enable { get; set; }
    }
}
