
namespace ttxaphuong.DTO.Uploads
{
    public class FolderDTO
    {
        public int? Id_folder { get; set; }
        public string? Name_folder { get; set; }
        public int? ParentId { get; set; }
        public List<FolderDTO> Children { get; set; } = new List<FolderDTO>(); // Danh sách thư mục con
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
