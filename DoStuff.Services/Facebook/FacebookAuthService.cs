using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DoStuff.Services.Facebook
{
    public class FacebookAuthService : IFacebookAuthService
    {
        private readonly HttpClient _httpClient;
        public FacebookAuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://graph.facebook.com");
        }

        public async Task<string> GetAccessToken(string code)
        {
            var response = await _httpClient.GetAsync($"/v12.0/oauth/access_token?client_id=171596774608986&redirect_uri=https://localhost:4200/login&code={code}");
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<FacebookUserData> GetUserInfo(string accessToken)
        {
            var response = await _httpClient.GetAsync($"/me?access_token={accessToken}?fields=id,email,name");
            return JsonConvert.DeserializeObject<FacebookUserData>(await response.Content.ReadAsStringAsync());
        }
    }
}
