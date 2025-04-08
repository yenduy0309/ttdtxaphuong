using ttxaphuong.DTO.News_events;

namespace ttxaphuong.Interfaces
{
    public interface IStatisticsService
    {
        Task<int> GetStatistics();
        Task<int> GetTotalNewsEventsAsync();
        Task<int> GetTotalDocumentsAsync();
        Task<int> GetTotalProceduresAsync();
        Task<int> GetTotalIntroducesAsync();
        Task<List<object>> GetNewsViewsByCategoryAsync();
        Task<List<object>> GetNewsViewsOverTimeAsync();
    }
}
