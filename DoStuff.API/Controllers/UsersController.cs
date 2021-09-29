using DoStuff.DAL;
using DoStuff.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;

namespace DoStuff.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly TodoListContext _context;
        public UsersController(TodoListContext context)
        {
            _context = context;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<User>> Login(User user)
        {
            User u = await GetUserByAccessToken();

            if (u != null)
            {
                return u;
            }

            u = await this._context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (u == null)
            {
                return NotFound();
            }

            var hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            user.HashedPassword,
            u.Salt,
            KeyDerivationPrf.HMACSHA1,
            10000,
            256 / 8));

            if (u.HashedPassword == hashedPassword)
            {
                u.AccessToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                u.ExpiresIn = new DateTimeOffset(DateTime.UtcNow.AddMonths(1)).ToUnixTimeMilliseconds();

                await this._context.SaveChangesAsync();
                return u;
            }

            return Unauthorized();
        }

        private async Task<User> GetUserByAccessToken()
        {
            var utcNowTimestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
            return await this._context.Users.FirstOrDefaultAsync(u => u.AccessToken == Request.Headers["Authorization"].ToString() && u.ExpiresIn > utcNowTimestamp);
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register(User user)
        {
            var u = await this._context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (u == null)
            {
                user.Salt = new byte[128 / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(user.Salt);
                }

                user.HashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    user.HashedPassword,
                    user.Salt,
                    KeyDerivationPrf.HMACSHA1,
                    10000,
                    256 / 8));

                await this._context.Users.AddAsync(user);
                await this._context.SaveChangesAsync();

                return Ok();
            }

            return BadRequest();
        }

        [HttpGet("Details")]
        public async Task<ActionResult<User>> GetUserDetails()
        {
            return await GetUserByAccessToken();
        }
    }
}
