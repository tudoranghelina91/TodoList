using DoStuff.DAL;
using DoStuff.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task Insert(User user)
        {
            await this._context.Users.AddAsync(user);
            await this._context.SaveChangesAsync();
        }
    }
}
