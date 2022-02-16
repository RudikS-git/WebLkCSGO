using Okolni.Source.Query.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ServerInfo
    {
        public string Ip { get; set; }
        public short? Port { get; set; }
        public TimeSpan? Duration { get; set; }
        public bool VAC { get; set; }
        public int Bots { get; set; }
        public int MaxPlayers { get; set; }
        public int Players { get; set; }
        public string Map { get; set; }
        public string Name { get; set; }
        public PlayerResponse PlayersList;
    }
}
