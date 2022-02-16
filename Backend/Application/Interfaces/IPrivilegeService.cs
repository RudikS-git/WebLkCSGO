using System;
using System.Threading.Tasks;
using Domain.Entities.Account;
using Domain.Entities.Product;
using DAL = Domain.Entities.Privilege;

namespace Application
{
    public interface IPrivilegeService
    {
        public DAL.Privilege Get(int steamId);
        public Task<int> Set(int steamId, int typePrivilegeId);
        public Task<Domain.Entities.Server.Server> FindServerByPlayer(string steamId);
        public Task<bool> IsCurrentPrivilegeBetter(int steamId, TypePrivilege typePrivilege);
    }
}
