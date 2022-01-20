using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace DoStuff.Services.Facebook
{
    public class FacebookAccessToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}
