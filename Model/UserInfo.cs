namespace ParentBookingAPI.Model
{
    public class UserInfo
    {
        public int UserID { get; set; }
        public string Username { get; set; } = string.Empty;

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string RefreshToken { get; set; } 
        public DateTime? TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
        public string    Role { get; set; }
    }
}
