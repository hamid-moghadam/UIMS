using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UIMS.Web.DTO
{
    public class LoginInfoViewModel
    {
        public string Token { get; set; }

        public UserViewModel UserViewModel { get; set; }
    }
}
