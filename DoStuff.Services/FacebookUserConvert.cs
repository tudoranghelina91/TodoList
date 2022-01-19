using DoStuff.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoStuff.Services
{
    public static class Convert
    {
        public static User ToUser(FacebookUserData facebookUserData)
        {
            return new User
            {
                Email = facebookUserData.Email
            };
        }
    }
}
