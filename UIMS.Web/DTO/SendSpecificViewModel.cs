using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.DTO
{
    public class SendSpecificViewModel:SendMessageViewModel
    {
        public string[] Ids { get; set; }


        public override bool IsValid()
        {
            return base.IsValid() && Ids != null && Ids.Length > 0;
        }
    }
}
