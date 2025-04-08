using ttxaphuong.DTO.Uploads;

namespace ttxaphuong.Interfaces
{
    public interface IUploadFilePdfService
    {
        Task<IEnumerable<PostPdfDTO>> GetAllPostPdfAsync();
        Task<PostPdfDTO> MovePdfAsync(int pdfId, int targetFolderId);
        Task<PostPdfDTO> UpdatePostPdfAsync(int id, PostPdfDTO postPdf);
        Task<object> DeletePostPdfAsync(int id);
        Task<PostPdfDTO> UploadPdfAsync(IFormFile file, int? folderId);
    }
}
