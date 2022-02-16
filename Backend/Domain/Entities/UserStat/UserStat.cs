using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.UserStat
{
    [Table("lvl_base")]
    public class UserStat
    {
        [Column("Id")]
        public int Id { get; set; }

        [Column("steam")]
        public string SteamAuth2 { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("value")]
        public int Value { get; set; }

        [Column("rank")]
        public int Lvl { get; set; }

        [Column("kills")]
        public int Kills { get; set; }

        [Column("deaths")]
        public int Deaths { get; set; }

        [Column("shoots")]
        public int Shoots { get; set; }

        [Column("hits")]
        public int Hits { get; set; }

        [Column("headshots")]
        public int Headshots { get; set; }

        [Column("assists")]
        public int Assists { get; set; }

        [Column("round_win")]
        public int RoundWins { get; set; }

        [Column("round_lose")]
        public int RoundLosses { get; set; }

        [Column("playtime")]
        public long PlayTime { get; set; }

        [Column("lastconnect")]
        public long LastConnection { get; set; }
    }
}
