using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Interfaces;
using Domain;
using Domain.Contexts;

namespace Infrastructure.Services
{
    public class ChatService : IChatService
    {
        private readonly DataContext _dataContext;
        
        public ChatService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        
        public IEnumerable<ChatRow> GetHistory(int serverId, int count, string steamId2)
        {
            return _dataContext.ChatRows.Where(it => it.ServerId == serverId && it.SteamAuth2 == steamId2)
                                        .OrderByDescending(it => it.Timestamp)
                                        .Take(count);
        }
    }
}
