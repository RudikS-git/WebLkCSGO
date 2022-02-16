using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Domain.Entities.Account;
using Domain.Entities.Support;

namespace Application.DTO
{
    public class TicketDetailDTO
    {
        public int Id { get; set; }

        public string SenderUserStatId { get; set; } // not null
        public string SenderUserName { get; set; }

        public string AccusedUserStatId { get; set; } // not null
        public string AccusedUserName { get; set; }

        public int? CheckingUserId { get; set; } // support, admin

        public string? CheckingUserStatId { get; set; } // support, admin
        public User CheckingUser { get; set; }
        public string CheckingUserName { get; set; }

        public State State { get; set; } // решено, не решено, в ожидании, закрыт       

        public DateTime DateCreation { get; set; }
        public string ReportMessage { get; set; } // set restriction max ... symbols & min ... attr

        public int ServerId { get; set; }
        public ServerDTO Server { get; set; } // сервер, на котором был подан репорт

        public string Answer { get; set; }

        public DateTime DateAnswer { get; set; }
    }
}
