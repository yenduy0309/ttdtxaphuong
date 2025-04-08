namespace ttxaphuong.Models.Accounts
{
    public class UpdatePasswordModel
    {
        public int Id_account { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}
