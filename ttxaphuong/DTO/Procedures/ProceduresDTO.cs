namespace ttxaphuong.DTO.Procedures
{
    public class ProceduresDTO
    {
        public int Id_procedures { get; set; }
        public string? Id_thutuc { get; set; }
        public string? Name_procedures { get; set; }
        public int Id_Field { get; set; }
        public string? Description { get; set; }

        public DateTime? Date_issue { get; set; }
        public DateTime? Create_at { get; set; }
        public int Id_account { get; set; }

        public string? FormatText { get; set; }
        public bool IsVisible { get; set; } = true; // Mặc định là hiển thị
    }
}
