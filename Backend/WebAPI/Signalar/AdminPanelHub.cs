using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application;
using Domain.Entities.Account;
using Infrastructure.SteamID;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebAPI.Signalar
{
    [Authorize(AuthenticationSchemes = "User")]
    [Authorize(Policy = "TicketManage")]
    public class AdminPanelHub : Hub
    {
        static public List<UserSession> ConnectedUsers = new List<UserSession>();
        private readonly IAccountService _accountService;

        public AdminPanelHub(IAccountService accountService)
        {
            _accountService = accountService;
        }
        
        public async override Task OnConnectedAsync()
        {
            var id = Context.ConnectionId;
            var auth = Context.User.Claims.FirstOrDefault(it => it.Type == ClaimTypes.NameIdentifier).Value;

            if (ConnectedUsers.Count(x => x.ConnectionId == id || x.AuthId == auth) == 0)
            {               
                var user = await _accountService.GetUser(auth);
                var userInfo = await SteamAPI.GetUserInfoAsync(user.Auth64Id);

                ConnectedUsers.Add(new UserSession
                {
                    ConnectionId = id,
                    
                    Name = userInfo[0].Name,
                    Role = user.Role.Name,
                    AuthId = user.Auth64Id,
                    AvatarSource = userInfo[0].Avatar
                });
            }

            await Clients.All.SendAsync("UpdateActiveConnections");
            
            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var item = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                ConnectedUsers.Remove(item);
            }

            Clients.All.SendAsync("UpdateActiveConnections");

            return base.OnDisconnectedAsync(exception);
        }

        public List<UserSession> GetAllActiveConnections()
        {
            return ConnectedUsers;
        }
    }
}
