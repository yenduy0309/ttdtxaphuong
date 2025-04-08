namespace ttxaphuong.DTO.Accounts
{
    public class AccountsDTO
    {
        public int? Id_account { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Fullname { get; set; }
        public string? Role { get; set; } = "Manager";
        public string? Status { get; set; } = "Active";
        public DateTime Create_at { get; set; } = DateTime.UtcNow;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public string? VerificationCode { get; set; }
        public DateTime? CodeExpiry { get; set; }
    }
}
