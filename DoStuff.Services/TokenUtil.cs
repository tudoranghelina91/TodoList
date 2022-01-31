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
        public static string GetUserEmail(string accessToken, JwtSettings jwtSettings)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            if (!jwtSecurityTokenHandler.CanReadToken(accessToken))
            {
                return null;
            }

            var claims = jwtSecurityTokenHandler.ReadJwtToken(accessToken).Claims;

            try
            {
                SecurityToken validatedToken;
                jwtSecurityTokenHandler.ValidateToken(accessToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings.IssuerSigningKey)),
                    ValidateIssuer = jwtSettings.ValidateIssuer,
                    ValidIssuer = jwtSettings.ValidIssuer,
                    ValidateAudience = jwtSettings.ValidateAudience,
                    ValidAudience = jwtSettings.ValidAudience,
                    RequireExpirationTime = jwtSettings.RequireExpirationTime,
                    ValidateLifetime = jwtSettings.RequireExpirationTime,
                    ClockSkew = TimeSpan.FromDays(31),
                }, out validatedToken);

                if (validatedToken == null)
                {
                    return null;
                }
            }

            catch
            {
                return null;
            }


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
