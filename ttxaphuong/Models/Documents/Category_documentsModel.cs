using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ttxaphuong.Models.Documents
{
    public class Category_documentsModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_category_document { get; set; }

        public string? Name_category_document { get; set; }

        public int? DocumentParentId { get; set; }
        [ForeignKey("DocumentParentId")]
        public Category_documentsModel? Category_Documents { get; set; }
        public List<Category_documentsModel> Children { get; set; } = new List<Category_documentsModel>();

    }
}
