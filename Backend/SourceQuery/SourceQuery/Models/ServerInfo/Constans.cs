using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.SourceQuery.Models.ServerInfo
{
    public static class Constants
    {
        /// <summary>
        /// Retrieves information about the server including, but not limited to: its name, the map currently being played, and the number of players.
        /// </summary>
        public static byte[] A2S_INFO_REQUEST = new byte[]
        {
                0xFF, 0xFF, 0xFF, 0xFF, 0x54, 0x53, 0x6F, 0x75, 0x72, 0x63, 0x65, 0x20, 0x45, 0x6E, 0x67, 0x69,
                0x6E, 0x65, 0x20, 0x51, 0x75, 0x65, 0x72, 0x79, 0x00
        };

        /// <summary>
        /// The middle byte of the challenge response.
        /// </summary>
        public static byte CHALLENGE_RESPONE = 0x41;

        /// <summary>
        /// This query retrieves information about the players currently on the server. It needs an initial step to acquire a challenge number.
        /// </summary>
        public static byte[] A2S_PLAYER_CHALLENGE_REQUEST = new byte[] {
            0xFF, 0xFF, 0xFF, 0xFF, 0x55, 0xFF, 0xFF, 0xFF, 0xFF
        };

        public const byte A2S_PLAYER_RESPONSE = 0x44;

        /// <summary>
        /// Returns the server rules, or configuration variables in name/value pairs. This query requires an initial challenge step.
        /// </summary>
        public static byte[] A2S_RULES_CHALLENGE_REQUEST = new byte[] {
            0xFF, 0xFF, 0xFF, 0xFF, 0x56, 0xFF, 0xFF, 0xFF, 0xFF
        };

        /// <summary>
        /// Ping the server to see if it exists, this can be used to calculate the latency to the server.
        /// </summary>
        public static byte[] A2A_PING_REQUEST = new byte[] {
            0xFF, 0xFF, 0xFF, 0xFF, 0x69
        };

        /// <summary>
        /// A2S_PLAYER and A2S_RULES queries both require a challenge number. Formerly, this number could be obtained via an A2S_SERVERQUERY_GETCHALLENGE request.
        /// </summary>
        public static byte[] A2S_SERVERQUERY_GETCHALLENGE_REQUEST = new byte[]
        {
            0xFF, 0xFF, 0xFF, 0xFF, 0x57
        };

        /// <summary>
        /// Mapping for Byte to ServerType conversion
        /// </summary>
        public static Dictionary<byte, EnumsServer.ServerType> ByteServerTypeMapping = new Dictionary<byte, EnumsServer.ServerType>() {
            { 0x64, EnumsServer.ServerType.Dedicated },
            { 0x6c, EnumsServer.ServerType.NonDedicated },
            { 0x70, EnumsServer.ServerType.SourceTvRelay },
            { 0x00, EnumsServer.ServerType.NonDedicated } //Rag Doll Kung Fu
        };

        /// <summary>
        /// Mapping for Byte to Environment conversion
        /// </summary>
        public static Dictionary<byte, EnumsServer.Environment> ByteEnvironmentMapping = new Dictionary<byte, EnumsServer.Environment>()
        {
            { 0x6c, EnumsServer.Environment.Linux },
            { 0x77, EnumsServer.Environment.Windows },
            { 0x6d, EnumsServer.Environment.Mac },
            { 0x6f, EnumsServer.Environment.Mac }
        };

        /// <summary>
        /// Mapping for Byte to TheShipMode conversion
        /// </summary>
        public static Dictionary<byte, EnumsServer.TheShipMode> ByteTheShipModeMapping = new Dictionary<byte, EnumsServer.TheShipMode>()
        {
            { 0x00, EnumsServer.TheShipMode.Hunt },
            { 0x01, EnumsServer.TheShipMode.Elimination },
            { 0x02, EnumsServer.TheShipMode.Duel },
            { 0x03, EnumsServer.TheShipMode.Deathmatch },
            { 0x04, EnumsServer.TheShipMode.VipTeam },
            { 0x05, EnumsServer.TheShipMode.TeamElimination }
        };

        public static Dictionary<byte, EnumsServer.Visibility> ByteVisibilityMapping = new Dictionary<byte, EnumsServer.Visibility>()
        {
            { 0x00, EnumsServer.Visibility.Public },
            { 0x01, EnumsServer.Visibility.Private }
        };

        public static uint SimpleResponseHeader = 0xFFFFFFFF;

        public static uint MultiPacketResponseHeader = 0xFFFFFFFE;

        public static short TheShipGameId = 2400;
    }
}
