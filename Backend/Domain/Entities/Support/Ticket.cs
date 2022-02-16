using System;
using System.ComponentModel.DataAnnotations;
using Domain.Entities.Account;

namespace Domain.Entities.Support
{
    public class Ticket
    {
        public int Id { get; set; }
        
        public int SenderUserStatId { get; set; } // not null
        public UserStat.UserStat SenderUserStat { get; set; }

        public int AccusedUserStatId { get; set; } // not null
        public UserStat.UserStat AccusedUserStat { get; set; }

        public int? CheckingUserId { get; set; } // support, admin
        public User CheckingUser { get; set; }

        public int? CheckingUserStatId { get; set; } // support, admin
        public UserStat.UserStat CheckingUserStat { get; set; }

        public State State { get; set; }

        public DateTime DateCreation { get; set; }
        public string ReportMessage { get; set; } // set restriction max ... symbols & min ... attr
        
        public int ServerId { get; set; } // not null!
        public Server.Server Server { get; set; } // сервер, на котором был подан репорт
        
        public string Answer { get; set; }

        public DateTime DateAnswer { get; set; }
    }

    public enum State
    {
        NotSolved,
        Pending,
        Closed
    }
}
