
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ttxaphuong.Models.News_events
{
    public class CategoriesModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_categories { get; set; }

        public string? Name_category { get; set; }

        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public CategoriesModel? Categories { get; set; }
        public List<CategoriesModel> Children { get; set; } = new List<CategoriesModel>();
    }
}
