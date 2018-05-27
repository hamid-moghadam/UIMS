using System;
using System.Collections.Generic;
using System.Text;

namespace UIMS.Web.Data.Helpers
{
    public sealed class TokenAuthenticationInfo
    {
        public string SecretKey { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

    }
}
