using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Application;
using Application.DTO;
using Application.DTO.Output;
using Microsoft.EntityFrameworkCore;
using SteamIDs_Engine;
using Infrastructure.SteamID;
using Domain.Context;
using Domain.Entities.Support;
using AutoMapper;
using AutoMapper.Configuration;
using Domain.Contexts;
using Ubiety.Dns.Core;
using Domain.Entities.UserStat;
using Domain.Entities.Account;
using Microsoft.Extensions.Configuration;
using AutoWrapper.Wrappers;

namespace Infrastructure.Services
{
    public class TicketService : ITicketService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IVkService _vkService;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

        public TicketService(DataContext context, 
                             IMapper mapper, 
                             IVkService vkService, 
                             Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _vkService = vkService;
            _configuration = configuration;
        }
        
        public async Task<int> AddAsync(string ipPort, string senderSteamAuth, string accusedSteamAuth, string reportMessage)
        {
            var serverInfo = ipPort.Split(":"); // 0 - ip, 1 - port
            int port = Int32.Parse(serverInfo[1]);

            var ticket = new Ticket();
            ticket.SenderUserStatId = (await _context.UserStats.Where(it => it.SteamAuth2 == SteamIDConvert.Steam64ToSteam2(long.Parse(senderSteamAuth)))
                                                            .FirstOrDefaultAsync()).Id;
            ticket.AccusedUserStatId = (await _context.UserStats.Where(it => it.SteamAuth2 == SteamIDConvert
                                                        .Steam64ToSteam2(long.Parse(accusedSteamAuth)))
                                                        .FirstOrDefaultAsync()).Id;
            
            
            ticket.ServerId = (await _context.ServerInfo.Where(it => it.Ip == serverInfo[0] && it.Port == port)
                                                        .FirstOrDefaultAsync())
                                                        .Id;
            
            ticket.DateCreation = DateTime.Now;
            ticket.DateAnswer = DateTime.Now;
            ticket.ReportMessage = reportMessage;

            await _context.AddAsync(ticket);
            await _context.SaveChangesAsync();

            return ticket.Id;
        }

        private async Task<Ticket> GetTicketAsync(int id)
        {
            var ticket = await _context.Tickets
                .Include(it => it.CheckingUser)
                .Include(it => it.Server)
                .Include(it => it.AccusedUserStat)
                .Include(it => it.SenderUserStat)
                .Include(it => it.CheckingUserStat)
                .Where(it => it.Id == id)
                .FirstOrDefaultAsync();

            return ticket;
        }
        
        public async Task<TicketDTO> GetAsync(int id)
        {
            var ticket = await GetTicketAsync(id);
            var ticketsDTO = _mapper.Map<TicketDTO>(ticket);

            return ticketsDTO;
        }


        public async Task<int> GetCount()
        {
            return await _context.Tickets.CountAsync();
        }

        public IEnumerable<TicketDTO> GetAsync(int page, int offset)
        {
            IEnumerable<Ticket> tickets = _context.Tickets
                //.Include(it => it.CheckingUser)
                .Include(it => it.Server)
                .Include(it => it.AccusedUserStat)
                .Include(it => it.SenderUserStat)
                .Include(it => it.CheckingUserStat)
                .AsNoTracking()
                .OrderByDescending(it => it.DateCreation)
                .Skip(page * offset)
                .Take(offset);
            
            var ticketsDTO = _mapper.Map<IEnumerable<TicketDTO>>(tickets);

            /* IQueryable<UserStat> userStats = _context.UserStats;
             
             foreach (var it in ticketsDTO)
             {
                 userStats = userStats
                     .Where(userStat => userStat.Id == it.SenderUserAuthId || userStat.Id == it.AccusedUserAuthId);
                             //.AsNoTracking();
 
                 var sender = await _statContext.UserStats.Where(userStat => userStat.Id == it.SenderUserAuthId)
                                                                  .AsNoTracking()
                                                                  .FirstOrDefaultAsync();
 
                 var accused = await _statContext.UserStats.Where(userStat => userStat.Id == it.AccusedUserAuthId)
                                                                  .AsNoTracking()
                                                                  .FirstOrDefaultAsync();
 
 
                 it.SenderUserName = userStats.First().Name;
                 it.AccusedUserName = userStats.Last().Name;
                     CheckingUserName = (await _statContext.UserStats.Where(userStat => userStat.Id == SteamIDConvert.Steam64ToSteam2(long.Parse(it.CheckingUser?.Auth64Id))).FirstOrDefaultAsync()).Name
             }
                 */
        
         //   List<UserStat> list = userStats.AsNoTracking().ToList();


           // foreach (var it in ticketsDTO)
           //{
            //    it.AccusedUserName = tickets
            //    it.SenderUserName
           // }


            return ticketsDTO;

        }
        
        

        public IEnumerable<TicketDTO> GetTicketsHistory(int ticketId, string accusedUserStatId)
        {
            string steamId = SteamIDs_Engine.SteamIDConvert.Steam64ToSteam2(long.Parse(accusedUserStatId));
            IEnumerable <Ticket> tickets = _context.Tickets
                                                            .Include(it => it.SenderUserStat)
                                                            .Where(it => it.AccusedUserStat.SteamAuth2 == steamId && it.Id != ticketId)
                                                            .OrderByDescending(it => it.DateCreation);

            return _mapper.Map<IEnumerable<TicketDTO>>(tickets);
        }
        
        public async Task<TicketDTO> SetTicketStateAsync(TicketEditDTO ticketEdit)
        {
            var ticket = await GetTicketAsync(ticketEdit.TicketId);

            if (ticket != null)
            {
                var checkingUser = await _context.User.Where(it => it.Auth64Id == ticketEdit.CheckingUserId).FirstOrDefaultAsync();

                if (checkingUser != null)
                {
                    var steamId = SteamIDConvert.Steam64ToSteam2(long.Parse(checkingUser.Auth64Id));

                    if (steamId == ticket.AccusedUserStat.SteamAuth2)
                    {
                        throw new ApiException("Невозможно поменять состояние репорта на самого себя");
                    }

                    if(ticket.State == ticketEdit.State) // попытка поставить уже существующий стейт
                    {
                        return null;
                    }

                    ticket.CheckingUser = checkingUser;
                    ticket.State = ticketEdit.State;
                    ticket.Answer = ticketEdit.Answer;

                    var userStat = await _context.UserStats.Where(it => it.SteamAuth2 == steamId).FirstOrDefaultAsync();
                   
                    ticket.CheckingUserStat = userStat;

                    await _context.SaveChangesAsync();

                    if (userStat != null)
                    {
                        Rcon rcon = new Rcon(ticket.Server.Ip, ticket.Server.Port, _configuration.GetValue<string>("Rcon"));
                        await rcon.Connect();

                        switch (ticketEdit.State)
                        {
                            case State.Closed:
                            {
                                _vkService.SendMessage($"[Сервер]\n{userStat.Name}({checkingUser.Auth64Id}) закрыл тикет №{ticket.Id} с причиной \"{ticket.Answer}\"");
                                await rcon.NotifyPlayer(ticket.SenderUserStat.SteamAuth2, @$"Администратор {userStat.Name} закрыл ваш репорт. Причина: {ticket.Answer}");
                                break;
                            }
                            
                            case State.Pending:
                            {
                                _vkService.SendMessage($"[Сервер]\n{userStat.Name}({checkingUser.Auth64Id}) взял тикет №{ticket.Id}");
                                await rcon.NotifyPlayer(ticket.SenderUserStat.SteamAuth2, $"Администратор {userStat.Name} принял ваш репорт.");
                                break;
                            }
                        }
                    }


                    return _mapper.Map<TicketDTO>(ticket);
                }
            }

            return null;
        }
    }
}
