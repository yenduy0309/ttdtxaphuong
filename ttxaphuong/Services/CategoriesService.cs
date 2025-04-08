using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Text;
using ttxaphuong.Data;
using ttxaphuong.DTO.News_events;
using ttxaphuong.Interfaces;
using ttxaphuong.Models.News_events;
using WebDoAn2.Exceptions;

namespace ttxaphuong.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoriesService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CategoriesDTO>> GetAllCategoriesAsync()
        {
            try
            {
                var allCategories = await _context.Categories.ToListAsync();

                var topLevelCategories = allCategories.Where(c => c.ParentId == null).ToList();

                foreach (var category in topLevelCategories)
                {
                    BuildCategoryHierarchy(category, allCategories);
                }

                return _mapper.Map<IEnumerable<CategoriesDTO>>(topLevelCategories);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy danh sách danh mục tin tức: " + ex.Message, ex);
            }
        }

        public async Task<CategoriesDTO> GetCategoriesByIdAsync(int id)
        {
            try
            {
                var category = await _context.Categories
                    .FirstOrDefaultAsync(b => b.Id_categories == id);

                if (category == null)
                {
                    throw new NotFoundException("Không tìm thấy danh mục tin tức với Id: " + id);
                }

                var allCategories = await _context.Categories.ToListAsync();

                BuildCategoryHierarchy(category, allCategories);

                return _mapper.Map<CategoriesDTO>(category);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy thông tin danh mục: " + ex.Message, ex);
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
        public async Task<CategoriesDTO> CreateCategoriesAsync(CategoriesDTO categories)
        {
            try
            {
                if (string.IsNullOrEmpty(categories.Name_category))
                {
                    throw new BadRequestException("Tên danh mục không được để trống.");
                }

                if (categories.ParentId.HasValue)
                {
                    var parentExists = await _context.Categories.AnyAsync(c => c.Id_categories == categories.ParentId.Value);
                    if (!parentExists)
                    {
                        throw new NotFoundException($"Danh mục cha với Id {categories.ParentId.Value} không tồn tại.");
                    }
                }

                var categoryModel = _mapper.Map<CategoriesModel>(categories);

                _context.Categories.Add(categoryModel);
                await _context.SaveChangesAsync();

                var createdCategory = await _context.Categories
                    .Include(c => c.Children)
                    .FirstOrDefaultAsync(c => c.Id_categories == categoryModel.Id_categories);

                return _mapper.Map<CategoriesDTO>(createdCategory);
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
                throw new Exception("Có lỗi xảy ra khi tạo danh mục tin tức mới: " + ex.Message, ex);
            }
        }

        //public async Task<CategoriesDTO> UpdateCategoriesAsync(int id, CategoriesDTO categoriesDTO)
        //{
        //    try
        //    {
        //        var model = await _context.Categories
        //            .Include(c => c.Children) 
        //            .FirstOrDefaultAsync(c => c.Id_categories == id)
        //            ?? throw new NotFoundException($"Không tìm thấy danh mục tin tức với Id: {id}");

        //        if (string.IsNullOrWhiteSpace(categoriesDTO.Name_category))
        //        {
        //            throw new BadRequestException("Tên danh mục tin tức không được để trống.");
        //        }

        //        model.Name_category = categoriesDTO.Name_category;

        //        if (categoriesDTO.ParentId != model.ParentId)
        //        {
        //            if (categoriesDTO.ParentId.HasValue)
        //            {
        //                var parentExists = await _context.Categories.AnyAsync(c => c.Id_categories == categoriesDTO.ParentId.Value);
        //                if (!parentExists)
        //                {
        //                    throw new NotFoundException($"Danh mục cha với Id {categoriesDTO.ParentId.Value} không tồn tại.");
        //                }

        //                // Kiểm tra tránh vòng lặp (không cho phép danh mục trở thành cha của chính nó hoặc con của nó)
        //                if (await IsDescendant(id, categoriesDTO.ParentId.Value))
        //                {
        //                    throw new BadRequestException("Không thể đặt danh mục làm cha của chính nó hoặc con của nó.");
        //                }
        //            }

        //            model.ParentId = categoriesDTO.ParentId;
        //        }

        //        await _context.SaveChangesAsync();

        //        var allCategories = await _context.Categories.ToListAsync();
        //        BuildCategoryHierarchy(model, allCategories);

        //        return _mapper.Map<CategoriesDTO>(model);
        //    }
        //    catch (NotFoundException)
        //    {
        //        throw;
        //    }
        //    catch (BadRequestException)
        //    {
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Có lỗi xảy ra khi cập nhật danh mục tin tức: " + ex.Message, ex);
        //    }
        //}

        public async Task<CategoriesDTO> UpdateCategoriesAsync(int id, CategoriesDTO categoriesDTO)
        {
            try
            {
                var model = await _context.Categories
                    .Include(c => c.Children)
                    .FirstOrDefaultAsync(c => c.Id_categories == id)
                    ?? throw new NotFoundException($"Không tìm thấy danh mục tin tức với Id: {id}");

                if (string.IsNullOrWhiteSpace(categoriesDTO.Name_category))
                {
                    throw new BadRequestException("Tên danh mục tin tức không được để trống.");
                }

                model.Name_category = categoriesDTO.Name_category;

                if (categoriesDTO.ParentId != model.ParentId)
                {
                    if (categoriesDTO.ParentId.HasValue)
                    {
                        var parentExists = await _context.Categories.AnyAsync(c => c.Id_categories == categoriesDTO.ParentId.Value);
                        if (!parentExists)
                        {
                            throw new NotFoundException($"Danh mục cha với Id {categoriesDTO.ParentId.Value} không tồn tại.");
                        }
                        if (await IsDescendant(id, categoriesDTO.ParentId.Value))
                        {
                            throw new BadRequestException("Không thể đặt danh mục làm cha của chính nó hoặc con của nó.");
                        }
                    }
                    model.ParentId = categoriesDTO.ParentId; // Cho phép ParentId = null
                }

                await _context.SaveChangesAsync();

                var allCategories = await _context.Categories.ToListAsync();
                BuildCategoryHierarchy(model, allCategories);

                return _mapper.Map<CategoriesDTO>(model);
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
                throw new Exception("Có lỗi xảy ra khi cập nhật danh mục tin tức: " + ex.Message, ex);
            }
        }

        private async Task<bool> IsDescendant(int id, int parentId)
        {
            var current = await _context.Categories.FirstOrDefaultAsync(c => c.Id_categories == parentId);
            while (current != null)
            {
                if (current.Id_categories == id)
                {
                    return true;
                }
                current = await _context.Categories.FirstOrDefaultAsync(c => c.Id_categories == current.ParentId);
            }
            return false;
        }
        // Xóa từng danh mục
        public async Task<object> DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _context.Categories
                    .Include(c => c.Children)
                    .FirstOrDefaultAsync(c => c.Id_categories == id)
                    ?? throw new NotFoundException($"Không tìm thấy danh mục tin tức với Id: {id}");

                foreach (var child in category.Children)
                {
                    child.ParentId = category.ParentId; // Chuyển lên cấp cha của danh mục bị xóa
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return new { message = "Xóa danh mục tin tức thành công, các menu con đã được chuyển lên cấp cao hơn!" };
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi xóa danh mục tin tức: " + ex.Message, ex);
            }
        }

        // Xóa nhiều danh mục
        public async Task<object> DeleteCategoriesAsync(List<int> ids)
        {
            try
            {
                var categoriesToDelete = await _context.Categories.AsNoTracking()
                    .Include(c => c.Children)
                    .Where(c => ids.Contains(c.Id_categories))
                    .ToListAsync();

                foreach (var category in categoriesToDelete)
                {
                    foreach (var child in category.Children)
                    {
                        child.ParentId = category.ParentId; // Chuyển lên cấp cha của danh mục bị xóa
                    }
                    _context.Categories.Remove(category); // Xóa danh mục
                }

                await _context.SaveChangesAsync();

                return new { message = "Xóa danh mục thành công, các danh mục con đã được chuyển lên cấp cao hơn!" };
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi xóa danh mục: " + ex.Message, ex);
            }
        }


        /*******************************************//*******************************************/

        public async Task<List<CategoriesDTO>> GetAllCategories()
        {
            return await _context.Categories
                .Select(c => new CategoriesDTO
                {
                    Id_categories = c.Id_categories,
                    Name_category = c.Name_category,
                    ParentId = c.ParentId
                })
                .ToListAsync();
        }

        public async Task<List<CategoriesDTO>> GetCategoryHierarchy()
        {
            var categories = await GetAllCategories(); // Lấy danh sách danh mục
            return await BuildCategoryTree(categories, null); // Tạo cây danh mục
        }

        public async Task<List<CategoriesDTO>> BuildCategoryTree(List<CategoriesDTO> categories, int? parentId)
        {
            var categoryList = categories
                .Where(c => c.ParentId == parentId)
                .Select(c => new CategoriesDTO
                {
                    Id_categories = c.Id_categories,
                    Name_category = c.Name_category,
                    ParentId = c.ParentId,
                    Children = new List<CategoriesDTO>() // Khởi tạo list Children
                })
                .ToList();

            foreach (var category in categoryList)
            {
                // Tìm danh mục con của danh mục hiện tại
                var children = await BuildCategoryTree(categories, category.Id_categories);
                category.Children.AddRange(children);
            }

            return categoryList;
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

        public async Task<List<string>> GetAllSubCategoryNamesByNameAsync(string parentName)
        {
            // Chuẩn hóa tên từ URL
            string normalizedInput = NormalizeTitle(parentName);
            var categories = await GetAllCategories();
            var parentCategory = categories.FirstOrDefault(c => NormalizeTitle(c.Name_category) == normalizedInput);

            if (parentCategory == null)
            {
                return new List<string>(); // Trả về danh sách rỗng nếu không tìm thấy danh mục cha
            }

            var result = new List<string>();
            GetSubCategoryNamesRecursive(categories, parentCategory.Id_categories, result);
            return result;
        }

        private void GetSubCategoryNamesRecursive(List<CategoriesDTO> categories, int? parentId, List<string> result)

        {
            var subCategories = categories.Where(c => c.ParentId == parentId).ToList();
            foreach (var sub in subCategories)
            {
                result.Add(sub.Name_category); // Chỉ lấy tên danh mục
                GetSubCategoryNamesRecursive(categories, sub.Id_categories, result); // Đệ quy lấy danh mục con
            }
        }

        public async Task<CategoriesDTO> GetCategoryById(int id)
        {
            var category = await _context.Categories
                .Where(c => c.Id_categories == id)
                .Select(c => c.Name_category) // Chỉ lấy trường Name của danh mục
                .FirstOrDefaultAsync();

            // Tạo đối tượng CategoriesDTO và gán giá trị
            return new CategoriesDTO
            {
                Name_category = category // Trả về đối tượng DTO chứa tên danh mục
            };
        }

    }
}
