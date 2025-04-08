using ttxaphuong.DTO.Uploads;

namespace ttxaphuong.Interfaces
{
    public interface IFolderPdfService
    {
        Task<IEnumerable<FolderPdfDTO>> GetAllFolderPdfAsync();
        Task<FolderPdfDTO> GetFolderPdfByIdAsync(int id); // Lấy tên danh mục
        Task<FolderPdfDTO> CreateFolderPdfAsync(FolderPdfDTO folder);
        Task<FolderPdfDTO> UpdateFolderPdfAsync(int id, FolderPdfDTO folder);
        Task<object> DeleteFolderPdfAsync(int id); // Xóa từng danh mục
    }
}
