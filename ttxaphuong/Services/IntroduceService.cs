using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Text;
using ttxaphuong.Data;
using ttxaphuong.DTO.Introduces;
using ttxaphuong.DTO.News_events;
using ttxaphuong.Interfaces;
using ttxaphuong.Models.Introduce;
using ttxaphuong.Models.News_events;
using WebDoAn2.Exceptions;

namespace ttxaphuong.Services
{
    public class IntroduceService : IIntroduceService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public IntroduceService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<IntroduceDTO>> GetAllIntroduceAsync()
        {
            try
            {
                var services = await _context.Introduces
                              .Include(h => h.Categories_IntroduceModel) 
                              .ToListAsync();
                return _mapper.Map<IEnumerable<IntroduceDTO>>(services);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy tất cả giới thiệu", ex);
            }
        }

        public async Task<IntroduceDTO> GetIntroduceByIdAsync(int id)
        {
            try
            {
                var abc = await _context.Introduces.FirstOrDefaultAsync(b => b.Id_introduce == id);
                return abc == null ? throw new NotFoundException("Không tìm thấy giới thiệu") : _mapper.Map<IntroduceDTO>(abc);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy thông tin giới thiệu", ex);
            }
        }

        public async Task<IntroduceDTO> CreateIntroduceAsync(IntroduceDTO abc)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(abc.Name_introduce))
                    throw new BadRequestException("Tên giới thiệu không được để trống.");

                var news_eventsModel = _mapper.Map<IntroduceModel>(abc);

                if (!string.IsNullOrEmpty(abc.Image_url))
                {
                    news_eventsModel.Image_url = abc.Image_url; // Lưu đường dẫn hình ảnh
                }

                _context.Introduces.Add(news_eventsModel);
                await _context.SaveChangesAsync();

                news_eventsModel.Id_introduce = news_eventsModel.Id_introduce;

                return _mapper.Map<IntroduceDTO>(news_eventsModel);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi tạo thông tin mới: " + ex.Message, ex);
            }
        }

        public async Task<IntroduceDTO> UpdateIntroduceAsync(int id, IntroduceDTO introduce)
        {
            try
            {
                var abc = await _context.Introduces.FindAsync(id)
                    ?? throw new NotFoundException("Không tìm thấy giới thiệu");

                if (string.IsNullOrWhiteSpace(introduce.Name_introduce))
                    throw new BadRequestException("Tên giới thiệu không được để trống.");

                _mapper.Map(introduce, abc);
                abc.Id_introduce = id; // Đảm bảo Id_introduce không thay đổi

                await _context.SaveChangesAsync();

                return _mapper.Map<IntroduceDTO>(abc);
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
                throw new Exception("Có lỗi xảy ra khi cập nhật giới thiệu: " + ex.Message, ex);
            }
        }

        public async Task<object> DeleteIntroduceAsync(int id)
        {
            try
            {
                var transports = await _context.Introduces.FindAsync(id)
                    ?? throw new NotFoundException("Không tìm thấy giới thiệu");

                _context.Introduces.Remove(transports);
                await _context.SaveChangesAsync();

                return new { message = "Xóa giới thiệu thành công!" };
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi xóa giới thiệu " + ex.Message, ex);
            }
        }


        public async Task<string> UploadImageIntroduceAsync(IFormFile imageFile)
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

        /*************************************/
        //lấy bài giới thiệu = name danh mục
        public async Task<List<IntroduceDTO>> GetIntroByNameCategogyAsync(string nameCatelogy)
        {
            try
            {
                var Introduce = await _context.Introduces
                    .Include(h => h.Categories_IntroduceModel) // Đảm bảo có Include
                    .Where(h => h.Categories_IntroduceModel != null && h.Categories_IntroduceModel.Name_cate_introduce == nameCatelogy)
                    .ToListAsync();

                if (!Introduce.Any())
                {
                    throw new NotFoundException("Không tìm thấy bài giới thiệu.");
                }

                return _mapper.Map<List<IntroduceDTO>>(Introduce);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin", ex);
            }
        }

        public async Task<IntroduceDTO> GetIntroByNameAsync(string name)
        {
            try
            {
                // Chuẩn hóa tên tài liệu từ URL
                string normalizedInput = NormalizeTitle(name);

                // Lấy tất cả các tài liệu và sau đó lọc bên ngoài cơ sở dữ liệu
                var intro = await _context.Introduces.ToListAsync();

                var Introduce = intro.FirstOrDefault(h => NormalizeTitle(h.Name_introduce) == normalizedInput);

                if (Introduce == null)
                {
                    throw new NotFoundException("Không tìm thấy bài");
                }
                return _mapper.Map<IntroduceDTO>(Introduce);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin", ex);
            }
        }

        // Hàm chuẩn hóa tiêu đề (chuyển về lowercase, thay ký tự đặc biệt)
        private string NormalizeTitle(string title)
        {
            if (string.IsNullOrEmpty(title)) return "";

            // Bước 1: Chuyển thành chữ thường
            title = title.ToLower();

            // Bước 2: Chuẩn hóa Unicode (loại bỏ dấu)
            title = title.Normalize(NormalizationForm.FormD);
            title = Regex.Replace(title, @"\p{M}", ""); // Loại bỏ dấu tiếng Việt

            // Bước 3: Thay thế ký tự đặc biệt
            title = title.Replace("đ", "d").Replace("Đ", "D") // Thay thế Đ, đ
                         .Replace("/", "-") // Thay dấu "/" thành "-"
                         .Replace(",", "-") // Thay dấu "," thành "-"
                         .Replace(".", "-") // Thay dấu "." thành "-"
                         .Replace("(", "").Replace(")", ""); // Loại bỏ dấu "(" và ")"

            // Bước 4: Xóa ký tự đặc biệt, chỉ giữ lại chữ, số và gạch ngang
            title = Regex.Replace(title, @"[^a-z0-9-]", "-");

            // Bước 5: Loại bỏ dấu "-" thừa
            title = Regex.Replace(title, @"-+", "-").Trim('-');

            return title;
        }

        //Tin liên quan trong dm
        public async Task<List<IntroduceDTO>> GetRelatedIntroAsync(int id)
        {
            try
            {
                // Tìm bài gth hiện tại
                var currentIntroduce = await _context.Introduces.FindAsync(id);
                if (currentIntroduce == null)
                {
                    return new List<IntroduceDTO>(); // Trả về danh sách rỗng
                }

                // Lấy danh sách bài viết liên quan cùng danh mục
                var relatedIntroduce = await _context.Introduces
                    .Where(h => h.Id_cate_introduce == currentIntroduce.Id_cate_introduce && h.Id_introduce != id)
                    .ToListAsync();

                // Nếu không có bài viết liên quan, ném ra ngoại lệ
                return relatedIntroduce.Any() ? _mapper.Map<List<IntroduceDTO>>(relatedIntroduce) : new List<IntroduceDTO>();
            }
            catch (Exception ex)
            {
                return new List<IntroduceDTO>();
            }
        }
    }
}

