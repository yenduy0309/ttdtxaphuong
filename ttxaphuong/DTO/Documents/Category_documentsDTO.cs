namespace ttxaphuong.DTO.Documents
{
    public class Category_documentsDTO
    {
        public int? Id_category_document { get; set; } 
        public string? Name_category_document { get; set; } 
        public int? DocumentParentId { get; set; }
        public List<Category_documentsDTO> Children { get; set; } = new List<Category_documentsDTO>();
    }
}
