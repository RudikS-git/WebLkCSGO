using System;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RconSharp;
using Infrastructure.Services;

namespace Infrastructure.Services
{
    public class Rcon : IRcon, IDisposable
    {
        private readonly string _ip;
        private readonly int _port;
        private readonly string _pass;
        private readonly RconClient _rconClient;
        
        public Rcon(string ip, int port, string pass)
        {
            _ip = ip;
            _port = port;
            _pass = pass;
            _rconClient = RconClient.Create(_ip, _port);
        }

        public async Task Connect()
        {
            await _rconClient.ConnectAsync();
            var authenticated = await _rconClient.AuthenticateAsync(_pass);

            if (!authenticated)
                throw new Exception("Не удалось авторизоваться на сервере");
        }

        public async Task RefreshVips()
        {
            await _rconClient.ExecuteCommandAsync("sm_refresh_vips");
        }

        public async Task NotifyPlayer(string steamId2, string message)
        {
            await _rconClient.ExecuteCommandAsync($"sm_reportnotifyplayermessage \"{message}\" \"{steamId2}\"");
        }
        
        public async Task<string> Status()
        {
            return await _rconClient.ExecuteCommandAsync($"status");
        }

        public void Dispose()
        {
            _rconClient.Disconnect();
        }
    }
}
