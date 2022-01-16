using System;
using System.IdentityModel.Tokens.Jwt;
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
    }
}
