using Newtonsoft.Json;

namespace Infrastructure.SteamID.Models
{
    public class UserInfoResponse
    {
        [JsonProperty("steamid")]
        public string SteamId { get; set; }

        [JsonProperty("communityvisibilitystate")]
        public int CommunityVisState { get; set; }

        [JsonProperty("profilestate")]
        public int ProfileState { get; set; }

        [JsonProperty("personaname")]
        public string Name { get; set; }

        [JsonProperty("commentpermission")]
        public int CommentPermission { get; set; }

        [JsonProperty("profileurl")]
        public string ProfileUrl { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("avatarmedium")]
        public string AvatarMedium { get; set; }

        [JsonProperty("avatarfull")]
        public string AvatarFull { get; set; }

        [JsonProperty("avatarhash")]
        public string AvatarHash { get; set; }

        [JsonProperty("lastlogoff")]
        public long LastLogoff { get; set; }

        [JsonProperty("personastate")]
        public int PersonaState { get; set; }

        [JsonProperty("realname")]
        public string RealName { get; set; }

        [JsonProperty("primaryclanid")]
        public string PrimaryClanId { get; set; }

        [JsonProperty("timecreated")]
        public string TimeCreated { get; set; }

        [JsonProperty("gameid")]
        public string GameId { get; set; }

        [JsonProperty("gameserverip")]
        public string GameServerIp { get; set; }

        [JsonProperty("gameextrainfo")]
        public string GameExtraInfo { get; set; }

        [JsonProperty("cityid")]
        public string CityId { get; set; }

        [JsonProperty("loccountrycode")]
        public string LocStateCode { get; set; }

        [JsonProperty("loccityid")]
        public string LocCityId { get; set; }
    }
}
