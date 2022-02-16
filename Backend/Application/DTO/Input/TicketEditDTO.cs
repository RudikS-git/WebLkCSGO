using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.Support;

namespace Application.DTO
{
    public class TicketEditDTO
    {
        public string CheckingUserId { get; set; }
        public int TicketId { get; set; }
        public State State { get; set; }
        public string Answer { get; set; }
    }
}
