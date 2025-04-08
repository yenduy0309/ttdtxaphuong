using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ttxaphuong.Models.Accounts;

namespace ttxaphuong.Models.Documents
{
    public class DocumentModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_document { get; set; }
        public string? Title { get; set; }

        public string? File_path { get; set; }
        public string? Description_short { get; set; }
        public string? Description { get; set; }
        public DateTime Create_at { get; set; }
        public int Id_account { get; set; }
        [ForeignKey("Id_account")]
        public AccountsModel Accounts { get; set; }
        public int? View_documents { get; set; }

        // Thêm quan hệ với Category_documentsModel
        public int Id_category_document { get; set; }

        [ForeignKey("Id_category_document")]
        public Category_documentsModel? Category_Documents { get; set; }
        public bool IsVisible { get; set; } = true; // Mặc định là hiển thị
    }
}
