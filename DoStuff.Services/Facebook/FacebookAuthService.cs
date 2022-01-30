using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DoStuff.Services.Facebook
{
    public class FacebookAuthService : IFacebookAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfigurationSection _facebookConfigurationSection;
        public FacebookAuthService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://graph.facebook.com");
            _facebookConfigurationSection = configuration.GetSection("Facebook");
        }

        public async Task<FacebookAccessToken> GetAccessToken(string code)
        {
            string clientId = _facebookConfigurationSection["ClientId"];
            string clientSecret = _facebookConfigurationSection["ClientSecret"];
            string redirectUri = _facebookConfigurationSection["RedirectUri"];
            var response = await _httpClient.GetAsync($"/v12.0/oauth/access_token?client_id={clientId}&client_secret={clientSecret}&redirect_uri={redirectUri}&code={code}");
            
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
