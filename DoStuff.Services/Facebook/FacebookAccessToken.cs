using Newtonsoft.Json;

namespace DoStuff.Services.Facebook
{
    public class FacebookAccessToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}
