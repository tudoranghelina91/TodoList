using System.Threading.Tasks;

namespace DoStuff.Services.Facebook
{
    public interface IFacebookAuthService
    {
        Task<FacebookAccessToken> GetAccessToken(string code);
        Task<FacebookUserData> GetFacebookUserData(FacebookAccessToken accessToken);
    }
}
