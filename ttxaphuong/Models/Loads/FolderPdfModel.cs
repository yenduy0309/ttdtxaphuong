using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ttxaphuong.Models.Loads
{
    public class FolderPdfModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id_folder_pdf { get; set; }

        public string? Name_folder { get; set; }

        public int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public FolderPdfModel? FolderPdf { get; set; }
        public List<FolderPdfModel> Children { get; set; } = new List<FolderPdfModel>(); // Danh sách thư mục con
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
