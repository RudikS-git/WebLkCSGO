using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTO;
using Domain.Entities.Account;
using Domain.Entities.Support;

namespace Application
{
    public interface ITicketService
    {
        public Task<TicketDTO> GetAsync(int id);
        public Task<int> AddAsync(string ipPort, string senderUserAuthId, string accusedUserAuthId, string reportMessage);
        public IEnumerable<TicketDTO> GetAsync(int page, int offset);
        public Task<TicketDTO> SetTicketStateAsync(TicketEditDTO ticketEdit);
        public IEnumerable<TicketDTO> GetTicketsHistory(int ticketId, string accusedUserStatId);
        public Task<int> GetCount();
    }
}
