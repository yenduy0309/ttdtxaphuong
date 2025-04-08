using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ttxaphuong.Models.Loads
{
    public class PostImageModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Image { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public int Id_folder { get; set; }
        [ForeignKey("Id_folder")]
        public FolderModel? FolderModel { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.Now;
    }
}
