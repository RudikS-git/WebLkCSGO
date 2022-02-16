using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain
{
    [Table("chatlog")]
    public class ChatRow
    {
        [Column("msg_id")]
        public int Id { get; set; }

        [Column("server_id")]
        public int ServerId { get; set; }

        [Column("auth")]
        public string SteamAuth2 { get; set; }

        [Column("ip")]
        public string Ip { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("team")]
        public int Team { get; set; }

        [Column("alive")]
        public int Alive { get; set; }

        [Column("timestamp")]
        public int Timestamp { get; set; }

        [Column("message")]
        public string Message { get; set; }

        [Column("type")]
        public string Type { get; set; }

    }
}
