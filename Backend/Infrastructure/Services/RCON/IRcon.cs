using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IRcon
    {
        public Task NotifyPlayer(string steamId2, string message);
    }
}
 