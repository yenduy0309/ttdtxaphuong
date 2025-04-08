
namespace ttxaphuong.DTO.News_events
{
    public class News_eventsDTO
    {
        public int Id_newsevent { get; set; }
        public string? Title { get; set; }
        public string? Description_short { get; set; }
        public string? Content { get; set; }
        public int Id_account { get; set; }
        public string? Image { get; set; }
        public DateTime Create_at { get; set; } = DateTime.Now;
        public string? Formatted_content { get; set; }
        public int? View { get; set; }

        public int Id_categories { get; set; }
        public bool IsVisible { get; set; } = true; // Mặc định là hiển thị
    }
}
