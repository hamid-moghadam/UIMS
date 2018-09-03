using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Extentions;

namespace UIMS.Web.DTO
{
    public class SendMessagePresentationInsertViewModel:SendMessageViewModel
    {
        public int Id { get; set; }

        public override bool IsValid()
        {
            return base.IsValid() && Id > 0;
        }
    }
}
