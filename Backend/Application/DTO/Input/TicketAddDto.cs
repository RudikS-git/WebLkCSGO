using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.Input
{
    public class TicketAddDto
    {
        public string senderUserAuthId { get; set; }
        public string accusedUserAuthId { get; set; }
        public string reportMessage { get; set; }
    }
}
