using Newtonsoft.Json;

namespace Domain.Entities.Server
{
    public class Server
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }

        [JsonProperty("state")]
        public int State { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        public Server(int id, string ip, int port)
        {
            Id = id;
            Ip = ip;
            Port = port;
        }

        public Server(string ip, int port)
        {
            Ip = ip;
            Port = port;
        }

        public Server() { }

    }
}
