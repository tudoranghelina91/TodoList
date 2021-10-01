using DoStuff.Models;

namespace DoStuff.API
{
    public interface IAuthService
    {
        bool IsAuthorized(User user);
    }
}
