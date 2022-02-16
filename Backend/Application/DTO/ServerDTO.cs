using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Application.DTO
{
    public class ServerDTO
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }
    }
}
