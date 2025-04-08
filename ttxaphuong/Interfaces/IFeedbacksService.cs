using ttxaphuong.DTO.Feedbacks;

namespace ttxaphuong.Interfaces
{
    public interface IFeedbacksService
    {
        Task<IEnumerable<FeedbacksDTO>> GetAllFeedbacksAsync();
        Task<FeedbacksDTO?> ApproveFeedbackAsync(int id);
        Task<FeedbacksDTO?> RejectFeedbackAsync(int id);
        Task<object> GetFeedbackStatisticsAsync();
        Task<FeedbacksDTO> CreateFeedbackAsync(FeedbacksDTO feedbacksDTO);
    }
}
