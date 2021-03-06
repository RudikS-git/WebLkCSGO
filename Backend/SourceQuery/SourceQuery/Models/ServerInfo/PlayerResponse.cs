using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.SourceQuery.Models.ServerInfo
{
    public class PlayerResponse
    {
        /// <summary>
        /// Always equal to 'D' (0x44)
        /// </summary>
        public byte Header { get; set; }

        /// <summary>
        /// List of Players on the server
        /// </summary>
        public List<Player> Players { get; set; }

        /// <summary>
        /// If the Playerresponse is a The Ship Response.
        /// </summary>
        public bool IsTheShip = false;
    }

    public class Player
    {
        /// <summary>
        /// Index of player chunk starting from 0.
        /// </summary>
        public byte Index { get; set; }

        /// <summary>
        /// Name of the player.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Player's score (usually "frags" or "kills".)
        /// </summary>
        public uint Score { get; set; }

        /// <summary>
        /// Time (in seconds) player has been connected to the server.
        /// </summary>
        public TimeSpan Duration { get; set; }

        public uint? Deaths { get; set; }
        public uint? Money { get; set; }
    }
}
