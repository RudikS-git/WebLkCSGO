using System.Collections.Generic;
using Domain;

namespace Application.Interfaces
{
    public interface IChatService
    {
        public IEnumerable<ChatRow> GetHistory(int serverId, int count, string steamId2);
    }
}
