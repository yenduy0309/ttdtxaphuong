using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.RegularExpressions;
using ttxaphuong.Data;
using ttxaphuong.DTO.Uploads;
using ttxaphuong.Interfaces;
using ttxaphuong.Models.Loads;
using WebDoAn2.Exceptions;

namespace ttxaphuong.Services
{
    public class UploadFileService : IUploadFileService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UploadFileService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PostImageDTO>> GetAllPostImageAsync()
        {
            try
            {
                var postImage = await _context.PostImages.ToListAsync();

                return _mapper.Map<IEnumerable<PostImageDTO>>(postImage);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy danh sách hình ảnh: " + ex.Message, ex);
            }
        }

        public async Task<PostImageDTO> MoveImageAsync(int imageId, int targetFolderId)
        {
            var image = await _context.PostImages.FirstOrDefaultAsync(img => img.Id_Image == imageId);
            if (image == null)
            {
                throw new Exception("Không tìm thấy hình ảnh.");
            }

            var targetFolder = await _context.Folders.FirstOrDefaultAsync(f => f.Id_folder == targetFolderId);
            if (targetFolder == null)
            {
                throw new Exception("Không tìm thấy thư mục đích.");
            }

            image.Id_folder = targetFolderId;
            _context.PostImages.Update(image);
            await _context.SaveChangesAsync();

            return _mapper.Map<PostImageDTO>(image);
        }

        public async Task<PostImageDTO> UpdatePostImageAsync(int id, PostImageDTO postImage)
        {
            try
            {
                var existingNewsEvent = await _context.PostImages.FindAsync(id)
                    ?? throw new NotFoundException("Không tìm thấy hình ảnh");

                if (string.IsNullOrWhiteSpace(postImage.FileName))
                    throw new BadRequestException("Tên hình ảnh không được để trống.");

                existingNewsEvent.FileName = postImage.FileName;
                existingNewsEvent.FilePath = postImage.FilePath;
                existingNewsEvent.Id_folder = postImage.Id_folder;

                await _context.SaveChangesAsync();
                return _mapper.Map<PostImageDTO>(existingNewsEvent);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật hình ảnh", ex);
            }
        }

        public async Task<object> DeletePostImageAsync(int id)
        {
            try
            {
                var postImage = await _context.PostImages.FindAsync(id)
                                ?? throw new NotFoundException("Không tìm thấy hình ảnh");

                // Xóa file vật lý
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", postImage.FilePath);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                // Xóa ảnh khỏi database
                _context.PostImages.Remove(postImage);
                await _context.SaveChangesAsync();

                return new { message = "Xóa hình ảnh thành công" };
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa hình ảnh", ex);
            }
        }

        public async Task<PostImageDTO> UploadImageAsync(IFormFile file, int? folderId)
        {
            if (file == null || file.Length == 0)
            {
                throw new BadRequestException("Vui lòng chọn một tệp ảnh.");
            }

            // Kiểm tra folderId và gán giá trị mặc định nếu cần
            int targetFolderId = folderId ?? 0; // Nếu folderId là null, gán giá trị mặc định là 0

            // Lấy thông tin thư mục từ cơ sở dữ liệu
            var folder = await _context.Folders
                .Include(f => f.Children) // Nếu cần lấy thông tin thư mục con
                .FirstOrDefaultAsync(f => f.Id_folder == targetFolderId);

            // Tạo đường dẫn thư mục phân cấp
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            var folderPath = uploadsFolder;

            // Khai báo và khởi tạo folderHierarchy bên ngoài khối if
            var folderHierarchy = new List<string>();

            if (folder != null)
            {
                // Tạo đường dẫn thư mục dựa trên cấu trúc phân cấp
                var currentFolder = folder;

                while (currentFolder != null)
                {
                    folderHierarchy.Add(currentFolder.Name_folder);
                    currentFolder = await _context.Folders
                        .FirstOrDefaultAsync(f => f.Id_folder == currentFolder.ParentId);
                }

                folderHierarchy.Reverse(); // Đảo ngược để có thứ tự từ thư mục gốc đến thư mục hiện tại
                folderPath = Path.Combine(uploadsFolder, Path.Combine(folderHierarchy.ToArray()));
            }

            // Tạo thư mục nếu chưa tồn tại
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Format tên file (giữ lại phần mở rộng)
            string originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
            string fileExtension = Path.GetExtension(file.FileName);
            string formattedFileName = FormatFilePath(originalFileName) + fileExtension;

            // Lưu file vào thư mục phân cấp
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var postImage = new PostImageModel
            {
                FileName = file.FileName,
                FilePath = Path.Combine(folder != null ? Path.Combine(folderHierarchy.ToArray()) : "", fileName)
                    .Replace("\\", "/")
                    .Trim(),
                Id_folder = targetFolderId,
                UploadedAt = DateTime.Now
            };

            _context.PostImages.Add(postImage);
            await _context.SaveChangesAsync();

            return _mapper.Map<PostImageDTO>(postImage);
        }

        public string FormatFilePath(string name)
        {
            if (string.IsNullOrEmpty(name)) return "";

            string formatted = name
                .Trim()
                .ToLower()
                .Normalize(NormalizationForm.FormD);

            // Xóa dấu tiếng Việt bằng Regex
            formatted = Regex.Replace(formatted, @"[\u0300-\u036f]", "");

            // Thay thế "đ" và "Đ"
            formatted = formatted.Replace("đ", "d").Replace("Đ", "d");

            // Thay thế ký tự đặc biệt bằng "-"
            formatted = Regex.Replace(formatted, @"[/,().]", "-");

            // Chỉ giữ chữ thường, số, và "-"
            formatted = Regex.Replace(formatted, @"[^a-z0-9-]", "-");

            // Loại bỏ dấu "-" liên tiếp
            formatted = Regex.Replace(formatted, @"-+", "-");

            // Xóa "-" ở đầu và cuối
            return formatted.Trim('-');
        }
    }
}
