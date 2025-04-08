namespace ttxaphuong.Models.Accounts
{
    public class TokenResponseModel
    {
        public int Id_account { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? Role { get; set; }
    }
}
