using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Application.DTO
{
    public class GameChatDTO
    {
        public int Id { get; set; }

        public int ServerId { get; set; }

        public string SteamAuth2 { get; set; }

        public string Ip { get; set; }

        public string Name { get; set; }

        public int Team { get; set; }

        public int Alive { get; set; }

        public DateTime Time { get; set; }

        public string Message { get; set; }

        public string Type { get; set; }
    }
}
