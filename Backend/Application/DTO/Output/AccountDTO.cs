using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.Account;
using Domain.Entities.Privilege;
using Domain.Entities.UserStat;

namespace Application.DTO
{
    public class AccountDTO
    {
        public bool IsAuthenticated { get; set; }
        
        public string Name { get; set; }
            
        public string AvatarSource { get; set; }
        
        public string SteamId { get; set; }
        
        public Role Role { get; set; }
        
        public UserStat UserStat { get; set; }
        
        public Privilege Privilege { get; set; }
    }
}
