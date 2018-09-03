using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIMS.Web.Extentions;

namespace UIMS.Web.DTO
{
    public class SendRoleViewModel:SendMessageViewModel
    {
        public string Role { get; set; }

        public override bool IsValid()
        {
            return base.IsValid() && Role.DefaultIfNull() != "";
        }
    }
}
