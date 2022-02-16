using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.Rank;

namespace Application.DTO
{
    public class ProfileDTO
    {
        public int Id { get; set; }
        
        public string Auth64 { get; set; }

        public string Name { get; set; }
        
        public int Value { get; set; }

        public int Lvl { get; set; }

        public int Kills { get; set; }

        public int Deaths { get; set; }

        public int Shoots { get; set; }

        public int Hits { get; set; }

        public int Headshots { get; set; }

        public int Assists { get; set; }

        public int RoundWins { get; set; }

        public int RoundLosses { get; set; }

        public long PlayTime { get; set; }

        public DateTime LastConnection { get; set; }
        
        public string AvatarSource { get; set; }

        public string GroupName { get; set; }
        
        public int PlaceTopOfTime { get; set; }
        
        public int PlaceTopOfPoints { get; set; }
        
        public Rank Rank { get; set; }

        public Rank NextRank { get; set; }
    }
}
