namespace Domain.Entities.Punishments
{
    public class PunishmentInfo
    {
        public int Id { get; set; }

        public string AuthId { get; set; }
        public string AuthId64 { get; set; }

        public string Name { get; set; }

        public string Created { get; set; }

        public string Ends { get; set; }

        public string Reason { get; set; }

        public string RemoveType { get; set; } // если не null, то разбанен

        public string Avatar { get; set; }

        public string ServerName { get; set; }
        
        public string AdminName { get; set; }
        public string AdminAuthId { get; set; }
        public string AdminAvatar { get; set; }
    }
}
