using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ttxaphuong.Models.Accounts;

namespace ttxaphuong.Models.News_events
{
    public class News_eventsModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_newsevent { get; set; }
        public string? Title { get; set; }
        public string? Description_short { get; set; }
        public string? Content { get; set; }
        public int Id_account { get; set; }
        [ForeignKey("Id_account")]
        public AccountsModel Accounts { get; set; }
        public string? Image { get; set; }
        public DateTime Create_at { get; set; } = DateTime.Now;
        public string? Formatted_content { get; set; }
        public int? View { get; set; }
        public int Id_categories { get; set; }

        [ForeignKey("Id_categories")]
        public CategoriesModel? CategoriesModel { get; set; }
        public bool IsVisible { get; set; } = true; // Mặc định là hiển thị
    }
}
