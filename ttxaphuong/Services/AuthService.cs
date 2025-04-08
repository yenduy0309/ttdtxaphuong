using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Text;
using ttxaphuong.Data;
using ttxaphuong.Interfaces;
using WebDoAn2.Exceptions;
using ttxaphuong.Models.Accounts;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using InfrastWebDoAn2ucture.Exceptions;
using ttxaphuong.DTO.Accounts;
using System.Security.Cryptography;
using Castle.Core.Resource;
using ttxaphuong.DTO.News_events;

namespace ttxaphuong.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthService(ApplicationDbContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<TokenResponseModel> LoginAsync(LoginModel loginModel)
        {
            try
            {
                var user = await _context.Accounts.FirstOrDefaultAsync(u => u.Username == loginModel.Username);
                if (user == null || !VerifyPassword(loginModel.Password, user.Password))
                {
                    throw new BadRequestException("Tên đăng nhập hoặc mật khẩu không đúng.");
                }

                if (user == null)
                    throw new BadRequestException("Tên đăng nhập không tồn tại.");

                if (user.Status != "Active")
                    throw new BadRequestException("Tài khoản đã bị vô hiệu hóa.");

                // Cập nhật Refresh Token vào database
                var accessToken = GenerateAccessToken(user);
                var refreshToken = GenerateRefreshToken(user);

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(1);

                await _context.SaveChangesAsync();

                return new TokenResponseModel
                {
                    Id_account = user.Id_account,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    Role = user.Role,
                };
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình đăng nhập" + ex.Message, ex);
            }
        }

        public async Task<object> RegisterAsync(RegisterModel registerModel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(registerModel.Username) ||
                    string.IsNullOrWhiteSpace(registerModel.Password) ||
                    string.IsNullOrWhiteSpace(registerModel.Email))
                {
                    return new { error = "Vui lòng nhập đầy đủ thông tin." };
                }

                // Chuẩn hóa dữ liệu đầu vào
                registerModel.Username = registerModel.Username.Trim();
                registerModel.Email = registerModel.Email.Trim().ToLower();

                // Kiểm tra tài khoản đã tồn tại
                var existingAccount = await _context.Accounts
                    .AnyAsync(c => c.Username == registerModel.Username || c.Email == registerModel.Email);

                if (existingAccount)
                {
                    return new { error = "Tên đăng nhập hoặc email đã được sử dụng." };
                }

                // Mã hóa mật khẩu trước khi lưu vào database
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerModel.Password);

                var newUser = new AccountsModel
                {
                    Username = registerModel.Username,
                    Password = HashPassword(registerModel.Password), // Lưu mật khẩu đã mã hóa
                    Email = registerModel.Email,
                    Fullname = registerModel.Fullname,
                    Role = registerModel.Role,
                    //Role = "Manager",
                    Status = "Active"
                };

                _context.Accounts.Add(newUser);
                await _context.SaveChangesAsync();

                return new { message = "Tài khoản đã được tạo thành công." };
            }
            catch (Exception ex)
            {
                return new { error = "Có lỗi xảy ra trong quá trình đăng ký: " + ex.Message };
            }
        }
        public async Task<object> ForgotPasswordAsync(ForgotPasswordModel forgotPasswordModel)
        {
            var user = await _context.Accounts.FirstOrDefaultAsync(u => u.Email == forgotPasswordModel.Email);
            if (user == null)
                throw new Exception("Email không tồn tại.");

            var verificationCode = new Random().Next(10000, 99999).ToString("D5");
            user.VerificationCode = verificationCode;
            user.CodeExpiry = DateTime.UtcNow.AddMinutes(5);
            await _context.SaveChangesAsync();

            // Gửi email chứa mã xác thực
            string subject = "Mã xác nhận đặt lại mật khẩu";
            string body = $"Mã xác nhận của bạn là: <b>{verificationCode}</b>. Mã có hiệu lực trong 5 phút.";
            await SendEmailAsync(user.Email, subject, body);

            return new { message = "Mã xác nhận đã được gửi đến email." };
        }


        //public async Task<object> VerifyResetCodeAsync(VerifyResetCodeModel model)
        //{
        //    var user = await _context.Accounts.FirstOrDefaultAsync(u => u.Email == model.Email && u.VerificationCode == model.Code);
        //    if (user == null || user.CodeExpiry < DateTime.UtcNow)
        //        throw new Exception("Mã xác nhận không hợp lệ hoặc đã hết hạn.");

        //    user.Password = model.NewPassword;
        //    user.VerificationCode = null;
        //    user.CodeExpiry = null;
        //    await _context.SaveChangesAsync();

        //    return new { message = "Mật khẩu đã được cập nhật." };
        //}

        public async Task<object> VerifyResetCodeAsync(VerifyResetCodeModel model)
        {
            var user = await _context.Accounts.FirstOrDefaultAsync(u => u.Email == model.Email && u.VerificationCode == model.Code);
            if (user == null || user.CodeExpiry < DateTime.UtcNow)
                throw new Exception("Mã xác nhận không hợp lệ hoặc đã hết hạn.");

            // Hash mật khẩu mới trước khi lưu vào database
            user.Password = HashPassword(model.NewPassword);
            user.VerificationCode = null;
            user.CodeExpiry = null;
            await _context.SaveChangesAsync();

            return new { message = "Mật khẩu đã được cập nhật thành công." };
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient(_configuration["EmailSettings:Host"])
                {
                    Port = int.Parse(_configuration["EmailSettings:Port"]),
                    Credentials = new NetworkCredential(
                        _configuration["EmailSettings:Email"],
                        _configuration["EmailSettings:Password"]
                    ),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_configuration["EmailSettings:Email"], _configuration["EmailSettings:DisplayName"]),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(toEmail);

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (SmtpException smtpEx)
            {
                throw new Exception("Có lỗi xảy ra khi gửi email: " + smtpEx.Message, smtpEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi gửi email: " + ex.Message, ex);
            }
        }

        public async Task<TokenResponseModel> RefreshTokenAsync(RefreshTokenModel refreshTokenModel)
        {
            try
            {
                if (string.IsNullOrEmpty(refreshTokenModel.RefreshToken))
                {
                    throw new BadRequestException("Refresh Token không hợp lệ.");
                }
                var accounts = await _context.Accounts.FirstOrDefaultAsync(c => c.RefreshToken == refreshTokenModel.RefreshToken);

                if (accounts == null || accounts.RefreshTokenExpiry < DateTime.UtcNow)
                {
                    throw new BadRequestException("Refresh Token không hợp lệ hoặc đã hết hạn.");
                }

                var newAccessToken = GenerateAccessToken(accounts);

                return new TokenResponseModel
                {
                    Id_account = accounts.Id_account,
                    AccessToken = newAccessToken,
                    RefreshToken = refreshTokenModel.RefreshToken,
                    Role = accounts.Role
                };
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình cập nhật mật khẩu " + ex.Message, ex);
            }
        }

        public async Task<object> DisableAccountAsync(int accountId)
        {
            var user = await _context.Accounts.FindAsync(accountId);
            if (user == null)
                throw new Exception("Tài khoản không tồn tại.");

            user.Status = "Inactive";
            await _context.SaveChangesAsync();
            return new { message = "Tài khoản đã bị vô hiệu hóa." };
        }

        private string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) throw new ArgumentException("Mật khẩu không được để trống.");

            using (SHA256 sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hashBytes = sha256.ComputeHash(bytes);

                return Convert.ToBase64String(hashBytes);
            }
        }

        public async Task<IEnumerable<AccountsDTO>> GetAllAccountsAsync()
        {
            try
            {
                var accounts = await _context.Accounts.ToListAsync();
                return _mapper.Map<IEnumerable<AccountsDTO>>(accounts);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy danh sách tài khoản" + ex.Message, ex);
            }
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            string hashedInputPassword = HashPassword(password);
            return hashedInputPassword == hashedPassword;
        }

        public async Task<AccountsDTO> GetAccountsByIdAsync(int id)
        {
            try
            {
                var account = await _context.Accounts
                    .FirstOrDefaultAsync(c => c.Id_account == id && c.Status == "Active");

                if (account == null)
                    throw new UnauthorizedException("Tài khoản đã bị vô hiệu hóa hoặc không tồn tại!");

                return _mapper.Map<AccountsDTO>(account);
            }
            catch (UnauthorizedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy thông tin tài khoản: " + ex.Message, ex);
            }
        }


        public async Task<AccountsDTO> UpdateAccountAsync(int id, AccountsDTO accounts)
        {
            try
            {
                var user = await _context.Accounts.FindAsync(id)
                    ?? throw new NotFoundException("Không tìm thấy tài khoản");

                if (string.IsNullOrWhiteSpace(accounts.Username))
                    throw new BadRequestException("Tên đăng nhập không được để trống.");

                // ✅ Chỉ cập nhật 4 trường
                user.Username = accounts.Username;
                user.Fullname = accounts.Fullname;
                user.Email = accounts.Email;
                user.Role = accounts.Role;

                await _context.SaveChangesAsync();
                return _mapper.Map<AccountsDTO>(user);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật tài khoản người dùng", ex);
            }
        }


        public async Task<object> AssignManagerPermissionsAsync(AssignPermissionsModel model)
        {
            var permissions = await _context.Permissions.FirstOrDefaultAsync(p => p.ManagerId == model.ManagerId);
            if (permissions == null)
            {
                permissions = new PermissionsModel { ManagerId = model.ManagerId };
                _context.Permissions.Add(permissions);
            }

            permissions.CanAddUser = model.CanAddUser;
            permissions.CanEditUser = model.CanEditUser;
            permissions.CanDeleteUser = model.CanDeleteUser;
            permissions.CanManageRoles = model.CanManageRoles;
            permissions.CanManagePermissions = model.CanManagePermissions;
            await _context.SaveChangesAsync();
            return new { message = "Quyền của Người quản lý đã được cập nhật." };
        }

        public async Task<AssignPermissionsModel> GetManagerPermissionsAsync(int managerId)
        {
            var permissions = await _context.Permissions
                .Where(p => p.ManagerId == managerId)
                .FirstOrDefaultAsync();

            if (permissions == null)
            {
                return new AssignPermissionsModel(); // Trả về mặc định nếu không có quyền
            }

            return new AssignPermissionsModel
            {
                ManagerId = permissions.ManagerId,
                CanAddUser = permissions.CanAddUser,
                CanEditUser = permissions.CanEditUser,
                CanDeleteUser = permissions.CanDeleteUser,
                CanViewUsers = permissions.CanViewUsers,
                CanManageRoles = permissions.CanManageRoles,
                CanManagePermissions = permissions.CanManagePermissions
            };
        }


        public async Task<object> AssignAdminRoleAsync(int accountId)
        {
            var user = await _context.Accounts.FindAsync(accountId);
            if (user == null)
                throw new Exception("Tài khoản không tồn tại.");

            user.Role = "Admin";
            await _context.SaveChangesAsync();
            return new { message = "Tài khoản đã được nâng cấp thành Admin." };
        }

        private string GenerateAccessToken(AccountsModel user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id_account.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken(AccountsModel accounts)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, accounts.Id_account.ToString()),
                new Claim(ClaimTypes.Role, accounts.Role),
                new Claim(ClaimTypes.DateOfBirth, DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"))
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<object> UpdatePasswordAsync(UpdatePasswordModel updatePasswordModel)
        {
            try
            {
                var customer = await _context.Accounts
                    .FirstOrDefaultAsync(c => c.Id_account == updatePasswordModel.Id_account && c.Status == "Active")
                    ?? throw new NotFoundException("Không tìm thấy khách hàng");

                if (!VerifyPassword(updatePasswordModel.OldPassword, customer.Password))
                {
                    throw new BadRequestException("Mật khẩu cũ không chính xác.");
                }

                customer.Password = HashPassword(updatePasswordModel.NewPassword);
                await _context.SaveChangesAsync();

                return new { message = "Mật khẩu đã được thay đổi thành công!" };
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình cập nhật mật khẩu " + ex.Message, ex);
            }
        }

        public async Task<object> DeleteAccountAsync(int accountId)
        {
            var user = await _context.Accounts.FindAsync(accountId);
            if (user == null)
                throw new NotFoundException("Tài khoản không tồn tại.");

            if (user.Status != "Inactive") // Chỉ cho phép xóa nếu tài khoản đã bị vô hiệu hóa
                throw new BadRequestException("Chỉ có thể xóa tài khoản đã bị vô hiệu hóa.");

            _context.Accounts.Remove(user);
            await _context.SaveChangesAsync();

            return new { message = "Tài khoản đã bị xóa thành công." };
        }

    }
}

