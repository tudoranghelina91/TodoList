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

        public async Task<FacebookAccessToken> GetAccessToken(string code)
        {
            var response = await _httpClient.GetAsync($"/v12.0/oauth/access_token?client_id=171596774608986&client_secret=a585ff6b41bfbbe6f222cbe581a69042&redirect_uri=https://localhost:4200/login&code={code}");
            
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<FacebookAccessToken>(await response.Content.ReadAsStringAsync());
        }

        public async Task<FacebookUserData> GetFacebookUserData(FacebookAccessToken accessToken)
        {
            var response = await _httpClient.GetAsync($"/me?access_token={accessToken.AccessToken}&fields=id,email,name");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<FacebookUserData>(await response.Content.ReadAsStringAsync());
        }
    }
}
