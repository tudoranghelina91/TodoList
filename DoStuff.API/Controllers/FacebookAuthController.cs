using DoStuff.Models;
using DoStuff.Models.Settings;
using DoStuff.Services.Facebook;
using DoStuff.Services.Users;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DoStuff.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacebookAuthController : Controller
    {
        private readonly IFacebookAuthService _facebookAuthService;
        private readonly IUserService _userService;
        private readonly JwtSettings _jwtSettings;
        public FacebookAuthController(IFacebookAuthService facebookAuthService, IUserService userService, JwtSettings jwtSettings)
        {
            _facebookAuthService = facebookAuthService;
            _userService = userService;
            _jwtSettings = jwtSettings;
        }

        [HttpGet]
        public async Task<ActionResult<string>> Login([FromQuery] string code)
        {
            if (code == null)
            {
                return BadRequest();
            }
            
            FacebookAccessToken accessToken = await this._facebookAuthService.GetAccessToken(code);
            
            if (accessToken == null)
            {
                return BadRequest();
            }

            FacebookUserData data = await _facebookAuthService.GetFacebookUserData(accessToken);

            if (data == null)
            {
                return BadRequest();
            }

            var user = await _userService.GetUserByEmail(data.Email);

            if (user == null)
            {
                user = new User
                {
                    Email = data.Email
                };

                user = await this._userService.Insert(user);
            }

            return TokenUtil.GenerateToken(user, _jwtSettings);
        }
    }
}
