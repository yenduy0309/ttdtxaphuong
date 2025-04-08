namespace ttxaphuong.DTO.Uploads
{
    public class FolderPdfDTO
    {
        public int? Id_folder_pdf { get; set; }
        public string? Name_folder { get; set; }
        public int? ParentId { get; set; }
        public List<FolderPdfDTO> Children { get; set; } = new List<FolderPdfDTO>(); // Danh sách thư mục con
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
