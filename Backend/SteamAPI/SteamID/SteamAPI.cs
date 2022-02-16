using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Punishments;
using Infrastructure.SteamID.Models;
using Newtonsoft.Json;

namespace Infrastructure.SteamID
{
    public class SteamAPI
    {
        static public async Task GetUserAvatarsAsync<T>(List<T> punishments) where T: PunishmentInfo
        {
            /*Parallel.For(0, punishments.Count - 1, (i) => {
                if (!string.IsNullOrEmpty(punishments[i].AuthId))
                {
                    long steam64 = SteamIDConvert.Steam2ToSteam64(punishments[i].AuthId);
                    if(steam64 != 0)
                    {
                        punishments[i].Avatar = GetUserAvatarAsync(steam64).Result;
                    }
                }
            });*/

            // [СДЕЛАНО] переписать за 1 запрос брать всю страницу
            // Реализовать кеширование

            List<string> list = new List<string>();

            foreach (var item in punishments)
            {
                int count = punishments.Count(it => it.AdminAuthId == item.AdminAuthId);

                if (count > 1 && list.Find(it => it == item.AdminAuthId) != null)
                {
                    continue;
                }

                list.Add(item.AdminAuthId);
            }

            StringBuilder stringBuilder = new StringBuilder();
            foreach(var item in punishments)
            {
                stringBuilder.Append(item.AuthId64);
                stringBuilder.Append(',');
            }

            foreach (var item in list)
            {
                stringBuilder.Append(item);
                stringBuilder.Append(',');
            }

            var usersInfo = await GetUserInfoAsync(stringBuilder.ToString());
            for(int i = 0; i < punishments.Count; i++)
            {
                punishments[i].Avatar = usersInfo.Find(it => it.SteamId == punishments[i].AuthId64)?.Avatar;
                punishments[i].AdminAvatar = usersInfo.Find(it => it.SteamId == punishments[i].AdminAuthId)?.Avatar;
            }
        }

        static public async Task<List<UserInfoResponse>> GetUserInfoAsync(string argRequest)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    httpClient.BaseAddress = new Uri("http://api.steampowered.com");
                    var response = await httpClient.GetAsync($"/ISteamUser/GetPlayerSummaries/v0002/?key=10674EBC8BC46045E9B6977925C52E2F&steamids={argRequest}");
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    stringResult = stringResult.Substring(23, stringResult.Length - 23 - 2);
      
                    return JsonConvert.DeserializeObject<List<UserInfoResponse>>(stringResult);

                 //   var avatars = new Dictionary<string, string>();
                 //   for (int i = 0; i < count; i++)
                 //   {
                 //       avatars.Add(usersInfo[i].SteamId, usersInfo[i].Avatar);
                 //   }
                 //   dynamic userInfo = JsonConvert.DeserializeObject(stringResult);
                   // string name = userInfo.response.players[0].personaname;
                   // string avatarSource = userInfo.response.players[0].avatarfull;
                  //  long id = userInfo.response.players[0].steamid;

                  //  return avatars;
                }
                catch (HttpRequestException httpRequestException)
                {
                    throw httpRequestException;
                }

            }
        }
    }
}
