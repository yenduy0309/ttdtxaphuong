using ttxaphuong.DTO.News_events;
using ttxaphuong.DTO.Uploads;

namespace ttxaphuong.Interfaces
{
    public interface IUploadFileService
    {
        Task<IEnumerable<PostImageDTO>> GetAllPostImageAsync();
        Task<PostImageDTO> MoveImageAsync(int imageId, int targetFolderId);
        Task<PostImageDTO> UpdatePostImageAsync(int id, PostImageDTO postImageDTO);
        Task<object> DeletePostImageAsync(int id);
        Task<PostImageDTO> UploadImageAsync(IFormFile file, int? folderId);

    }
}
