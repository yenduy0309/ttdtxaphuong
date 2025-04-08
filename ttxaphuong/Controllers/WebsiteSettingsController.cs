using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ttxaphuong.DTO;
using ttxaphuong.DTO.Introduces;
using ttxaphuong.Interfaces;
using ttxaphuong.Models.Accounts;
using ttxaphuong.Services;

namespace ttxaphuong.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WebsiteSettingsController : ControllerBase
    {
        private readonly IWebsiteSettingsService _websiteSettingsService;

        public WebsiteSettingsController(IWebsiteSettingsService websiteSettingsService)
        {
            _websiteSettingsService = websiteSettingsService;
        }

        // Lấy dữ liệu duy nhất
        [HttpGet]
        public async Task<IActionResult> GetWebsiteSettings()
        {
            var settings = await _websiteSettingsService.GetWebsiteSettingsAsync();
            if (settings == null)
                return NotFound("Chưa có dữ liệu cài đặt");

            return Ok(settings);
        }

        // Cập nhật dữ liệu duy nhất
        [HttpPut]
        public async Task<IActionResult> UpdateWebsiteSettings([FromBody] WebsiteSettingsDTO settings)
        {
            if (settings == null)
                return BadRequest("Dữ liệu không hợp lệ");

            var updatedSettings = await _websiteSettingsService.UpdateWebsiteSettingsAsync(settings);
            return Ok(updatedSettings); // Trả về dữ liệu đã cập nhật
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile imageFile)
        {
            return Ok(new { ImagePath = await _websiteSettingsService.UploadImageAsync(imageFile) });
        }

    }
}
