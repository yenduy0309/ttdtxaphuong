using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ttxaphuong.Data;
using ttxaphuong.DTO.Category_field;
using ttxaphuong.Interfaces;
using ttxaphuong.Models.Procedures;
using WebDoAn2.Exceptions;

namespace ttxaphuong.Services
{
    public class CategoryFieldService : ICategory_fieldService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoryFieldService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<Category_fieldDTO>> GetAllCategory_fieldAsync()
        {
            try
            {
                var allCategories = await _context.Categogy_Fields.ToListAsync();

                var topLevelCategories = allCategories.Where(c => c.FielParentId == null).ToList();

                foreach (var category in topLevelCategories)
                {
                    BuildCategoryHierarchy(category, allCategories);
                }

                return _mapper.Map<IEnumerable<Category_fieldDTO>>(topLevelCategories);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy danh sách danh mục lĩnh vực: " + ex.Message, ex);
            }
        }

        public async Task<Category_fieldDTO> GetCategory_fieldByIdAsync(int id)
        {
            try
            {
                var category = await _context.Categogy_Fields
                    .FirstOrDefaultAsync(b => b.Id_Field == id);

                if (category == null)
                {
                    throw new NotFoundException("Không tìm thấy danh mục lĩnh vực với Id: " + id);
                }

                var allCategories = await _context.Categogy_Fields.ToListAsync();

                BuildCategoryHierarchy(category, allCategories);

                return _mapper.Map<Category_fieldDTO>(category);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy thông tin danh mục lĩnh vực: " + ex.Message, ex);
            }
        }
        private void BuildCategoryHierarchy(Categogy_fieldModel parent, List<Categogy_fieldModel> allCategories)
        {
            parent.Children = allCategories.Where(c => c.FielParentId == parent.Id_Field).ToList();
            foreach (var child in parent.Children)
            {
                BuildCategoryHierarchy(child, allCategories);
            }
        }
        public async Task<Category_fieldDTO> CreateCategory_fieldAsync(Category_fieldDTO categories)
        {
            try
            {
                if (string.IsNullOrEmpty(categories.Name_Field))
                {
                    throw new BadRequestException("Tên danh mục lĩnh vực không được để trống.");
                }

                if (categories.FielParentId.HasValue)
                {
                    var parentExists = await _context.Categogy_Fields.AnyAsync(c => c.Id_Field == categories.FielParentId.Value);
                    if (!parentExists)
                    {
                        throw new NotFoundException($"Danh mục cha với Id {categories.FielParentId.Value} không tồn tại.");
                    }
                }

                var categoryModel = _mapper.Map<Categogy_fieldModel>(categories);

                _context.Categogy_Fields.Add(categoryModel);
                await _context.SaveChangesAsync();

                var createdCategory = await _context.Categogy_Fields
                    .Include(c => c.Children)
                    .FirstOrDefaultAsync(c => c.Id_Field == categoryModel.Id_Field);

                return _mapper.Map<Category_fieldDTO>(createdCategory);
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
                throw new Exception("Có lỗi xảy ra khi tạo danh mục lĩnh vực mới: " + ex.Message, ex);
            }
        }

        public async Task<Category_fieldDTO> UpdateCategory_fieldAsync(int id, Category_fieldDTO categoriesDTO)
        {
            try
            {
                var model = await _context.Categogy_Fields
                    .Include(c => c.Children)
                    .FirstOrDefaultAsync(c => c.Id_Field == id)
                    ?? throw new NotFoundException($"Không tìm thấy danh mục lĩnh vực với Id: {id}");

                if (string.IsNullOrWhiteSpace(categoriesDTO.Name_Field))
                {
                    throw new BadRequestException("Tên danh mục lĩnh vực không được để trống.");
                }

                model.Name_Field = categoriesDTO.Name_Field;

                if (categoriesDTO.FielParentId != model.FielParentId)
                {
                    if (categoriesDTO.FielParentId.HasValue)
                    {
                        var parentExists = await _context.Categogy_Fields.AnyAsync(c => c.Id_Field == categoriesDTO.FielParentId.Value);
                        if (!parentExists)
                        {
                            throw new NotFoundException($"Danh mục cha với Id {categoriesDTO.FielParentId.Value} không tồn tại.");
                        }
                        if (await IsDescendant(id, categoriesDTO.FielParentId.Value))
                        {
                            throw new BadRequestException("Không thể đặt danh mục làm cha của chính nó hoặc con của nó.");
                        }
                    }
                    model.FielParentId = categoriesDTO.FielParentId; // Cho phép ParentId = null
                }

                await _context.SaveChangesAsync();

                var allCategories = await _context.Categogy_Fields.ToListAsync();
                BuildCategoryHierarchy(model, allCategories);

                return _mapper.Map<Category_fieldDTO>(model);
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
                throw new Exception("Có lỗi xảy ra khi cập nhật danh mục lĩnh vực: " + ex.Message, ex);
            }
        }

        private async Task<bool> IsDescendant(int id, int parentId)
        {
            var current = await _context.Categogy_Fields.FirstOrDefaultAsync(c => c.Id_Field == parentId);
            while (current != null)
            {
                if (current.Id_Field == id)
                {
                    return true;
                }
                current = await _context.Categogy_Fields.FirstOrDefaultAsync(c => c.Id_Field == current.FielParentId);
            }
            return false;
        }
        public async Task<object> DeleteCategory_fieldAsync(int id)
        {
            try
            {
                var category = await _context.Categogy_Fields
                    .Include(c => c.Children)
                    .FirstOrDefaultAsync(c => c.Id_Field == id)
                    ?? throw new NotFoundException($"Không tìm thấy danh mục lĩnh vực với Id: {id}");

                foreach (var child in category.Children)
                {
                    child.FielParentId = category.FielParentId; // Chuyển lên cấp cha của danh mục bị xóa
                }

                _context.Categogy_Fields.Remove(category);
                await _context.SaveChangesAsync();

                return new { message = "Xóa danh mục lĩnh vực thành công, các menu con đã được chuyển lên cấp cao hơn!" };
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi xóa danh mục lĩnh vực: " + ex.Message, ex);
            }
        }

        /*******************************************//*******************************************/

        public async Task<List<Category_fieldDTO>> GetAllCategoriesAsync()
        {
            return await _context.Categogy_Fields
                .Select(c => new Category_fieldDTO
                {
                    Id_Field = c.Id_Field,
                    Name_Field = c.Name_Field,
                    FielParentId = c.FielParentId
                })
                .ToListAsync();
        }

        public async Task<List<Category_fieldDTO>> GetCategoryHierarchy()
        {
            var categories = await GetAllCategoriesAsync(); // Lấy danh sách danh mục
            return await BuildCategoryTree(categories, null); // Tạo cây danh mục
        }

        public async Task<List<Category_fieldDTO>> BuildCategoryTree(List<Category_fieldDTO> categories, int? parentId)
        {
            var categoryList = categories
                .Where(c => c.FielParentId == parentId)
                .Select(c => new Category_fieldDTO
                {
                    Id_Field = c.Id_Field,
                    Name_Field = c.Name_Field,
                    FielParentId = c.FielParentId,
                    Children = new List<Category_fieldDTO>() // Khởi tạo list Children
                })
                .ToList();

            foreach (var category in categoryList)
            {
                // Tìm danh mục con của danh mục hiện tại
                var children = await BuildCategoryTree(categories, category.Id_Field);
                category.Children.AddRange(children);
            }

            return categoryList;
        }

        public async Task<List<string>> GetAllSubCategoryNamesByNameAsync(string parentName)
        {
            var categories = await GetAllCategoriesAsync();
            var parentCategory = categories.FirstOrDefault(c => c.Name_Field == parentName);

            if (parentCategory == null)
            {
                return new List<string>(); // Trả về danh sách rỗng nếu không tìm thấy danh mục cha
            }

            var result = new List<string>();
            GetSubCategoryNamesRecursive(categories, parentCategory.Id_Field, result);
            return result;
        }

        private void GetSubCategoryNamesRecursive(List<Category_fieldDTO> categories, int? parentId, List<string> result)

        {
            var subCategories = categories.Where(c => c.FielParentId == parentId).ToList();
            foreach (var sub in subCategories)
            {
                result.Add(sub.Name_Field); // Chỉ lấy tên danh mục
                GetSubCategoryNamesRecursive(categories, sub.Id_Field, result); // Đệ quy lấy danh mục con
            }
        }
        /***************************************/
        public async Task<List<int>> GetAllSubCategoryIdByIdAsync(int id)
        {
            var categories = await GetAllCategoriesAsync();
            var parentCategory = categories.FirstOrDefault(c => c.Id_Field == id);

            if (parentCategory == null)
            {
                return new List<int>(); // Trả về danh sách rỗng nếu không tìm thấy danh mục cha
            }

            var result = new List<int>();
            GetSubCategoryIdRecursive(categories, parentCategory.Id_Field, result);
            return result;
        }

        private void GetSubCategoryIdRecursive(List<Category_fieldDTO> categories, int? parentId, List<int> result)

        {
            var subCategories = categories.Where(c => c.FielParentId == parentId).ToList();
            foreach (var sub in subCategories)
            {
                // Kiểm tra nếu Id_Field không null trước khi thêm vào result
                if (sub.Id_Field.HasValue)
                {
                    result.Add(sub.Id_Field.Value); // Sử dụng Value để lấy giá trị của Id_Field
                }
                GetSubCategoryIdRecursive(categories, sub.Id_Field, result); // Đệ quy lấy danh mục con
            }
        }

        public async Task<Category_fieldDTO> GetCategoryById(int id)
        {
            var namecategory = await _context.Categogy_Fields
                .Where(c => c.Id_Field == id)
                .Select(c => c.Name_Field) // Chỉ lấy trường Name của danh mục
                .FirstOrDefaultAsync();

            // Tạo đối tượng Category_fieldDTO và gán giá trị
            return new Category_fieldDTO
            {
                Name_Field = namecategory // Trả về đối tượng DTO chứa tên danh mục
            };
        }

    }
}
