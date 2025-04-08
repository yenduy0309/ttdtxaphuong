using AutoMapper;
using System.Text.RegularExpressions;
using System.Text;
using ttxaphuong.Data;
using ttxaphuong.DTO.Uploads;
using ttxaphuong.Models.Loads;
using WebDoAn2.Exceptions;
using ttxaphuong.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ttxaphuong.Services
{
    public class UploadFilePdfService : IUploadFilePdfService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UploadFilePdfService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PostPdfDTO>> GetAllPostPdfAsync()
        {
            try
            {
                var postPdf = await _context.PostPdfs.ToListAsync();

                return _mapper.Map<IEnumerable<PostPdfDTO>>(postPdf);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy danh sách pdf: " + ex.Message, ex);
            }
        }

        public async Task<PostPdfDTO> MovePdfAsync(int pdfId, int targetFolderId)
        {
            var pdf = await _context.PostPdfs.FirstOrDefaultAsync(img => img.Id_Pdf == pdfId);
            if (pdf == null)
            {
                throw new Exception("Không tìm thấy pdf.");
            }

            var targetFolder = await _context.FolderPdfs.FirstOrDefaultAsync(f => f.Id_folder_pdf == targetFolderId);
            if (targetFolder == null)
            {
                throw new Exception("Không tìm thấy thư mục đích.");
            }

            pdf.Id_folder_pdf = targetFolderId;
            _context.PostPdfs.Update(pdf);
            await _context.SaveChangesAsync();

            return _mapper.Map<PostPdfDTO>(pdf);
        }

        public async Task<PostPdfDTO> UpdatePostPdfAsync(int id, PostPdfDTO postPdf)
        {
            try
            {
                var existingNewsEvent = await _context.PostPdfs.FindAsync(id)
                    ?? throw new NotFoundException("Không tìm thấy pdf");

                if (string.IsNullOrWhiteSpace(postPdf.FileName))
                    throw new BadRequestException("Tên pdf không được để trống.");

                existingNewsEvent.FileName = postPdf.FileName;
                existingNewsEvent.FilePath = postPdf.FilePath;
                existingNewsEvent.Id_folder_pdf = postPdf.Id_folder_pdf;

                await _context.SaveChangesAsync();
                return _mapper.Map<PostPdfDTO>(existingNewsEvent);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật pdf", ex);
            }
        }

        public async Task<object> DeletePostPdfAsync(int id)
        {
            try
            {
                var postPdf = await _context.PostPdfs.FindAsync(id)
                                ?? throw new NotFoundException("Không tìm thấy pdf");

                // Xóa file vật lý
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Pdf", postPdf.FilePath);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                // Xóa ảnh khỏi database
                _context.PostPdfs.Remove(postPdf);
                await _context.SaveChangesAsync();

                return new { message = "Xóa pdf thành công" };
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa pdf", ex);
            }
        }

        public async Task<PostPdfDTO> UploadPdfAsync(IFormFile file, int? folderId)
        {
            if (file == null || file.Length == 0)
            {
                throw new BadRequestException("Vui lòng chọn một tệp PDF.");
            }

            // Kiểm tra định dạng file PDF
            if (file.ContentType != "application/pdf" || Path.GetExtension(file.FileName).ToLower() != ".pdf")
            {
                throw new BadRequestException("Chỉ được phép upload file PDF.");
            }

            // Kiểm tra folderId và gán giá trị mặc định nếu cần
            int targetFolderId = folderId ?? 0; // Nếu folderId là null, gán giá trị mặc định là 0

            // Lấy thông tin thư mục từ cơ sở dữ liệu
            var folder = await _context.FolderPdfs
                .Include(f => f.Children) // Nếu cần lấy thông tin thư mục con
                .FirstOrDefaultAsync(f => f.Id_folder_pdf == targetFolderId);

            // Tạo đường dẫn thư mục phân cấp
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Pdf");
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
                    currentFolder = await _context.FolderPdfs
                        .FirstOrDefaultAsync(f => f.Id_folder_pdf == currentFolder.ParentId);
                }

                folderHierarchy.Reverse(); // Đảo ngược để có thứ tự từ thư mục gốc đến thư mục hiện tại
                folderPath = Path.Combine(uploadsFolder, Path.Combine(folderHierarchy.ToArray()));
            }

            // Tạo thư mục nếu chưa tồn tại
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Định dạng lại tên file (giữ nguyên phần mở rộng .pdf)
            string originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
            string fileExtension = Path.GetExtension(file.FileName);
            string formattedFileName = FormatFilePath(originalFileName) + fileExtension;

            // Lưu file vào thư mục phân cấp
            var fileName = Guid.NewGuid().ToString() + ".pdf";
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var postPdf = new PostPdfModel
            {
                FileName = file.FileName,
                FilePath = Path.Combine(folder != null ? Path.Combine(folderHierarchy.ToArray()) : "", fileName)
                    .Replace("\\", "/")
                    .Trim(),
                Id_folder_pdf = targetFolderId,
                UploadedAt = DateTime.Now
            };

            _context.PostPdfs.Add(postPdf);
            await _context.SaveChangesAsync();

            return _mapper.Map<PostPdfDTO>(postPdf);
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

