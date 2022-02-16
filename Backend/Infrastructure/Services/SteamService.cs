using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Infrastructure.SteamID.Models;
using Newtonsoft.Json;

namespace Infrastructure.Services
{
    public class SteamService
    {
        public HttpClient Client { get; }
        private readonly IConfiguration _configuration;

        public SteamService(HttpClient client, IConfiguration configuration)
        {
            client.BaseAddress = new Uri("http://api.steampowered.com");

            Client = client;
            _configuration = configuration;
        }

        public async Task<UsersInfoResponse> GetUserInfo(string steamId64)
        {
            var response = await Client.GetAsync(
                $"/ISteamUser/GetPlayerSummaries/v0002/?key={_configuration.GetValue<string>("SteamApiKey")}&steamids={steamId64}");

            response.EnsureSuccessStatusCode();

            // stringResult = "{\"response\":{\"players\":[{\"steamid\":\"76561198116635803\",\"communityvisibilitystate\":3,\"profilestate\":1,\"personaname\":\"RudikS\",\"commentpermission\":1,\"profileurl\":\"https://steamcommunity.com/id/rudiks/\",\"avatar\":\"https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/0c/0ca825517929768cbd24c8f64f7e7beb9131a6fa.jpg\",\"avatarmedium\":\"https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/0c/0ca825517929768cbd24c8f64f7e7beb9131a6fa_medium.jpg\",\"avatarfull\":\"https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/0c/0ca825517929768cbd24c8f64f7e7beb9131a6fa_full.jpg\",\"avatarhash\":\"0ca825517929768cbd24c8f64f7e7beb9131a6fa\",\"lastlogoff\":1608505385,\"personastate\":1,\"primaryclanid\":\"103582791459643204\",\"timecreated\":1385890725,\"personastateflags\":0}]}}";

            var result = await response.Content
                .ReadAsStringAsync();

            result = result.Substring(12, result.Length - 1 - 12);

            UsersInfoResponse usersInfo = JsonConvert.DeserializeObject<UsersInfoResponse>(result);

            return usersInfo;
        }
    }
}
