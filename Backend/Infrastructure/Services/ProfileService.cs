using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application;
using Application.DTO;
using Application.Interfaces;
using Domain.Contexts;
using Domain.Entities.Privilege;
using Domain.Entities.Rank;
using Domain.Entities.UserStat;
using Infrastructure.Exceptions;
using Infrastructure.SteamID;
using Infrastructure.Unix;
using Microsoft.EntityFrameworkCore;
using SteamIDs_Engine;

namespace Infrastructure.Services
{
    public class ProfileService : IProfileService
    {
        private readonly DataContext context;
        private readonly ITypePrivilegeService _typePrivilegeService;

        public ProfileService(DataContext dataContext, ITypePrivilegeService typePrivilegeService)
        {
            context = dataContext;
            _typePrivilegeService = typePrivilegeService;
        }

        public async Task<ProfileDTO> GetProfileAsync(int id)
        {
            var userStat = await context.UserStats.FindAsync(id);

            if (userStat == null)
            {
                throw new ServiceException("Не удалось найти такой профиль");
            }

            var userInfo =
                    await SteamAPI.GetUserInfoAsync(SteamIDConvert.Steam2ToSteam64(userStat.SteamAuth2).ToString());
                
           var steamId32 = SteamIDConvert.Steam2ToSteam32(userStat.SteamAuth2);

           Privilege privilege = await context.Privileges.Where(it => it.AuthId32 == steamId32)
                                                    .FirstOrDefaultAsync();

           int placePoints = context.UserStats
                                       // .OrderByDescending(it => it.Value)
                                        .Where(it => it.Value >= userStat.Value)
                                        .Count();

           int placeTime = context.UserStats
                                       // .OrderByDescending(it => it.PlayTime)
                                        .Where(it => it.PlayTime >= userStat.PlayTime)
                                        .Count();
           
           string groupName = "Игрок";
           if (privilege != null)
           {
               var typePrivilege = await _typePrivilegeService.GetTypePrivilegeAsync(privilege.GroupName);

               if (typePrivilege != null)
               {
                   groupName = typePrivilege.Name;
               }
               else
               {
                   groupName = privilege.GroupName;
               }
           }

           var rank = await context.Rank.Where(it => it.Lvl == userStat.Lvl).FirstOrDefaultAsync();
           var nextRank = await context.Rank.Where(it => it.Lvl == userStat.Lvl + 1).FirstOrDefaultAsync();

            ProfileDTO profileDto = new ProfileDTO()
            {
                Id = id,
                Auth64 = userInfo[0].ProfileUrl,
                Name = userInfo[0].Name,
                Value = userStat.Value,
                Assists = userStat.Assists,
                Deaths = userStat.Deaths,
                Headshots = userStat.Headshots,
                Hits = userStat.Hits,
                Kills = userStat.Kills,
                Lvl = userStat.Lvl,
                PlayTime = userStat.PlayTime,
                RoundLosses = userStat.RoundLosses,
                RoundWins = userStat.RoundWins,
                Shoots = userStat.Shoots,
                LastConnection = DateTimeUnix.UnixTimeStampToDateTime(userStat.LastConnection),

                AvatarSource = userInfo[0].AvatarFull,
                GroupName = groupName,
                PlaceTopOfPoints = placePoints,
                PlaceTopOfTime = placeTime,
                Rank = rank,
                NextRank = nextRank

            };

            return profileDto;

        }
        
    }
}
