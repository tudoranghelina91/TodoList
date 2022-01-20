using DoStuff.DAL;
using DoStuff.Models;
using DoStuff.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DoStuff.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(User user)
        {
            var u = await this._userService.GetUserByEmail(user.Email);

            if (u == null)
            {
                return NotFound();
            }

            var hashedPassword = PasswordUtil.Decode(user.HashedPassword, u.Salt);

            if (u.HashedPassword == hashedPassword)
            {
                return TokenUtil.GenerateToken(u);
            }

            return Unauthorized();
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register(User user)
        {
            if (user.Email == null || user.HashedPassword == null)
            {
                return BadRequest();
            }

            var u = await this._userService.GetUserByEmail(user.Email);

            if (u == null)
            {
                byte[] salt;

                user.HashedPassword = PasswordUtil.Encode(user.HashedPassword, out salt);
                user.Salt = salt;

                await this._userService.Insert(user);

                return Ok();
            }

            return BadRequest();
        }
    }
}
