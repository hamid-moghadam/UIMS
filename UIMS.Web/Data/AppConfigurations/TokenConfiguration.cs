using System;
using System.Collections.Generic;
using System.Text;
using UIMS.Web.Data.Helpers;

namespace UIMS.Web.Data.AppConfigurations
{
    public class TokenConfiguration:ConfigurationBase
    {
        public TokenAuthenticationInfo GetTokenAuthenticationInfo()
        {
            return new TokenAuthenticationInfo()
            {
                Audience = GetConfiguration().GetSection("TokenAuthenticationInfo:Audience").Value,
                Issuer = GetConfiguration().GetSection("TokenAuthenticationInfo:Issuer").Value,
                SecretKey = GetConfiguration().GetSection("TokenAuthenticationInfo:SecretKey").Value
            };
        }

    }
}
