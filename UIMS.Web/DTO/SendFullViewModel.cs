using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.DTO
{
    public class SendFullViewModel:SendMessageViewModel
    {
        public string[] Ids { get; set; }

        public int NotificationTypeId { get; set; }


        public override bool IsValid()
        {
            return base.IsValid() && NotificationTypeId > 0 && Ids != null && Ids.Length > 0;
        }
    }
}
