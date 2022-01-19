﻿using System.Threading.Tasks;

namespace DoStuff.Services.Facebook
{
    public interface IFacebookAuthService
    {
        Task<string> GetAccessToken(string code);
        Task<FacebookUserData> GetUserInfo(string accessToken);
    }
}
