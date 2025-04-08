using ttxaphuong.DTO.News_events;
using ttxaphuong.DTO.Uploads;

namespace ttxaphuong.Interfaces
{
    public interface IFolderService
    {
        Task<IEnumerable<FolderDTO>> GetAllFolderAsync();
        Task<FolderDTO> GetFolderByIdAsync(int id); // Lấy tên danh mục
        Task<FolderDTO> CreateFolderAsync(FolderDTO folder);
        Task<FolderDTO> UpdateFolderAsync(int id, FolderDTO folder);
        Task<object> DeleteFolderAsync(int id); // Xóa từng danh mục
    }
}
