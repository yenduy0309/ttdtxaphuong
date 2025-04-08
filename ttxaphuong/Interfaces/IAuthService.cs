using ttxaphuong.DTO.Accounts;
using ttxaphuong.DTO.News_events;
using ttxaphuong.Models.Accounts;

namespace ttxaphuong.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponseModel> LoginAsync(LoginModel loginModel);
        Task<object> RegisterAsync(RegisterModel registerModel);
        Task<object> ForgotPasswordAsync(ForgotPasswordModel forgotPasswordModel);
        Task<object> VerifyResetCodeAsync(VerifyResetCodeModel model);
        Task<TokenResponseModel> RefreshTokenAsync(RefreshTokenModel refreshTokenModel);
        Task<object> DisableAccountAsync(int accountId);
        Task<IEnumerable<AccountsDTO>> GetAllAccountsAsync();
        Task<AccountsDTO> GetAccountsByIdAsync(int id);
        Task<AccountsDTO> UpdateAccountAsync(int id, AccountsDTO accountsDTO);
        Task<object> AssignManagerPermissionsAsync(AssignPermissionsModel model);
        Task<object> AssignAdminRoleAsync(int accountId);
        Task<AssignPermissionsModel> GetManagerPermissionsAsync(int managerId);

        Task<object> UpdatePasswordAsync(UpdatePasswordModel updatePasswordModel);

        // 🔥 Thêm phương thức xóa tài khoản
        Task<object> DeleteAccountAsync(int accountId);
    }
}
