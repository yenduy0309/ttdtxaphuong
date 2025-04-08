namespace ttxaphuong.DTO.News_events
{
    public class CategoriesDTO
    {
        public int? Id_categories { get; set; }
        public string? Name_category { get; set; }
        public int? ParentId { get; set; }
        public List<CategoriesDTO> Children { get; set; } = new List<CategoriesDTO>();
    }
}
