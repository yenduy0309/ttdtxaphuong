using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ttxaphuong.DTO.Accounts;
using ttxaphuong.DTO.News_events;
using ttxaphuong.Interfaces;
using ttxaphuong.Models.Accounts;
using ttxaphuong.Services;
using WebDoAn2.Exceptions;

namespace ttxaphuong.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> Login([FromBody] LoginModel authDto)
        {
            if (authDto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var response = await _authService.LoginAsync(authDto);
            return Ok(response);
        }

        [HttpPost]
        [AllowAnonymous] // Cho phép đăng ký mà không cần xác thực
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            if (registerModel == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }
            var response = await _authService.RegisterAsync(registerModel);
            return Ok(response);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<IEnumerable<AccountsDTO>>> GetAccounts()
        {
            return Ok(await _authService.GetAllAccountsAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        [AllowAnonymous] // Cho phép đăng ký mà không cần xác thực
        public async Task<ActionResult<AccountsDTO>> GetAccountsById(int id)
        {
            return Ok(await _authService.GetAccountsByIdAsync(id));
        }

        [HttpPut]
        [AllowAnonymous] // Cho phép đăng ký mà không cần xác thực
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel forgotPasswordModel)
        {
            if (forgotPasswordModel == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            return Ok(await _authService.ForgotPasswordAsync(forgotPasswordModel));
        }

        [HttpPut("VerifyResetCode")]
        [AllowAnonymous] // Cho phép sử dụng mà không cần đăng nhập
        public async Task<IActionResult> VerifyResetCode([FromBody] VerifyResetCodeModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Code) || string.IsNullOrEmpty(model.NewPassword))
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            try
            {
                var result = await _authService.VerifyResetCodeAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous] // Cho phép đăng ký mà không cần xác thực
        public async Task<IActionResult> RefreshToken(RefreshTokenModel refreshTokenModel)
        {
            if (refreshTokenModel == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }
            return Ok(await _authService.RefreshTokenAsync(refreshTokenModel));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DisableAccount(int id)
        {
            return Ok(await _authService.DisableAccountAsync(id));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateAccount(int id, [FromBody] AccountsDTO accountsDTO)
        {
            try
            {
                if (accountsDTO == null)
                    return BadRequest(new { message = "Dữ liệu cập nhật không hợp lệ." });

                var updatedAccount = await _authService.UpdateAccountAsync(id, accountsDTO);
                return Ok(updatedAccount);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật tài khoản.", error = ex.Message });
            }
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignManagerPermissions([FromBody] AssignPermissionsModel model)
        {
            return Ok(await _authService.AssignManagerPermissionsAsync(model));
        }

        [HttpGet("GetManagerPermissions/{managerId}")]
        [AllowAnonymous] // Cho phép đăng ký mà không cần xác thực
        public async Task<IActionResult> GetManagerPermissions(int managerId)
        {
            var response = await _authService.GetManagerPermissionsAsync(managerId);
            return Ok(response);
        }

        [HttpPost("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignAdminRole(int id)
        {
            return Ok(await _authService.AssignAdminRoleAsync(id));
        }

        [HttpPut]
        [AllowAnonymous] // Cho phép đăng ký mà không cần xác thực
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordModel updatePassword)
        {
            if (updatePassword == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            return Ok(await _authService.UpdatePasswordAsync(updatePassword));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        [AllowAnonymous] // Cho phép đăng ký mà không cần xác thực
        public async Task<IActionResult> DeleteAccount(int id)
        {
            try
            {
                var result = await _authService.DeleteAccountAsync(id);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra khi xóa tài khoản.", error = ex.Message });
            }
        }

    }
}

