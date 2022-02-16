using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.SourceQuery.Models.ServerInfo;
using DAL = Domain.Entities.Server;

namespace Application
{
    public interface IServerService
    {

        public Task<List<DAL.Server>> GetServersAsync();
        public Task<ServersInfo> GetServersInfoAsync();
        public Task<DAL.Server> AddServerAsync(DAL.Server server);
        public Task<DAL.Server> DeleteServerAsync(int? id);
        public Task<DAL.Server> EditServerAsync(DAL.Server server);
    }
}
