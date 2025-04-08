namespace ttxaphuong.Models.Accounts
{
    public class RegisterModel
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Fullname { get; set; }
        public string? Role { get; set; } = "Manager";
        public DateTime Create_at { get; set; } = DateTime.UtcNow;

    }
}
