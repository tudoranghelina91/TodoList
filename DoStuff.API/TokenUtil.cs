using DoStuff.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DoStuff.API
{
    public static class TokenUtil
    {
        public static int GetUserId(string t)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = Convert.ToString(t).Replace("Bearer", string.Empty).TrimStart();
            return Convert.ToInt32(jwtSecurityTokenHandler.ReadJwtToken(token).Subject);
        }

        public static string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisismySecretKey"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var token = new JwtSecurityToken(
                "Test",
                "Test",
                new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, Convert.ToString(user.Id)),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email)
                },
                expires: DateTime.UtcNow.AddMonths(1), 
                signingCredentials: credentials
            );

            return jwtSecurityTokenHandler.WriteToken(token);
        }
    }
}
