using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Privilege
{
    [Table("vip_users")]
    public class Privilege
    {
        public int Id { get; set; }
        
        [Column("account_id")]
        public int AuthId32 { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("lastvisit")]
        public int LastVisit { get; set; }
        
        [Column("sid")]
        public int ServerId { get; set; }
        
        [Column("group")]
        public string GroupName { get; set; }
        
        [Column("expires")]
        public int Expires { get; set; }
    }
}
