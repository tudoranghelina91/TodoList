using DoStuff.DAL;
using DoStuff.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DoStuff.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly TodoListContext _context;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        public UsersController(TodoListContext context, JwtSecurityTokenHandler jwtSecurityTokenHandler)
        {
            _context = context;
            _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisismySecretKey"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var u = await this._context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

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
                var token = new JwtSecurityToken(
                    "Test",
                    "Test",
                    new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, Convert.ToString(u.Id)),
                        new Claim(JwtRegisteredClaimNames.Email, u.Email)
                    },
                    expires: DateTime.UtcNow.AddMonths(1), signingCredentials: credentials);

                await this._context.SaveChangesAsync();
                return _jwtSecurityTokenHandler.WriteToken(token);
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
    }
}
