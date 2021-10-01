using DoStuff.Models;
using System;

namespace DoStuff.API
{
    public class AuthService : IAuthService
    {
        public bool IsAuthorized(User user) =>
            user != null && user.ExpiresIn > new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
    }
}
