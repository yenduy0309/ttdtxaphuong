using ttxaphuong.DTO.News_events;

namespace ttxaphuong.Interfaces
{
    public interface ICategoriesService
    {
        Task<IEnumerable<CategoriesDTO>> GetAllCategoriesAsync();
        Task<CategoriesDTO> GetCategoriesByIdAsync(int id);
        Task<CategoriesDTO> CreateCategoriesAsync(CategoriesDTO categories);
        Task<CategoriesDTO> UpdateCategoriesAsync(int id, CategoriesDTO categories);
        
        Task<object> DeleteCategoriesAsync(List<int> ids); // Xóa nhiều danh mục
        Task<object> DeleteCategoryAsync(int id); // Xóa từng danh mục

        /*******************************************//*******************************************/
        Task<List<CategoriesDTO>> GetAllCategories(); // Lấy toàn bộ danh mục
        Task<CategoriesDTO> GetCategoryById(int id); // Lấy tên danh mục
        Task<List<CategoriesDTO>> GetCategoryHierarchy(); // Lấy danh mục theo cấp bậc
        Task<List<CategoriesDTO>> BuildCategoryTree(List<CategoriesDTO> categories, int? parentId);

        //lấy tên dm con từ danh mục cha
        Task<List<string>> GetAllSubCategoryNamesByNameAsync(string parentName);
    }
}
