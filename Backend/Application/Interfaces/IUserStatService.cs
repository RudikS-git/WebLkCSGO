using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities.UserStat;

using Dal = Domain.Entities.UserStat;

namespace Application
{
    public interface IUserStatService
    {
        public Dal.UserStat Get(string steamId);
        public Task<int> GetCountPlayersDayAsync();
        public Task<int> GetCountNewPlayersDayAsync();
        public Task<int> GetCountPlayersAsync();

        public IEnumerable<Dal.UserStat> GetTopPlayersByPoints(int count);
        public IEnumerable<Dal.UserStat> GetTopPlayersByHours(int count);
    }
}
