using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.SteamID.Models
{
    [JsonObject("response")]
    public class UsersInfoResponse
    {
        [JsonProperty("players")]
        public List<UserInfoResponse> players;
    }
}
