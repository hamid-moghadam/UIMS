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
        public static JwtSecurityToken GetSecurityToken(bool webLogin,AppUser user, List<string> roles)
        {
            List<Claim> claims = new List<Claim>();
            roles.ForEach(x => claims.Add(new Claim("role", x)));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            var tokenInfo = new TokenConfiguration().GetTokenAuthenticationInfo();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenInfo.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                    tokenInfo.Issuer,
                    tokenInfo.Audience,
                    claims.ToArray(),
                    expires: webLogin?DateTime.Now.AddHours(2): DateTime.Now.AddYears(1),
                    signingCredentials: creds);

            return token;


        }
    }
}
