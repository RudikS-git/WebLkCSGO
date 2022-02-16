using System;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SteamIDs_Engine;
using Domain.Contexts;
using Domain.Entities.Account;
using Domain.Entities.Product;
using Infrastructure.Exceptions;
using DAL = Domain.Entities.Privilege;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class PrivilegeService : IPrivilegeService
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<PrivilegeService> _privilegeService;
        private readonly ITypePrivilegeService _typePrivilegeService;
        private readonly IConfiguration _configuration;

        public PrivilegeService(DataContext dataContext, 
                                ILogger<PrivilegeService> privilegeService,
                                ITypePrivilegeService typePrivilegeService,
                                IConfiguration configuration)
        {
            _dataContext = dataContext;
            _privilegeService = privilegeService;
            _typePrivilegeService = typePrivilegeService;
            _configuration = configuration;
        }

        public DAL.Privilege Get(int steamId)
        {
            return _dataContext.Privileges.Where(it => it.AuthId32 == steamId)
                                          .FirstOrDefault();
        }

        public async Task<int> Set(int steamId, int typePrivilegeId)
        {
            var privilegeUser = _dataContext.Privileges.Where(it => it.AuthId32 == steamId).FirstOrDefault();
            var typePrivilege = _dataContext.TypePrivileges.Where(it => it.Id == typePrivilegeId).FirstOrDefault();

            if (typePrivilege == null)
            {
                // добавить логгер
            }

            if (!typePrivilege.Sale)
            {
                throw new ServiceException("Данная привилегия не продаётся");
            }

            if (privilegeUser != null)
            {
                var currentPrivilege = await _dataContext.TypePrivileges.Where(it => it.GroupName == privilegeUser.GroupName)
                                                                                        .FirstOrDefaultAsync();

                if (currentPrivilege != null)
                {
                    if (typePrivilege.Lvl > currentPrivilege.Lvl) // не разрешаем менять на более низкий уровень
                    {
                        privilegeUser.GroupName = typePrivilege.GroupName;
                        return await UpdatePrivilege(privilegeUser);
                    }
                }
            }
            else
            {
                DAL.Privilege privilege = new DAL.Privilege()
                {
                    AuthId32 = steamId,
                    GroupName = typePrivilege.GroupName,
                    Name = " ",
                    Expires = 0,
                    ServerId = 0,
                    LastVisit = 0
                };
                
                return await SetPrivilege(privilege);
            }

            return -1;
        }

        public async Task SavePrivilege(DAL.Privilege privilege)
        {
            await _dataContext.SaveChangesAsync();

            var server = await FindServerByPlayer(SteamIDConvert.Steam32ToSteam2(privilege.AuthId32));

            if (server != null)
            {
                Rcon rcon = new Rcon(server.Ip, server.Port, _configuration.GetValue<string>("Rcon"));
                await rcon.Connect();
                await rcon.RefreshVips();
            }
        }


        public async Task<int> UpdatePrivilege(DAL.Privilege privilege)
        {
            var updatePrivilege = _dataContext.Privileges.Update(privilege);

            if (updatePrivilege != null)
            {
                await SavePrivilege(privilege);
                return updatePrivilege.Entity.Id;
            }

            return -1;
        }
        
        public async Task<int> SetPrivilege(DAL.Privilege privilege)
        {
            var addedPrivilege = await _dataContext.Privileges.AddAsync(privilege);

            if (addedPrivilege != null)
            {
                await SavePrivilege(privilege);
                return addedPrivilege.Entity.Id;
            }

            return -1;
        }

        /// <summary>
        /// Ищет игрока на серверах
        /// </summary>
        /// <param name="steamId"></param>
        /// <returns>Возвращает экземпляр сервера, если игрок найден , иначе null</returns>
        public async Task<Domain.Entities.Server.Server> FindServerByPlayer(string steamId)
        {
            var servers = _dataContext.ServerInfo;

            foreach (var it in servers)
            {
                Rcon rcon = new Rcon(it.Ip, it.Port, _configuration.GetValue<string>("Rcon"));
                await rcon.Connect();
                string status = await rcon.Status();

                if (status.Contains(steamId))
                {
                    return new Domain.Entities.Server.Server(it.Ip, it.Port);
                }
            }

            return null;
        }

        public async Task<bool> IsCurrentPrivilegeBetter(int steamId, TypePrivilege typePrivilege)
        {
            var privilege = await _dataContext.Privileges.Where(it => it.AuthId32 == steamId)
                                                         .FirstOrDefaultAsync();

            if (privilege != null)
            {
                var currentTypePrivilege = await _typePrivilegeService.GetTypePrivilegeAsync(privilege.GroupName);

                if (currentTypePrivilege != null && currentTypePrivilege.Lvl >= typePrivilege.Lvl)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
