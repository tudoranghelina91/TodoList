using DoStuff.Models;
using DoStuff.Services.Facebook;
using DoStuff.Services.Users;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DoStuff.API.Controllers
{
    public class FacebookAuthController : Controller
    {
        private readonly IFacebookAuthService _facebookAuthService;
        private readonly IUserService _userService;
        public FacebookAuthController(IFacebookAuthService facebookAuthService, IUserService userService)
        {
            _facebookAuthService = facebookAuthService;
            _userService = userService;
        }

        public async Task <ActionResult<string>> GetAccessToken(string code)
        {
            return await this._facebookAuthService.GetAccessToken(code);
        }

        public async Task<ActionResult<FacebookUserData>> GetFacebookUserInfo(string accessToken)
        {
            return await this._facebookAuthService.GetFacebookUserData(accessToken);
        }

        public async Task<ActionResult<string>> Login(string accessToken)
        {
            FacebookUserData data = await _facebookAuthService.GetFacebookUserData(accessToken);

            if (data == null)
            {
                return BadRequest();
            }

            var user = await _userService.GetUserByEmail(data.Email);
            if (user == null)
            {
                return NotFound();
            }

            return TokenUtil.GenerateToken(user);
        }
    }
}
