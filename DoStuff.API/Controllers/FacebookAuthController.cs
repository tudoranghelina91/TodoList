using DoStuff.Services.Facebook;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DoStuff.API.Controllers
{
    public class FacebookAuthController : Controller
    {
        private readonly IFacebookAuthService _facebookAuthService;
        public FacebookAuthController(IFacebookAuthService facebookAuthService)
        {
            _facebookAuthService = facebookAuthService;
        }

        public async Task <ActionResult<string>> GetAccessToken(string code)
        {
            return await this._facebookAuthService.GetAccessToken(code);
        }

        public async Task<ActionResult<FacebookUserData>> GetFacebookUserInfo(string accessToken)
        {
            return await this._facebookAuthService.GetUserInfo(accessToken);
        }
    }
}
