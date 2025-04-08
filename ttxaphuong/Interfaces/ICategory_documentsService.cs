using ttxaphuong.DTO.Documents;

namespace ttxaphuong.Interfaces
{
    public interface ICategory_documentsService
    {
        Task<IEnumerable<Category_documentsDTO>> GetCategory_documentsAsync();
        Task<Category_documentsDTO> GetCategory_documentsByIdAsync(int id);
        Task<Category_documentsDTO> CreateCategory_documentsAsync(Category_documentsDTO categories_documents);
        Task<Category_documentsDTO> UpdateCategory_documentsAsync(int id, Category_documentsDTO categories_documents);
        Task<object> DeleteCategory_documentsAsync(int id);

        /*******************************************//*******************************************/
        Task<List<Category_documentsDTO>> GetAllCategories(); // Lấy toàn bộ danh mục
        Task<List<Category_documentsDTO>> GetCat_DocHierarchy(); // Lấy danh mục theo cấp bậc
        Task<List<Category_documentsDTO>> BuildCat_DocTree(List<Category_documentsDTO> categories, int? parentId);

        //lấy tên dm con từ danh mục cha
        Task<List<string>> GetAllSubCat_DocNamesByNameAsync(string parentName);
        Task<Category_documentsDTO> GetCategoryById(int id); // Lấy tên danh mục

    }
}
