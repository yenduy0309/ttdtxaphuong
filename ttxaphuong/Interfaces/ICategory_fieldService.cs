using ttxaphuong.DTO.Category_field;
using ttxaphuong.DTO.News_events;

namespace ttxaphuong.Interfaces
{
    public interface ICategory_fieldService
    {
        Task<IEnumerable<Category_fieldDTO>> GetAllCategory_fieldAsync();
        Task<Category_fieldDTO> GetCategory_fieldByIdAsync(int id);
        Task<Category_fieldDTO> CreateCategory_fieldAsync(Category_fieldDTO category_Field);
        Task<Category_fieldDTO> UpdateCategory_fieldAsync(int id, Category_fieldDTO category_Field);
        Task<object> DeleteCategory_fieldAsync(int id);


        /*******************************************//*******************************************/
        Task<List<Category_fieldDTO>> GetAllCategoriesAsync(); // Lấy toàn bộ danh mục
        Task<List<Category_fieldDTO>> GetCategoryHierarchy(); // Lấy danh mục theo cấp bậc
        Task<List<Category_fieldDTO>> BuildCategoryTree(List<Category_fieldDTO> categories, int? parentId);

        //lấy tên dm con từ danh mục cha
        Task<List<string>> GetAllSubCategoryNamesByNameAsync(string parentName);
        Task<List<int>> GetAllSubCategoryIdByIdAsync(int id);
        Task<Category_fieldDTO> GetCategoryById(int id); // Lấy tên danh mục
    }
}
