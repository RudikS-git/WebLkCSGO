using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Domain.Context;
using Domain.Contexts;
using Domain.Entities.UserStat;
using Microsoft.EntityFrameworkCore.Internal;
using Dal = Domain.Entities.UserStat;

namespace Infrastructure.Services
{
    public class UserStatService : IUserStatService
    {
        private DataContext context;

        public UserStatService(DataContext dataContext)
        {
            context = dataContext;
        }

        public UserStat Get(string? steamId)
        {
            return context.UserStats.Where(it => it.SteamAuth2 == steamId).SingleOrDefault();
        }

        public async Task<int> GetCountPlayersDayAsync()
        {
            long startCurrentDay = new DateTimeOffset(DateTime.Today).ToUnixTimeSeconds();
            return await context.UserStats.CountAsync(it => it.LastConnection >= startCurrentDay);
        }

        public async Task<int> GetCountNewPlayersDayAsync()
        {
            long nowTime = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
            long todayTime = new DateTimeOffset(DateTime.Today).ToUnixTimeSeconds();

            return await context.UserStats.CountAsync(it => it.LastConnection >= todayTime && nowTime - todayTime >= it.PlayTime);
        }

        public async Task<int> GetCountPlayersAsync()
        {
            return await context.UserStats.CountAsync();
        }

        public IEnumerable<Dal.UserStat> GetTopPlayersByPoints(int count)
        {
            return context.UserStats.OrderByDescending(it => it.Value).Take(count);
        }

        public IEnumerable<Dal.UserStat> GetTopPlayersByHours(int count)
        {
            return context.UserStats.OrderByDescending(it => it.PlayTime).Take(count);
        }
    }
}
