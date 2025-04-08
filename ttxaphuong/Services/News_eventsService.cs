using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Text;
using ttxaphuong.Data;
using ttxaphuong.DTO.News_events;
using ttxaphuong.Interfaces;
using ttxaphuong.Models.News_events;
using WebDoAn2.Exceptions;
using System.Xml.Linq;

namespace ttxaphuong.Services
{
    public class News_eventsService : INews_eventsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public News_eventsService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        //public async Task<IEnumerable<News_eventsDTO>> GetNews_Events()
        //{
        //    try
        //    {
        //        var newsEvents = await _context.News_Events
        //            .Include(ne => ne.CategoriesModel)
        //            .Include(ne => ne.Accounts).Where(h => h.IsVisible == true)
        //            .ToListAsync();

        //        return _mapper.Map<IEnumerable<News_eventsDTO>>(newsEvents);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Có lỗi xảy ra khi lấy danh sách tin tức và sự kiện: " + ex.Message, ex);
        //    }
        //}

        public async Task<IEnumerable<News_eventsDTO>> GetAllNews_EventsAsync(bool? isVisible = null)
        {
            var query = _context.News_Events.AsQueryable();

            if (isVisible.HasValue)
            {
                query = query.Where(n => n.IsVisible == isVisible.Value);
            }

            var newsEvents = await query
                .Include(ne => ne.CategoriesModel)
                .Include(ne => ne.Accounts)
                .ToListAsync();

            return _mapper.Map<IEnumerable<News_eventsDTO>>(newsEvents);
        }

        public async Task<object> SetVisibility(int id, bool isVisible)
        {
            try
            {
                var newsEvent = await _context.News_Events.FindAsync(id);
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

        private void BuildCategoryHierarchy(CategoriesModel parent, List<CategoriesModel> allCategories)
        {
            parent.Children = allCategories.Where(c => c.ParentId == parent.Id_categories).ToList();
            foreach (var child in parent.Children)
            {
                BuildCategoryHierarchy(child, allCategories);
            }
        }

        public async Task<News_eventsDTO> CreateNews_EventsAsync(News_eventsDTO news_events)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(news_events.Title))
                    throw new BadRequestException("Tên tin tức và sự kiện không được để trống.");

                var news_eventsModel = _mapper.Map<News_eventsModel>(news_events);

                if (!string.IsNullOrEmpty(news_events.Image))
                {
                    news_eventsModel.Image = news_events.Image; // Lưu đường dẫn hình ảnh
                }

                _context.News_Events.Add(news_eventsModel);
                news_eventsModel.IsVisible = news_events.IsVisible; // ✅ Lưu trạng thái hiển thị

                await _context.SaveChangesAsync();

                news_eventsModel.Id_newsevent = news_eventsModel.Id_newsevent;

                return _mapper.Map<News_eventsDTO>(news_eventsModel);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi tạo tin tức và sự kiện mới: " + ex.Message, ex);
            }
        }

        public async Task<News_eventsDTO> UpdateNews_EventsAsync(int id, News_eventsDTO news_events)
        {
            try
            {
                var existingNewsEvent = await _context.News_Events.FindAsync(id)
                    ?? throw new NotFoundException("Không tìm thấy tin tức và sự kiện");

                if (string.IsNullOrWhiteSpace(news_events.Title))
                    throw new BadRequestException("Tên tin tức và sự kiện không được để trống.");

                existingNewsEvent.Title = news_events.Title;
                existingNewsEvent.Description_short = news_events.Description_short;
                existingNewsEvent.Content = news_events.Content;
                existingNewsEvent.Image = news_events.Image;
                existingNewsEvent.Id_account = news_events.Id_account;
                existingNewsEvent.Formatted_content = news_events.Formatted_content;
                existingNewsEvent.Id_categories = news_events.Id_categories;
                existingNewsEvent.IsVisible = news_events.IsVisible; // ✅ Cập nhật trạng thái hiển thị

                await _context.SaveChangesAsync();
                return _mapper.Map<News_eventsDTO>(existingNewsEvent);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật tin tức và sự kiện", ex);
            }
        }

        public async Task<object> DeleteNews_EventsAsync(int id)
        {
            try
            {
                var news_events = await _context.News_Events.FindAsync(id)
                            ?? throw new NotFoundException("Không tìm tin tức và sự kiện");
                _context.News_Events.Remove(news_events);
                await _context.SaveChangesAsync();
                return new { message = "Xóa tin tức và sự kiện thành công" };
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa tin tức và sự kiện", ex);
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

        public async Task<News_eventsDTO> GetNews_EventsByIdAsync(int id)
        {
            var news_events = await _context.News_Events.FirstOrDefaultAsync(h => h.Id_newsevent == id);

            if (news_events == null || news_events.IsVisible == false)
            {
                throw new NotFoundException("Bài viết này không tồn tại hoặc đã bị ẩn.");
            }

            // Tăng lượt xem
            news_events.View = (news_events.View ?? 0) + 1;
            await _context.SaveChangesAsync();

            return _mapper.Map<News_eventsDTO>(news_events);
        }


        /***************************************Phần User************************************/
        //Top5 tin tức mới nhất
        public async Task<List<News_eventsDTO>> GetTop5LatestNews_EventsAsync()
        {
            try
            {
                var latestNewsEvents = await _context.News_Events
                    .Where(h => h.IsVisible == true)
                    .OrderByDescending(h => h.Id_newsevent) // Sắp xếp giảm dần theo ID
                    .Take(5) // Lấy 5 bài viết mới nhất
                    .ToListAsync();

                return _mapper.Map<List<News_eventsDTO>>(latestNewsEvents);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách top 5 bài viết mới nhất", ex);
            }
        }

        //Top5 tin mới nhất trong dm 
        public async Task<List<News_eventsDTO>> GetTop5LatestNews_EventsByCategoryAsync(string categoryName)
        {
            try
            {
                // Chuẩn hóa tên danh mục
                string normalizedInput = NormalizeTitle(categoryName);

                // Lấy dữ liệu từ database trước, không dùng NormalizeTitle trong truy vấn
                var news_events = await _context.News_Events
                    .Include(h => h.CategoriesModel)
                    .Where(h => h.IsVisible == true) // Lọc những bài viết hiển thị
                    .ToListAsync(); // Chuyển về danh sách trong bộ nhớ

                // Lọc lại danh mục bằng C#
                var latestNewsEvents = news_events
                    .Where(h => NormalizeTitle(h.CategoriesModel.Name_category) == normalizedInput)
                    .OrderByDescending(h => h.Id_newsevent) // Sắp xếp giảm dần theo ID
                    .Take(5) // Lấy 5 bài viết mới nhất
                    .ToList();

                return _mapper.Map<List<News_eventsDTO>>(latestNewsEvents);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách top 5 bài viết mới nhất trong cùng danh mục", ex);
            }
        }
        //
        public async Task<List<News_eventsDTO>> GetTopViewedNews(int count)
        {
            try
            {
                // Lấy danh sách tất cả tin tức từ cơ sở dữ liệu
                var newsList = await _context.News_Events
                    .OrderByDescending(news => news.View) // Sắp xếp theo lượt xem giảm dần
                    .Take(count) // Lấy số lượng `count` tin tức có lượt xem cao nhất
                    .ToListAsync();

                return _mapper.Map<List<News_eventsDTO>>(newsList);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách tin tức xem nhiều nhất", ex);
            }
        }

        //tin tức nổi bật
        public async Task<List<News_eventsDTO>> GetFeaturedNewsAsync()
        {
            try
            {
                var today = DateTime.Today; // Lấy ngày hôm nay, bỏ phần giờ
                var oneWeekAgo = today.AddDays(-7); // Ngày cách đây 7 ngày

                // Lọc các tin được tạo trong khoảng 7 ngày qua
                var featuredNews = await _context.News_Events
                    .Where(n => n.Create_at.Date >= oneWeekAgo && n.Create_at.Date <= today && n.IsVisible == true) // So sánh chỉ theo ngày
                    .OrderByDescending(n => n.View) // Sắp xếp theo lượt xem giảm dần
                    .Take(4) 
                    .ToListAsync();

                // Chuyển đổi sang DTO
                var result = _mapper.Map<List<News_eventsDTO>>(featuredNews);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách tin tức nổi bật", ex);
            }
        }

        //Tin liên quan trong dm
        public async Task<List<News_eventsDTO>> GetRelatedNews_EventsAsync(string title)
        {
            try
            {
                // Chuẩn hóa tên tài liệu từ URL
                string normalizedInput = NormalizeTitle(title);

                var allNewsEvents = await _context.News_Events.ToListAsync();

                // Tìm bài viết hiện tại dựa trên tiêu đề chuẩn hóa
                var currentNewsEvent = allNewsEvents
                    .FirstOrDefault(h => NormalizeTitle(h.Title) == normalizedInput);
                if (currentNewsEvent == null)
                {
                    return new List<News_eventsDTO>(); // Trả về danh sách rỗng
                }

                // Lấy danh sách bài viết liên quan cùng danh mục
                var relatedNewsEvents = allNewsEvents
                    .Where(h => h.Id_categories == currentNewsEvent.Id_categories && h.IsVisible == true && h.Id_newsevent != currentNewsEvent.Id_newsevent)
                    .ToList();

                // Nếu không có bài viết liên quan, ném ra ngoại lệ
                return relatedNewsEvents.Any() ? _mapper.Map<List<News_eventsDTO>>(relatedNewsEvents) : new List<News_eventsDTO>();
            }
            catch (Exception ex)
            {
                return new List<News_eventsDTO>();
            }
        }

        public async Task<News_eventsDTO> GetNews_EventsByNameAsync(string name)
        {
            try
            {
                // Chuẩn hóa tên tài liệu từ URL
                string normalizedInput = NormalizeTitle(name);
                // Lấy tất cả các tài liệu và sau đó lọc bên ngoài cơ sở dữ liệu
                var news = await _context.News_Events
                    .Where(h => h.IsVisible == true).ToListAsync();

                // Tìm tài liệu với tên đã chuẩn hóa
                var news_events = news.FirstOrDefault(d => NormalizeTitle(d.Title) == normalizedInput);

                // Kiểm tra nếu bài viết không tồn tại
                if (news_events == null)
                {
                    return null;
                }

                // Tăng lượt xem trong cơ sở dữ liệu
                news_events.View = (news_events.View ?? 0) + 1; // Tăng lượt xem
                await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu

                // Chuyển đổi sang DTO và gán lượt xem
                var result = _mapper.Map<News_eventsDTO>(news_events);
                result.View = news_events.View; // Gán lượt xem từ cơ sở dữ liệu

                return result;
            }
            catch (Exception ex)
            {
                return null;
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

        //lấy tin tức = name dm
        public async Task<List<News_eventsDTO>> GetNews_EventsByNameCategogyAsync(string nameCatelogy)
        {
            try
            {
                // Chuẩn hóa tên tài liệu từ URL
                string normalizedInput = NormalizeTitle(nameCatelogy);

                // Lấy dữ liệu từ database trước, không dùng NormalizeTitle trong truy vấn
                var news_events = await _context.News_Events
                    .Include(h => h.CategoriesModel)
                    .Where(h => h.IsVisible == true) // Lọc trong database
                    .ToListAsync(); // Chuyển thành danh sách trong bộ nhớ

                // Lọc lại bằng C# vì NormalizeTitle không thể dịch sang SQL
                var filteredNews = news_events
                    .Where(h => NormalizeTitle(h.CategoriesModel.Name_category) == normalizedInput)
                    .ToList(); // Không dùng ToListAsync() vì dữ liệu đã ở bộ nhớ

                if (!filteredNews.Any())
                {
                    return null;
                }

                return _mapper.Map<List<News_eventsDTO>>(filteredNews);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}