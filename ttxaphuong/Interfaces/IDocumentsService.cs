
using ttxaphuong.DTO.Documents;
using ttxaphuong.DTO.News_events;

namespace ttxaphuong.Interfaces
{
    public interface IDocumentsService
    {
        //Task<IEnumerable<DocumentsDTO>> GetDocuments1();
        Task<IEnumerable<DocumentsDTO>> GetAllDocumentsAsync(bool? isVisible = null);
        Task<object> SetVisibility(int id, bool isVisible);
        Task<DocumentsDTO> GetDocumentsByIdAsync(int id);
        Task<DocumentsDTO> CreateDocumentsAsync(DocumentsDTO documents);
        Task<DocumentsDTO> UpdateDocumentsAsync(int id, DocumentsDTO documents);
        Task<string> UploadPdfDocumentsAsync(IFormFile pdfFile);
        Task<object> DeleteDocumentsAsync(int id);

        /**************************************//************************************/
        Task<DocumentsDTO> GetDocByNameAsync(string name);
        Task<List<DocumentsDTO>> GetDocByNameCategogyAsync(string nameCategogy);
        Task<List<DocumentsDTO>> GetTop5LatestDocAsync();
        Task<List<DocumentsDTO>> GetTop5LatestDocByCategoryAsync(int categoryId);
        Task<List<DocumentsDTO>> GetRelatedDocAsync(int id);
        Task<List<DocumentsDTO>> GetFeaturedDocAsync();
        Task<string> GetDocumentFilePathAsync(int id);
    }
}
