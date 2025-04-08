using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Text;
using ttxaphuong.Data;
using ttxaphuong.DTO.Documents;
using ttxaphuong.Interfaces;
using ttxaphuong.Models.Documents;
using WebDoAn2.Exceptions;

namespace ttxaphuong.Services
{
    public class Category_documentsService : ICategory_documentsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public Category_documentsService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<Category_documentsDTO>> GetCategory_documentsAsync()
        {
            try
            {
                var allCategories_documents = await _context.Category_Documents.ToListAsync();

                var topLevelCategories = allCategories_documents.Where(c => c.DocumentParentId == null).ToList();

                foreach (var category in topLevelCategories)
                {
                    BuildCategoryHierarchy(category, allCategories_documents);
                }

                return _mapper.Map<IEnumerable<Category_documentsDTO>>(topLevelCategories);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy danh sách danh mục văn bản: " + ex.Message, ex);
            }
        }

        public async Task<Category_documentsDTO> GetCategory_documentsByIdAsync(int id)
        {
            try
            {
                var category = await _context.Category_Documents
                    .FirstOrDefaultAsync(b => b.Id_category_document == id);

                if (category == null)
                {
                    throw new NotFoundException("Không tìm thấy danh mục văn bản với Id: " + id);
                }

                var allCategories = await _context.Category_Documents.ToListAsync();

                BuildCategoryHierarchy(category, allCategories);

                return _mapper.Map<Category_documentsDTO>(category);
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

        private void BuildCategoryHierarchy(Category_documentsModel parentDocuments, List<Category_documentsModel> allCategories_documents)
        {
            parentDocuments.Children = allCategories_documents.Where(c => c.DocumentParentId == parentDocuments.Id_category_document).ToList();
            foreach (var child in parentDocuments.Children)
            {
                BuildCategoryHierarchy(child, allCategories_documents);
            }
        }

        public async Task<Category_documentsDTO> CreateCategory_documentsAsync(Category_documentsDTO category_Documents)
        {
            try
            {
                if (string.IsNullOrEmpty(category_Documents.Name_category_document))
                {
                    throw new BadRequestException("Tên danh mục không được để trống.");
                }

                if (category_Documents.DocumentParentId.HasValue)
                {
                    var parentExists = await _context.Category_Documents.AnyAsync(c => c.Id_category_document == category_Documents.DocumentParentId.Value);
                    if (!parentExists)
                    {
                        throw new NotFoundException($"Danh mục cha với Id {category_Documents.DocumentParentId.Value} không tồn tại.");
                    }
                }

                var categoryModel = _mapper.Map<Category_documentsModel>(category_Documents);

                _context.Category_Documents.Add(categoryModel);
                await _context.SaveChangesAsync();

                var createdCategory = await _context.Category_Documents
                    .Include(c => c.Children)
                    .FirstOrDefaultAsync(c => c.Id_category_document == categoryModel.Id_category_document);

                return _mapper.Map<Category_documentsDTO>(createdCategory);
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
                throw new Exception("Có lỗi xảy ra khi tạo danh mục văn bản mới: " + ex.Message, ex);
            }
        }

        public async Task<Category_documentsDTO> UpdateCategory_documentsAsync(int id, Category_documentsDTO categories_documentsDTO)
        {
            try
            {
                var model = await _context.Category_Documents
                    .Include(c => c.Children)
                    .FirstOrDefaultAsync(c => c.Id_category_document == id)
                    ?? throw new NotFoundException($"Không tìm thấy danh mục văn bản với Id: {id}");

                if (string.IsNullOrWhiteSpace(categories_documentsDTO.Name_category_document))
                {
                    throw new BadRequestException("Tên danh mục văn bản không được để trống.");
                }

                model.Name_category_document = categories_documentsDTO.Name_category_document;

                if (categories_documentsDTO.DocumentParentId != model.DocumentParentId)
                {
                    if (categories_documentsDTO.DocumentParentId.HasValue)
                    {
                        var parentExists = await _context.Category_Documents.AnyAsync(c => c.Id_category_document == categories_documentsDTO.DocumentParentId.Value);
                        if (!parentExists)
                        {
                            throw new NotFoundException($"Danh mục cha với Id {categories_documentsDTO.DocumentParentId.Value} không tồn tại.");
                        }
                        if (await IsDescendant(id, categories_documentsDTO.DocumentParentId.Value))
                        {
                            throw new BadRequestException("Không thể đặt danh mục làm cha của chính nó hoặc con của nó.");
                        }
                    }
                    model.DocumentParentId = categories_documentsDTO.DocumentParentId; // Cho phép ParentId = null
                }

                await _context.SaveChangesAsync();

                var allCategories = await _context.Category_Documents.ToListAsync();
                BuildCategoryHierarchy(model, allCategories);

                return _mapper.Map<Category_documentsDTO>(model);
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
                throw new Exception("Có lỗi xảy ra khi cập nhật danh mục văn bản: " + ex.Message, ex);
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

        public async Task<object> DeleteCategory_documentsAsync(int id)
        {
            try
            {
                var category = await _context.Category_Documents
                    .Include(c => c.Children)
                    .FirstOrDefaultAsync(c => c.Id_category_document == id)
                    ?? throw new NotFoundException($"Không tìm thấy danh mục văn bản với Id: {id}");

                foreach (var child in category.Children)
                {
                    child.DocumentParentId = category.DocumentParentId; // Chuyển lên cấp cha của danh mục bị xóa
                }

                _context.Category_Documents.Remove(category);
                await _context.SaveChangesAsync();

                return new { message = "Xóa danh mục văn bản thành công, các menu con đã được chuyển lên cấp cao hơn!" };
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi xóa danh mục văn bản: " + ex.Message, ex);
            }
        }


        /*******************************************//*******************************************/

        public async Task<List<Category_documentsDTO>> GetAllCategories()
        {
            return await _context.Category_Documents
                .Select(c => new Category_documentsDTO
                {
                    Id_category_document = c.Id_category_document,
                    Name_category_document = c.Name_category_document,
                    DocumentParentId = c.DocumentParentId
                })
                .ToListAsync();
        }
        public async Task<List<Category_documentsDTO>> GetCat_DocHierarchy()
        {
            var categories_doc = await GetAllCategories(); // Lấy danh sách danh mục
            return await BuildCat_DocTree(categories_doc, null); // Tạo cây danh mục
        }

        public async Task<List<Category_documentsDTO>> BuildCat_DocTree(List<Category_documentsDTO> categories, int? parentId)
        {
            var categoryList = categories
                .Where(c => c.DocumentParentId == parentId)
                .Select(c => new Category_documentsDTO
                {
                    Id_category_document = c.Id_category_document,
                    Name_category_document = c.Name_category_document,
                    DocumentParentId = c.DocumentParentId,
                    Children = new List<Category_documentsDTO>() // Khởi tạo list Children
                })
                .ToList();

            foreach (var category in categoryList)
            {
                // Tìm danh mục con của danh mục hiện tại
                var children = await BuildCat_DocTree(categories, category.Id_category_document);
                category.Children.AddRange(children);
            }

            return categoryList;
        }

        public async Task<List<string>> GetAllSubCat_DocNamesByNameAsync(string parentName)
        {
            // Chuẩn hóa tên từ URL
            string normalizedInput = NormalizeTitle(parentName);
            var categories = await GetAllCategories();
            var parentCategory = categories.FirstOrDefault(c => NormalizeTitle(c.Name_category_document) == normalizedInput);

            if (parentCategory == null)
            {
                return new List<string>(); // Trả về danh sách rỗng nếu không tìm thấy danh mục cha
            }

            var result = new List<string>();
            GetSubCategoryNamesRecursive(categories, parentCategory.Id_category_document, result);
            return result;
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

        private void GetSubCategoryNamesRecursive(List<Category_documentsDTO> categories, int? parentId, List<string> result)

        {
            var subCategories = categories.Where(c => c.DocumentParentId == parentId).ToList();
            foreach (var sub in subCategories)
            {
                result.Add(sub.Name_category_document); // Chỉ lấy tên danh mục
                GetSubCategoryNamesRecursive(categories, sub.Id_category_document, result); // Đệ quy lấy danh mục con
            }
        }

        public async Task<Category_documentsDTO> GetCategoryById(int id)
        {
            var namecategory = await _context.Category_Documents
                .Where(c => c.Id_category_document == id)
                .Select(c => c.Name_category_document) // Chỉ lấy trường Name của danh mục
                .FirstOrDefaultAsync();

            // Tạo đối tượng Category_documentsDTO và gán giá trị
            return new Category_documentsDTO
            {
                Name_category_document = namecategory // Trả về đối tượng DTO chứa tên danh mục
            };
        }
    }
}
