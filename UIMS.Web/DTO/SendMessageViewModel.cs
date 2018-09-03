using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Extentions;

namespace UIMS.Web.DTO
{
    public class SendMessageViewModel
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string Subtitle { get; set; } = "";

        public int NotificationTypeId { get; set; }

        public virtual bool IsValid() => Title.DefaultIfNull() != "" && Content.DefaultIfNull() != "";
    }
}
