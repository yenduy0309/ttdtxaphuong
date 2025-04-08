using ttxaphuong.DTO.Introduces;

namespace ttxaphuong.Interfaces
{
    public interface ICategories_introduceService
    {
        Task<IEnumerable<Categories_introduceDTO>> GetAllCategories_introducesAsync();
        Task<Categories_introduceDTO> GetCategories_introducesByIdAsync(int id);
        Task<Categories_introduceDTO> CreateCategories_introducesAsync(Categories_introduceDTO categories_Introduce);
        Task<Categories_introduceDTO> UpdateCategories_introducesAsync(int id, Categories_introduceDTO categories_Introduce);
        Task<object> DeleteCategories_introducesAsync(int id);

        /*******************************************/
        Task<List<Categories_introduceDTO>> GetAllCategories(); // Lấy toàn bộ danh mục
        Task<Categories_introduceDTO> GetCategoryById(int id); // Lấy tên danh mục
    }
}
