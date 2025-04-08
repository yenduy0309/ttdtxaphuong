using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ttxaphuong.Models.Loads
{
    public class PostPdfModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Pdf { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public int Id_folder_pdf { get; set; }
        [ForeignKey("Id_folder_pdf")]
        public FolderPdfModel? FolderPdfModel { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.Now;
    }
}
