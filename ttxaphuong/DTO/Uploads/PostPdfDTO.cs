namespace ttxaphuong.DTO.Uploads
{
    public class PostPdfDTO
    {
        public int? Id_pdf { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public int Id_folder_pdf { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.Now;
    }
}
