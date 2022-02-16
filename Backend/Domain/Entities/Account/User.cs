using System;
using System.Collections.Generic;
using Domain.Entities.Purchase;

namespace Domain.Entities.Account
{
    public class User
    {
        public int Id { get; set; }
        public string Auth64Id { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public string State { get; set; }
        
        public List<RefreshToken> RefreshTokens { get; set; }

        public DateTime LastChanged { get; set; }
    }
}
