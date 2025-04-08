using ttxaphuong.DTO;
using ttxaphuong.DTO.Introduces;
using ttxaphuong.Models.Accounts;

namespace ttxaphuong.Interfaces
{
    public interface IWebsiteSettingsService
    {
        Task<IEnumerable<WebsiteSettingsDTO>> GetWebsiteSettingsAsync();
        Task<WebsiteSettingsDTO> UpdateWebsiteSettingsAsync(WebsiteSettingsDTO settings);
        Task<string> UploadImageAsync(IFormFile imageFile);
    }
}
