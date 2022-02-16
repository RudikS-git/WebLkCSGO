namespace Domain.Entities.Punishments
{
    public class CommInfo : PunishmentInfo
    {
        public byte Type { get; set; } // 1 - chat, 2 - voice, 3 - both
    }
}
