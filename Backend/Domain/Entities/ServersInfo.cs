using Newtonsoft.Json;
using Okolni.Source.Query.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ServersInfo
    {
        [JsonProperty("servers")]
        public List<ServerInfo> Servers { get; set; } = new List<ServerInfo>();

        [JsonProperty("players")]
        public int players;

        [JsonProperty("slots")]
        public int slots;

        public int countNewPlayersDay;

        public int countPlayersDay;

        public int countPlayers;
    }
}
