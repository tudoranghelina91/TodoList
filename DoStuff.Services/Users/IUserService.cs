using DoStuff.Models;
using System.Threading.Tasks;

namespace DoStuff.Services.Users
{
    public interface IUserService
    {
        Task<User> GetUserByEmail(string email);

        Task<User> Insert(User user);
    }
}
