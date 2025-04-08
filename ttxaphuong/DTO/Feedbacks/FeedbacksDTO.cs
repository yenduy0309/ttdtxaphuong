namespace ttxaphuong.DTO.Feedbacks
{
    public class FeedbacksDTO
    {
        public int? Id_feedbacks { get; set; }
        public string? Fullname { get; set; }
        public string? Email { get; set; }
        public string? Content { get; set; }
        public DateTime? Received_at { get; set; }
        public string? Phone { get; set; }
        public string? Status { get; set; } = "Pending"; // Giá trị mặc định là Chờ duyệt
    }
}
