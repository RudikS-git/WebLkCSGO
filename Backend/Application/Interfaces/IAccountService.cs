using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTO;
using Domain.Entities.Account;

namespace Application
{
    public interface IAccountService
    {
        public Task<AccountDTO> GetUserInfo(string steamId64);
        public Task<User> GetUser(string steamId);
        public Role GetRole(string steamId);
        public Task<decimal> GetBalance(string steamId);

        public Task AddBalanceAsync(string steamId, decimal sum);
        public Task InsertNewUser(User user);
        public Task<User> CreateUserAsync(string steamId);
        public Task<bool> UserIsExist(string steamId);

        public Task RejectToken(string authId, string valueRefreshToken);
        public Task SaveRefreshToken(string authId, string refreshToken);

        public Task<bool> ValidateLastChanged(string authId, string lastChanged);


    }
}
