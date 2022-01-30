using DoStuff.Models;
using DoStuff.Models.Settings;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace DoStuff.API
{
    public static class TokenUtil
    {
        public static string GetUserEmail(string accessToken)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = Convert.ToString(accessToken).Replace("Bearer", string.Empty).TrimStart();
            var claims = jwtSecurityTokenHandler.ReadJwtToken(token).Claims;
            return claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email).Value;
        }

        public static string GenerateToken(User user, JwtSettings jwtSettings)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.IssuerSigningKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var token = new JwtSecurityToken(
                jwtSettings.ValidIssuer,
                jwtSettings.ValidAudience,
                new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, Convert.ToString(user.Id)),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email)
                },
                expires: DateTime.UtcNow.AddDays(31), 
                signingCredentials: credentials
            );

            return jwtSecurityTokenHandler.WriteToken(token);
        }
    }
}
