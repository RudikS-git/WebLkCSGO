using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Application;
using Application.DTO;
using Microsoft.EntityFrameworkCore;

using Domain.Contexts;
using Domain.Entities;
using Domain.Entities.Account;
using Infrastructure.Exceptions;
using Infrastructure.SteamID.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using SteamIDs_Engine;
using DAL = Domain.Entities.Product;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly DataContext context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountService> _logger;
        private readonly IPrivilegeService _privilegeService;
        private readonly IUserStatService _userStatService;
        private readonly SteamService _steamService;

        public AccountService(DataContext accountContext, 
                              IConfiguration configuration,
                              ILogger<AccountService> logger,
                              IPrivilegeService privilegeService,
                              IUserStatService userStatService,
                              SteamService steamService)
        {
            context = accountContext;
            _configuration = configuration;
            _logger = logger;
            _privilegeService = privilegeService;
            _userStatService = userStatService;
            _steamService = steamService;
        }
        
        public void Login()
        {
            
        }

        public async Task<AccountDTO> GetUserInfo(string steamId64)
        {
            UsersInfoResponse usersInfo;
            try
            {
                usersInfo = await _steamService.GetUserInfo(steamId64);

                if (usersInfo == null)
                {
                    throw new ServiceException("Не удалось получить данные STEAM данного игрока");
                }
            }
            catch (HttpRequestException)
            {
                throw new ServiceException("Не удалось получить данные STEAM данного игрока");
            }

            string name = usersInfo.players[0].Name; // userInfo.response.players[0].personaname;
            string avatarSource = usersInfo.players[0].Avatar; //userInfo.response.players[0].avatar;
            string steamId = usersInfo.players[0].SteamId;// = userInfo.response.players[0].steamid;  //userInfo.SteamId;  //SteamIDConvert.Steam64ToSteam2(id);

            //User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString() - получение steam id юзера, который вызвал action

            User user = await GetUser(steamId);
            _logger.LogDebug($"{steamId} | {user}");

            if (user == null)
            {
                _logger.LogDebug($"{steamId} | user is null");
                throw new ServiceException("User is null");
            }

            var userStat = _userStatService.Get(SteamIDConvert.Steam64ToSteam2(long.Parse(user.Auth64Id)));
            _logger.LogDebug($"{steamId} | {userStat}");

            if (userStat == null)
            {
                _logger.LogDebug($"{steamId} | userStat is null");
                throw new ServiceException("UserStat is null");
            }

            var privilege = _privilegeService.Get(SteamIDConvert.Steam64ToSteam32(long.Parse(user.Auth64Id)));

            _logger.LogDebug($"{steamId} | {privilege}");

            return new AccountDTO()
            {
                IsAuthenticated = true,
                Name = name,
                AvatarSource = avatarSource,
                SteamId = steamId,
                Role = user.Role,
                UserStat = userStat,
                Privilege = privilege

            };
        }
        
        public Role GetRole(string steamId)
        {
            return context.User.Where(it => it.Auth64Id == steamId)
                .Include(u => u.Role)
                .First().Role;

            /* using(MySqlConnection mySqlConnection = GetConnection())
             {
                 await mySqlConnection.OpenAsync();
                 MySqlCommand command = new MySqlCommand("SELECT `role` FROM `lk_users` WHERE `auth` = @steamId", mySqlConnection);
                 command.Parameters.Add("@steamId", MySqlDbType.Int64).Value = auth;
 
                 using (var reader = await command.ExecuteReaderAsync())
                 {
                     reader.Read();
                     string role = reader[0].ToString();
 
                     return role;
                 }
             }*/
        }

        public async Task AddBalanceAsync(string steamId, decimal sum)
        {
            User user = await GetUser(steamId);

            if (user != null)
            {
                context.User.Update(user);
                await context.SaveChangesAsync();
            }

            /* using (MySqlConnection mySqlConnection = GetConnection())
             {
                 await mySqlConnection.OpenAsync();
                 MySqlCommand command = new MySqlCommand("UPDATE `players_money` SET `money` = `money` + @sum WHERE `auth` = @steamId", mySqlConnection);
                 command.Parameters.Add("@sum", MySqlDbType.Decimal).Value = sum;
                 command.Parameters.Add("@steamId", MySqlDbType.String).Value = steamId;
 
                 var rows = await command.ExecuteNonQueryAsync();
 
             }*/
        }

        public async Task<User> GetUser(string steamId)
        {
            return await context.User.Where(it => it.Auth64Id == steamId)
                .Include(u => u.Role)
                .Include(it => it.RefreshTokens)
                .FirstOrDefaultAsync();
        }

        public async Task InsertNewUser(User user)
        {
            await context.User.AddAsync(user);

            /*using (MySqlConnection mySqlConnection = GetConnection())
            {
                await mySqlConnection.OpenAsync();

                /*MySqlCommand command = new MySqlCommand("INSERT `players_money` SET `money` = `money` + @sum WHERE `auth` = @steamId", mySqlConnection);
                command.Parameters.Add("@sum", MySqlDbType.Decimal).Value = sum;
                command.Parameters.Add("@steamId", MySqlDbType.String).Value = steamId;

                var rows = await command.ExecuteNonQueryAsync();
            }*/
        }

        public async Task<User> CreateUserAsync(string steamId)
        {
            User user = new User()
            {
                RoleId = 1,
                Auth64Id = steamId,
                Role = await context.Roles.FindAsync(1)
            };

            context.User.Add(user);
            await context.SaveChangesAsync();

            return user;

            /* using (MySqlConnection mySqlConnection = GetConnection())
             {
                 await mySqlConnection.OpenAsync();
                 MySqlCommand command = new MySqlCommand("INSERT INTO `lk_users` SET `auth` = @steamId, `role`='User'", mySqlConnection);
                 command.Parameters.Add("@steamId", MySqlDbType.String).Value = steamId;
 
                 await command.ExecuteNonQueryAsync();
             }*/
        }

        public async Task<bool> UserIsExist(string steamId)
        {
            return (await context.User.Where(it => it.Auth64Id == steamId).CountAsync()) != 0 ? true : false;

            /* using (MySqlConnection mySqlConnection = GetConnection())
             {
                 await mySqlConnection.OpenAsync();
                 MySqlCommand command = new MySqlCommand("SELECT EXISTS(SELECT auth FROM `lk_users` WHERE auth = @steamId)", mySqlConnection);
                 command.Parameters.Add("@steamId", MySqlDbType.String).Value = steamId;

                 using (var reader = await command.ExecuteReaderAsync())
                 {
                     reader.Read();

                     return reader[0].ToString() == "1" ? true : false;
                 }
             }*/
        }

        public async Task<decimal> GetBalance(string steamId)
        {
            var user = await context.User.Where(it => it.Auth64Id == steamId).FirstOrDefaultAsync();
            return -1;

            /*
             *  STEAM_X: Y: Z.
             *  STEAM64 =  Z * 2 + V + Y
             */
            // string[] parts = steamId.Split(new char[] { '_', ':' });

            // long steam64 = Convert.ToInt32(parts[3]) * 2 + 1 + Convert.ToInt32(parts[2]);

            //https://steamcommunity.com/openid/id/76561198116635803

            //string id = steamId.Replace("https://steamcommunity.com/openid/id/", "");

            /* using (MySqlConnection mySqlConnection = GetConnection())
             {
                 await mySqlConnection.OpenAsync();
                 MySqlCommand command = new MySqlCommand("SELECT `cash` FROM `lk_user` WHERE `auth` = @steamId", mySqlConnection);
                 command.Parameters.Add("@steamId", MySqlDbType.Int64).Value = steamId;

                 using (var reader = await command.ExecuteReaderAsync())
                 {
                     reader.Read();
                     decimal balance = Convert.ToDecimal(reader[0]);

                     return balance;
                 }
             }*/
        }

        public async Task<DAL.TypePrivilege> GetPrivilegy(int id)
        {
            return await context.TypePrivileges.FindAsync(id);
        }

        public async Task<bool> ValidateLastChanged(string authId, string lastChanged)
        {
            //DATE_FORMAT(CURRENT_TIMESTAMP(), '%d.%m.%Y %H:%i')
            if (DateTime.TryParse(lastChanged, out DateTime lastCDateTime))
            {
                var user = await context.User.Where(it => it.Auth64Id == authId).FirstOrDefaultAsync();

                if (user != null)
                {
                    if(user.LastChanged == lastCDateTime)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public async Task RejectToken(string authId, string valueRefreshToken)
        {
            var user = await context.User.Include(it => it.RefreshTokens)
                .FirstOrDefaultAsync(it => it.Auth64Id == authId);

            if (user != null)
            {
                var refreshToken = user.RefreshTokens.SingleOrDefault(it => it.Value == valueRefreshToken);

                if (refreshToken != null)
                {
                    refreshToken.Revoked = DateTime.Now;

                    await context.SaveChangesAsync();
                }
                
            }
        }
        
        public async Task SaveRefreshToken(string authId, string refreshToken)
        {
            var user = await context.User.Include(it => it.RefreshTokens)
                                         .FirstOrDefaultAsync(it => it.Auth64Id == authId);

            if (user != null)
            {
                user.RefreshTokens.Add(new RefreshToken()
                {
                    Value = refreshToken,
                    Expires = DateTime.Now.AddDays(7),
                    Created = DateTime.Now
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
