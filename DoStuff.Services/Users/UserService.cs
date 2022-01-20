using DoStuff.DAL;
using DoStuff.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DoStuff.Services.Users
{
    public class UserService : IUserService
    {
        private readonly TodoListContext _context;
        public UserService(TodoListContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await this._context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> Insert(User user)
        {
            await this._context.Users.AddAsync(user);
            await this._context.SaveChangesAsync();
            return await this._context.Users.SingleOrDefaultAsync(u => u.Email == user.Email);
        }
    }
}
