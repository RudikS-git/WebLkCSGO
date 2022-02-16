using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Infrastructure.SourceQuery.Models.ServerInfo
{    
    public class InfoResponse
    {
        public int Id { get; set; }

        /// <summary>
        /// Always equal to 'I' (0x49)
        /// </summary>
        [JsonProperty("status")]
        public byte Header { get; set; }

        /// <summary>
        /// Protocol version used by the server.
        /// </summary>
        [JsonIgnore()]
        public byte Protocol { get; set; }

        /// <summary>
        /// Name of the server.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Map the server has currently loaded.
        /// </summary>
        [JsonProperty("map")]
        public string Map { get; set; }

        /// <summary>
        /// Name of the folder containing the game files.
        /// </summary>
        [JsonIgnore()]
        public string Folder { get; set; }

        /// <summary>
        /// Full name of the game.
        /// </summary>
        [JsonIgnore()]
        public string Game { get; set; }

        /// <summary>
        /// Steam Application ID of game. (Steam Application ID: https://developer.valvesoftware.com/wiki/Steam_Application_IDs)
        /// </summary>
        [JsonIgnore()]
        public short ID { get; set; }

        /// <summary>
        /// Number of players on the server.
        /// </summary>
        [JsonProperty("busySlots")]
        public int BusySlots { get; set; }

        /// <summary>
        /// Maximum number of players the server reports it can hold.
        /// </summary>
        [JsonProperty("slots")]
        public int MaxPlayers { get; set; }

        /// <summary>
        /// Number of bots on the server.
        /// </summary>
        [JsonIgnore()]
        public int Bots { get; set; }

        /// <summary>
        /// Indicates the type of server
        /// </summary>
        [JsonIgnore()]
        public EnumsServer.ServerType ServerType { get; set; }

        /// <summary>
        /// Indicates the operating system of the server
        /// </summary>
        [JsonIgnore()]
        public EnumsServer.Environment Environment { get; set; }

        /// <summary>
        /// Indicates whether the server requires a password
        /// </summary>
        [JsonIgnore()]
        public EnumsServer.Visibility Visibility { get; set; }

        /// <summary>
        /// Specifies whether the server uses VAC:
        /// false for unsecured
        /// true for secured
        /// </summary>
        [JsonIgnore()]
        public bool VAC { get; set; }

        /// <summary>
        /// [ONLY AVAILABLE IF THE SERVER IS RUNNING 'The Ship'] Indicates the game mode
        /// </summary>
        [JsonIgnore()]
        public EnumsServer.TheShipMode? Mode { get; set; }

        /// <summary>
        /// [ONLY AVAILABLE IF THE SERVER IS RUNNING 'The Ship'] The number of witnesses necessary to have a player arrested.
        /// </summary>
        [JsonIgnore()]
        public int? Witnesses { get; set; }

        /// <summary>
        /// [ONLY AVAILABLE IF THE SERVER IS RUNNING 'The Ship'] Time (in seconds) before a player is arrested while being witnessed.
        /// </summary>
        [JsonIgnore()]
        public TimeSpan? Duration { get; set; }

        /// <summary>
        /// Version of the game installed on the server.
        /// </summary>
        [JsonIgnore()]
        public string Version { get; set; }

        /// <summary>
        /// If present, this specifies which additional data fields will be included.
        /// </summary>
        [JsonIgnore()]
        public byte? EDF { get; set; }

        /// <summary>
        /// if ( EDF & 0x80 ) proves true: The server's game port number.
        /// </summary>
        [JsonProperty("port")]
        public short? Port { get; set; }

        /// <summary>
        /// Custom field for json response on client
        /// </summary>
        [JsonProperty("ip")]
        public string Ip { get; set; }

        /// <summary>
        /// if ( EDF & 0x10 ) proves true: Server's SteamID.
        /// </summary>
        [JsonIgnore()]
        public uint? SteamID { get; set; }

        /// <summary>
        /// if ( EDF & 0x40 ) proves true: Spectator port number for SourceTV.
        /// </summary>
        [JsonIgnore()]
        public short? SourceTvPort { get; set; }

        /// <summary>
        /// if ( EDF & 0x40 ) proves true: Name of the spectator server for SourceTV.
        /// </summary>
        [JsonIgnore()]
        public string SourceTvName { get; set; }

        /// <summary>
        /// if ( EDF & 0x20 ) proves true: Tags that describe the game according to the server (for future use.)
        /// </summary>
        [JsonIgnore()]
        public string KeyWords { get; set; }

        /// <summary>
        /// if ( EDF & 0x01 ) proves true: The server's 64-bit GameID. 
        /// If this is present, a more accurate AppID is present in the low 24 bits. 
        /// The earlier AppID could have been truncated as it was forced into 16-bit storage.
        /// </summary>
        [JsonIgnore()]
        public uint? GameID { get; set; }

        /// <summary>
        /// If the Server is a The Ship Server
        /// </summary>
        [JsonIgnore()]
        public bool IsTheShip => ID == 2400;

        /// <summary>
        /// If the Info contains a game Port
        /// </summary>
        [JsonIgnore()]
        public bool HasPort => Port != null;

        /// <summary>
        /// If the Info contains a SteamId
        /// </summary>
        [JsonIgnore()]
        public bool HasSteamID => SteamID != null;

        /// <summary>
        /// If the Info contains SourceTv Information
        /// </summary>
        [JsonIgnore()]
        public bool HasSourceTv => SourceTvPort != null && SourceTvName != null;

        /// <summary>
        /// If the Info contains KeyWords
        /// </summary>
        [JsonIgnore()]
        public bool HasKeywords => KeyWords != null;

        /// <summary>
        /// If the Info contains a GameID
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public bool HasGameID => GameID != null;

        public override string ToString()
        {
            string response = string.Empty;
            response += $" Header: 0x{Header.ToString("X2")};";
            response += $" Protocol: {Protocol.ToString()};";
            response += $" Name: {Name};";
            response += $" Map: {Map};";
            response += $" Folder: {Folder};";
            response += $" Game: {Game};";
            response += $" ID: {ID};";
            response += $" Players: {BusySlots};";
            response += $" Max. Players: {MaxPlayers};";
            response += $" Bots: {Bots};";
            response += $" Server type: {ServerType};";
            response += $" Environment: {Environment};";
            response += $" Visibility: {Visibility};";
            response += $" VAC: {VAC};";
            if (IsTheShip)
            {
                response += $" Mode: {Mode};";
                response += $" Witnesses: {Witnesses};";
                response += $" Duration: {Duration};";
            }
            response += $" Version: {Version};";
            if (HasGameID || HasPort || HasKeywords || HasSourceTv || HasSteamID)
            {
                if (HasPort)
                    response += $" Port: {Port};";
                if (HasSteamID)
                    response += $" SteamID: {SteamID};";
                if (HasSourceTv)
                {
                    response += $" SourceTvPort: {SourceTvPort};";
                    response += $" SourceTvName: {SourceTvName};";
                }
                if (HasKeywords)
                    response += $" Keywords: {KeyWords};";
                if (HasGameID)
                    response += $" GameID: {GameID};";
            }
            return response;
        }
    }
}
