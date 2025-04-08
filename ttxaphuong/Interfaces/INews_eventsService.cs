using ttxaphuong.DTO.News_events;

namespace ttxaphuong.Interfaces
{
    public interface INews_eventsService
    {
        //Task<IEnumerable<News_eventsDTO>> GetNews_Events();

        Task<IEnumerable<News_eventsDTO>> GetAllNews_EventsAsync(bool? isVisible = null);
        Task<News_eventsDTO> CreateNews_EventsAsync(News_eventsDTO news_EventsDTO);
        Task<News_eventsDTO> UpdateNews_EventsAsync(int id, News_eventsDTO news_EventsDTO);
        Task<object> DeleteNews_EventsAsync(int id);
        Task<string> UploadImageAsync(IFormFile imageFile);
        Task<object> SetVisibility(int id, bool isVisible);

        /*********************************************************************/
        Task<News_eventsDTO> GetNews_EventsByNameAsync(string name);
        Task<List<News_eventsDTO>> GetNews_EventsByNameCategogyAsync(string nameCategogy);
        Task<List<News_eventsDTO>> GetTop5LatestNews_EventsAsync();
        Task<List<News_eventsDTO>> GetTop5LatestNews_EventsByCategoryAsync(string categoryName);
        Task<List<News_eventsDTO>> GetRelatedNews_EventsAsync(string title);
        Task<List<News_eventsDTO>> GetTopViewedNews(int count);
        Task<List<News_eventsDTO>> GetFeaturedNewsAsync();
    }
}
