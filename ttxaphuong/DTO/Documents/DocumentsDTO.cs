
namespace ttxaphuong.DTO.Documents
{
    public class DocumentsDTO
    {
        public int? Id_document { get; set; }
        public string? Title { get; set; }
        public string? File_path { get; set; }
        public string? Description_short { get; set; }
        public string? Description { get; set; }
        public DateTime? Create_at { get; set; }
        public int Id_account { get; set; }
        public int? View_documents { get; set; }

        /**************************************/
        //public int? CategoriesDocumentModelId { get; set; }
        public int Id_category_document { get; set; }
        public bool IsVisible { get; set; } = true; // Mặc định là hiển thị

    }
}
