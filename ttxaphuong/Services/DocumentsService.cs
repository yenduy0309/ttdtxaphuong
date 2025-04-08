using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Text;
using ttxaphuong.Data;
using ttxaphuong.DTO.Documents;
using ttxaphuong.Interfaces;
using ttxaphuong.Models.Documents;
using WebDoAn2.Exceptions;
using ttxaphuong.DTO.News_events;

namespace ttxaphuong.Services
{
    public class DocumentsService : IDocumentsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DocumentsService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<DocumentsDTO>> GetAllDocumentsAsync(bool? isVisible = null)
        {
            var query = _context.Documents.AsQueryable();

            if (isVisible.HasValue)
            {
                query = query.Where(n => n.IsVisible == isVisible.Value);
            }

            var documents = await query
                .Include(h => h.Category_Documents)
                .Include(h => h.Accounts)
                .ToListAsync();

            return _mapper.Map<IEnumerable<DocumentsDTO>>(documents);
        }

        public async Task<object> SetVisibility(int id, bool isVisible)
        {
            try
            {
                var newsEvent = await _context.Documents.FindAsync(id);
                if (newsEvent == null)
                {
                    throw new NotFoundException("Không tìm thấy bài viết");
                }

                newsEvent.IsVisible = isVisible;
                await _context.SaveChangesAsync();

                return new { message = "Cập nhật trạng thái thành công", isVisible };
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật trạng thái hiển thị", ex);
            }
        }

        public async Task<DocumentsDTO> GetDocumentsByIdAsync(int id)
        {
            try
            {
                var documents = await _context.Documents.FirstOrDefaultAsync(h => h.Id_document == id);
                return documents == null ? throw new NotFoundException("Không tìm thấy văn bản pháp luật ") : _mapper.Map<DocumentsDTO>(documents)
                    ;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin văn bản pháp luật", ex);
            }
        }

        public async Task<DocumentsDTO> CreateDocumentsAsync(DocumentsDTO documents)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(documents.Title))
                    throw new BadRequestException("Tên văn bản pháp luật không được để trống.");

                var documentModel = _mapper.Map<DocumentModel>(documents);

                if (!string.IsNullOrEmpty(documents.File_path))
                {
                    documentModel.File_path = documents.File_path;
                }

                _context.Documents.Add(documentModel);
                await _context.SaveChangesAsync();

                documentModel.Id_document = documentModel.Id_document;

                return _mapper.Map<DocumentsDTO>(documentModel);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi tạo văn bản pháp luật mới: " + ex.Message, ex);
            }
        }

        public async Task<DocumentsDTO> UpdateDocumentsAsync(int id, DocumentsDTO documents)
        {
            try
            {
                var document = await _context.Documents.FindAsync(id)
                               ?? throw new NotFoundException("Không tìm thấy văn bản pháp luật");

                if (string.IsNullOrWhiteSpace(documents.Title))
                    throw new BadRequestException("Tên văn bản pháp luật không được để trống.");

                document.Title = documents.Title;
                document.Description_short = documents.Description_short;
                document.Description = documents.Description;
                document.Id_account = documents.Id_account;
                document.Id_category_document = documents.Id_category_document;

                if (documents.File_path != null) 
                {
                    document.File_path = documents.File_path;
                }

                await _context.SaveChangesAsync();
                return _mapper.Map<DocumentsDTO>(document);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật văn bản pháp luật", ex);
            }
        }

        public async Task<object> DeleteDocumentsAsync(int id)
        {
            try
            {
                var documents = await _context.Documents.FindAsync(id)
                            ?? throw new NotFoundException("Không tìm thấy văn bản pháp luật");
                _context.Documents.Remove(documents);
                await _context.SaveChangesAsync();
                return new { message = "Xóa văn bản pháp luật thành công" };
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa văn bản pháp luật", ex);
            }
        }

        public async Task<string> UploadPdfDocumentsAsync(IFormFile pdfFile)
        {
            if (pdfFile.Length == 0 || pdfFile.ContentType != "application/pdf")
                throw new BadRequestException("File không hợp lệ hoặc không phải định dạng PDF.");

            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            var pdfName = Path.GetRandomFileName() + ".pdf";
            var path = Path.Combine(uploadFolder, pdfName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await pdfFile.CopyToAsync(stream);
            }

            return pdfName;
        }

        /******************************************************************************/
        public async Task<DocumentsDTO> GetDocByNameAsync(string name)
        {
            try
            {
                // Chuẩn hóa tên tài liệu từ URL                
                string normalizedInput = NormalizeTitle(name);

                // Lấy tất cả các tài liệu và sau đó lọc bên ngoài cơ sở dữ liệu
                var docs = await _context.Documents
                    .Where(h => h.IsVisible == true)
                    .ToListAsync();

                // Tìm tài liệu với tên đã chuẩn hóa
                var doc = docs.FirstOrDefault(d => NormalizeTitle(d.Title) == normalizedInput);

                // Kiểm tra nếu văn bản không tồn tại
                if (doc == null)
                {
                    throw new NotFoundException("Không tìm thấy văn bản");
                }

                // Tăng lượt xem trong cơ sở dữ liệu
                doc.View_documents = (doc.View_documents ?? 0) + 1; // Tăng lượt xem
                await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu

                // Chuyển đổi sang DTO và gán lượt xem
                var result = _mapper.Map<DocumentsDTO>(doc);
                result.View_documents = doc.View_documents; // Gán lượt xem từ cơ sở dữ liệu

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin tin tức và sự kiện", ex);
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

        //lấy văn bản = name danh mục
        public async Task<List<DocumentsDTO>> GetDocByNameCategogyAsync(string nameCategory)
        {
            try
            {
                // Chuẩn hóa tên danh mục từ URL
                string normalizedInput = NormalizeTitle(nameCategory);

                // Lấy dữ liệu từ database
                var documents = await _context.Documents
                    .Include(h => h.Category_Documents)
                    .Where(h => h.IsVisible == true) // Lọc ngay trong truy vấn
                    .ToListAsync();

                // Lọc lại bằng C# vì NormalizeTitle không thể dùng trong truy vấn
                var filteredDocs = documents
                    .Where(h => NormalizeTitle(h.Category_Documents.Name_category_document) == normalizedInput)
                    .ToList(); // Chuyển sang danh sách đã lọc

                // Kiểm tra nếu danh sách rỗng
                if (!filteredDocs.Any())
                {
                    throw new NotFoundException("Không tìm thấy văn bản.");
                }

                return _mapper.Map<List<DocumentsDTO>>(filteredDocs);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //Top5 văn bản pháp luật mới nhất
        public async Task<List<DocumentsDTO>> GetTop5LatestDocAsync()
        {
            try
            {
                var latestDoc = await _context.Documents
                    .OrderByDescending(h => h.Id_document) // Sắp xếp giảm dần theo ID
                    .Where(h => h.IsVisible == true)
                    .Take(5) // Lấy 5 văn bản pl mới nhất
                    .ToListAsync();

                return _mapper.Map<List<DocumentsDTO>>(latestDoc);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách top 5 văn bản pháp luật mới nhất", ex);
            }
        }

        //Top5 tin mới nhất trong dm 
        public async Task<List<DocumentsDTO>> GetTop5LatestDocByCategoryAsync(int categoryId)
        {
            try
            {
                // Lấy danh sách văn bản pl cùng danh mục và sắp xếp theo ID
                var latestDoc = await _context.Documents
                    .Where(h => h.Id_category_document == categoryId && h.IsVisible == true) // Chỉ lấy bài viết cùng danh mục
                    .OrderByDescending(h => h.Id_document)
                    .Take(5) // Lấy 5 văn bản mới nhất
                    .ToListAsync();

                return _mapper.Map<List<DocumentsDTO>>(latestDoc);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách top 5 văn bản mới nhất trong cùng danh mục", ex);
            }
        }

        //Tin liên quan trong dm
        public async Task<List<DocumentsDTO>> GetRelatedDocAsync(int id)
        {
            try
            {
                // Tìm bài viết hiện tại
                var currentDocument = await _context.Documents.FindAsync(id);
                if (currentDocument == null)
                {
                    return new List<DocumentsDTO>(); // Trả về danh sách rỗng
                }

                // Lấy danh sách bài viết liên quan cùng danh mục
                var relatedDocument = await _context.Documents
                    .Where(h => h.Id_category_document == currentDocument.Id_category_document && h.Id_document != id && h.IsVisible == true)
                    .ToListAsync();

                // Nếu không có bài viết liên quan, ném ra ngoại lệ
                return relatedDocument.Any() ? _mapper.Map<List<DocumentsDTO>>(relatedDocument) : new List<DocumentsDTO>();
            }
            catch (Exception ex)
            {
                return new List<DocumentsDTO>();
            }
        }

        //văn bản nổi bật
        public async Task<List<DocumentsDTO>> GetFeaturedDocAsync()
        {
            try
            {
                var today = DateTime.Today; // Lấy ngày hôm nay, bỏ phần giờ
                var oneWeekAgo = today.AddDays(-7); // Ngày cách đây 7 ngày

                // Lọc các tin được tạo trong khoảng 7 ngày qua
                var featuredDoc = await _context.Documents
                    .Where(n => n.Create_at.Date >= oneWeekAgo && n.Create_at.Date <= today && n.IsVisible == true) // So sánh chỉ theo ngày
                    .OrderByDescending(n => n.View_documents) // Sắp xếp theo lượt xem giảm dần
                    .Take(5) // Lấy top 5 văn bản có lượt xem nhiều nhất trong tuần
                    .ToListAsync();

                // Chuyển đổi sang DTO
                var result = _mapper.Map<List<DocumentsDTO>>(featuredDoc);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách văn bản", ex);
            }
        }
        public async Task<string> GetDocumentFilePathAsync(int id)
        {
            try
            {
                var document = await _context.Documents.FirstOrDefaultAsync(d => d.Id_document == id);
                if (document == null)
                {
                    throw new NotFoundException("Không tìm thấy văn bản.");
                }

                return Path.Combine(Directory.GetCurrentDirectory(), "Uploads", document.File_path);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy đường dẫn file tài liệu", ex);
            }
        }
    }
}

