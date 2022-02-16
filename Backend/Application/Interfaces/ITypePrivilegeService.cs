using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTO;
using Domain.Entities.Account;
using Domain.Entities.Product;
using DAL = Domain.Entities.Privilege;

namespace Application
{
    public interface ITypePrivilegeService
    {
        public Task<IEnumerable<TypePrivilegeDTO>> GetPrivilegesAsync(int steamId);
        public Task<TypePrivilege> AddAsync(TypePrivilege typePrivilege);
        public Task<TypePrivilege> EditAsync(TypePrivilege typePrivilege);
        public Task<Feature> AddFeatureAsync(Feature feature);
        public Task<Feature> EditFeatureAsync(Feature feature);

        public Task<TypePrivilege> GetTypePrivilegeAsync(string groupName);
        public Task<TypePrivilege> GetTypePrivilegeAsync(int id);
        public Task<decimal> CalculatePriceAsync(TypePrivilege typePrivilege, int steamId);
    }
}
