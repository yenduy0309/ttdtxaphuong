using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ttxaphuong.Models.News_events;

namespace ttxaphuong.Models.Loads
{
    public class FolderModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id_folder { get; set; }

        public string? Name_folder { get; set; }

        public int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public FolderModel? Folder { get; set; } 
        public List<FolderModel> Children { get; set; } = new List<FolderModel>(); // Danh sách thư mục con
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
