using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ttxaphuong.Data;
using ttxaphuong.DTO;
using ttxaphuong.DTO.Introduces;
using ttxaphuong.Interfaces;
using ttxaphuong.Models.Accounts;
using WebDoAn2.Exceptions;

namespace ttxaphuong.Services
{
    public class WebsiteSettingsService : IWebsiteSettingsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public WebsiteSettingsService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WebsiteSettingsDTO>> GetWebsiteSettingsAsync()
        {
            try
            {
                var services = await _context.WebsiteSettings.ToListAsync();
                return _mapper.Map<IEnumerable<WebsiteSettingsDTO>>(services);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy setting", ex);
            }
        }

        public async Task<WebsiteSettingsDTO> UpdateWebsiteSettingsAsync(WebsiteSettingsDTO settingsDto)
        {
            try
            {
                var existingSetting = await _context.WebsiteSettings.FirstOrDefaultAsync();

                if (existingSetting == null)
                {
                    var newSetting = _mapper.Map<WebsiteSettingsModel>(settingsDto); // Chuyển từ DTO sang Model
                    _context.WebsiteSettings.Add(newSetting);
                    existingSetting = newSetting;
                }
                else
                {
                    _mapper.Map(settingsDto, existingSetting); // Cập nhật dữ liệu
                    _context.WebsiteSettings.Update(existingSetting);
                }

                await _context.SaveChangesAsync();
                return _mapper.Map<WebsiteSettingsDTO>(existingSetting); // Trả về DTO
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi cập nhật cài đặt website: " + ex.Message, ex);
            }
        }

        public async Task<string> UploadImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                throw new BadRequestException("Tệp hình ảnh không hợp lệ.");
            }

            string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }
            var imageName = imageFile.FileName.Replace(" ", "");
            var path = Path.Combine(uploadFolder, imageName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return imageName;
        }
    }
}
