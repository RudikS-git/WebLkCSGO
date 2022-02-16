using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Application.DTO;
using AutoMapper;
using AutoMapper.Internal;
using Microsoft.EntityFrameworkCore;
using Domain.Context;
using Domain.Contexts;
using Domain.Entities.Account;
using Domain.Entities.Product;
using Infrastructure.Exceptions;

namespace Infrastructure.Services
{
    public class TypePrivilegeService : ITypePrivilegeService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public TypePrivilegeService(DataContext context, 
                                    IMapper mapper
                                    )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TypePrivilegeDTO>> GetPrivilegesAsync(int steamId)
        {
            List<TypePrivilege> privileges = null;
            privileges = await _context.TypePrivileges
                                    .Include(it => it.Features)
                                    .Include(it => it.Discount)
                                    .AsNoTracking()
                                    .Where(it => it.Sale == true)
                                    .ToListAsync();

            var privilegesDTO = _mapper.Map<List<TypePrivilegeDTO>>(privileges);

            var privilege = await _context.Privileges.Where(it => it.AuthId32 == steamId)
                .FirstOrDefaultAsync();

            if (privilege != null)
            {
                var currentTypePrivilege = await GetTypePrivilegeAsync(privilege.GroupName);

                if (currentTypePrivilege != null && currentTypePrivilege.Sale)
                {
                    for (int i = 0; i < privileges.Count(); i++)
                    {
                        if (privileges[i].Lvl > currentTypePrivilege.Lvl)
                        {
                            privilegesDTO[i].DiscountPrice = await CalculatePriceAsync(privileges[i], steamId);
                        }
                    }
                }
            }

            return privilegesDTO;
        }

        public async Task<TypePrivilege> AddAsync(Domain.Entities.Product.TypePrivilege typePrivilege)
        {
            var addedPrivilege = await _context.TypePrivileges.AddAsync(typePrivilege);
            await _context.SaveChangesAsync();

            return addedPrivilege.Entity;
        }

        public async Task<TypePrivilege> EditAsync(TypePrivilege typePrivilege)
        {
            var addedtypePrivilege = _context.TypePrivileges.Update(typePrivilege);
            await _context.SaveChangesAsync();

            return addedtypePrivilege.Entity;
        }

        public async Task<TypePrivilege> DeleteAsync(TypePrivilege typePrivilege)
        {
            var removedTypePrivilege = _context.TypePrivileges.Remove(typePrivilege);
            await _context.SaveChangesAsync();

            return removedTypePrivilege.Entity;
        }

        public async Task<Feature> AddFeatureAsync(Feature feature)
        {
            var addedFeature = await _context.Feature.AddAsync(feature);
            await _context.SaveChangesAsync();

            return addedFeature.Entity;
        }

        public async Task<Feature> EditFeatureAsync(Feature feature)
        {
            var addedFeature = _context.Feature.Update(feature);
            await _context.SaveChangesAsync();
            
            return addedFeature.Entity;
        }

        public async Task<TypePrivilege> DeleteFeatureAsync(TypePrivilege typePrivilege)
        {
            var removedTypePrivilege = _context.TypePrivileges.Remove(typePrivilege);
            await _context.SaveChangesAsync();

            return removedTypePrivilege.Entity;
        }

        public async Task<TypePrivilege> GetTypePrivilegeAsync(string groupName)
        {
            return await _context.TypePrivileges.Include(it => it.Discount)
                                                .Where(it => it.GroupName == groupName)
                                                .FirstOrDefaultAsync();
        }

        public async Task<TypePrivilege> GetTypePrivilegeAsync(int id)
        {
            return await _context.TypePrivileges.Include(it => it.Discount)
                                                .Where(it => it.Id == id)
                                                .FirstOrDefaultAsync();
        }

        public async Task<decimal> CalculatePriceAsync(TypePrivilege typePrivilege, int steamId)
        {
            var privilege = await _context.Privileges.Where(it => it.AuthId32 == steamId)
                                                     .FirstOrDefaultAsync();
            AccountingModel accountingModel;
            
            if (privilege != null)
            {
                var currentTypePrivilege = await GetTypePrivilegeAsync(privilege.GroupName);

                if (currentTypePrivilege == null)
                {
                    throw new ServiceException("Имеется привилегия, которая не может быть изменена");
                }

                accountingModel = new TypePrivilegeAccountingModel(steamId, privilege, currentTypePrivilege);
            }
            else
            {
                accountingModel = new DefaultAccountingModel();
            }

            var newPrice = accountingModel.CalculateFinallyPrice(typePrivilege);

            if (newPrice <= 0)
            {
                return typePrivilege.Price;
            }

            return newPrice;
        }  
    }
}
