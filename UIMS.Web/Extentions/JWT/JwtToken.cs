using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UIMS.Web.Data.AppConfigurations;
using UIMS.Web.Models;

namespace UIMS.Web.Extentions.JWT
{
    public static class JwtToken
    {
        public static JwtSecurityToken GetSecurityToken(AppUser user, List<string> roles)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("roles",string.Join(',',roles))
            };
            var tokenInfo = new TokenConfiguration().GetTokenAuthenticationInfo();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenInfo.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                    tokenInfo.Issuer,
                    tokenInfo.Audience,
                    claims,
                    expires: DateTime.Now.AddYears(1),
                    signingCredentials: creds);

            return token;


        }
    }
}
