using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;
using Domain.Context;
using DAL = Domain.Entities.Server;
using Application;
using Domain.Contexts;
using Okolni.Source.Query;
using Domain.Entities;
using Okolni.Source.Query.Responses;

namespace Infrastructure.Services
{
    public class ServerAdmin : IServerService
    {
        private readonly DataContext context;

        public ServerAdmin(DataContext serverContext)
        {
            context = serverContext;
        }

        public async Task<List<DAL.Server>> GetServersAsync()
        {
            /*using (MySqlConnection mySqlConnection = GetConnection())
            {
                mySqlConnection.Open();
                MySqlCommand command = new MySqlCommand($"SELECT `ip`, `port` FROM `sb_servers`",
                                                         mySqlConnection);

                List<Server> servers = new List<Server>();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        servers.Add(new Server(reader[0].ToString(), int.Parse(reader[1].ToString())));
                    }
                }

                return servers;
            }*/
            
            return await context.ServerInfo.ToListAsync();
        }

        public async Task<ServersInfo> GetServersInfoAsync()
        {
            var servers = await GetServersAsync();
            
            if (servers == null)
            {
                return null;
            }

            ServersInfo serversInfo = new ServersInfo();
            ServerInfo serverInfo;
            Okolni.Source.Query.Responses.PlayerResponse playerResponse;

            for (int i = 0; i < servers.Count; i++)
            {
                IQueryConnection queryConnection = null;
                try
                {
                    queryConnection = new QueryConnection();
                    queryConnection.Host = servers[i].Ip;
                    queryConnection.Port = servers[i].Port;
                    queryConnection.Connect();
 
                    var data = queryConnection.GetInfo();
                    playerResponse = queryConnection.GetPlayers();

                    serversInfo.Servers.Add(new ServerInfo()
                    {
                        Bots = data.Bots,
                        Duration = data.Duration,
                        Ip = servers[i].Ip,
                        Map = data.Map,
                        MaxPlayers = data.MaxPlayers,
                        Name = data.Name,
                        Players = data.Players,
                        Port = (short)servers[i].Port,
                        VAC = data.VAC,
                        PlayersList = playerResponse
                    });

                    serversInfo.players += data.Players;
                    serversInfo.slots += data.MaxPlayers;
                }
                catch
                {
                    serversInfo.Servers.Add(new ServerInfo()
                    {
                        Ip = servers[i].Ip,
                        Name = $"Не удалось получить название сервера({servers[i].Ip}:{servers[i].Port})",
                        PlayersList = null
                    });

                }
                finally
                {
                    if (queryConnection != null)
                    {
                       // queryConnection.Disconnect();
                    }
                }
                
            }

            return serversInfo;
        }

        public async Task<DAL.Server> AddServerAsync(DAL.Server server)
        {
            var searchServer = await context.ServerInfo.FindAsync(server.Id);

            if (searchServer == null)
            {
                await context.ServerInfo.AddAsync(server);
                await context.SaveChangesAsync();
            }
            else
            {
                return null;
            }


            return server;
        }

        public async Task<DAL.Server> DeleteServerAsync(int? id)
        {
            var server = await context.ServerInfo.FindAsync(id);

            if (server != null)
            {
                context.ServerInfo.Remove(server); 
                await context.SaveChangesAsync();
            }
            
            return server;
        }

        public async Task<DAL.Server> EditServerAsync(DAL.Server server)
        {
            var existingServer = await context.ServerInfo.FindAsync(server.Id);

            if(existingServer != null)
            {
                //   context.Entry(searchServer).State = EntityState.Detached;
                context.Entry(existingServer).CurrentValues.SetValues(server);
                await context.SaveChangesAsync();

                return server;
            }

            return null;

        }
    }
}
