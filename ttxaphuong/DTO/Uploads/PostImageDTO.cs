
namespace ttxaphuong.DTO.Uploads
{
    public class PostImageDTO
    {
        public int? Id_Image { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public int Id_folder { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.Now;
    }
}
