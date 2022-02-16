namespace WebAPI.Signalar
{
    public class UserSession
    {
        public string ConnectionId { get; set; }
        
        public string Name { get; set; }
        public string AuthId { get; set; }
        public string Role { get; set; }
        public string AvatarSource { get; set; }
    }
}
