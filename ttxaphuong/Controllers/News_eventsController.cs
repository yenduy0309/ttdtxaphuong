using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ttxaphuong.DTO.News_events;
using ttxaphuong.Interfaces;

namespace ttxaphuong.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class News_eventsController : Controller
    {
        private readonly INews_eventsService _newsEventsService;

        public News_eventsController(INews_eventsService newsEventsService)
        {
            _newsEventsService = newsEventsService;
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetNews_Events([FromQuery] bool? isVisible = null)
        {
            return Ok(await _newsEventsService.GetAllNews_EventsAsync(isVisible));
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateNews_Events([FromBody] News_eventsDTO news_EventsDTO)
        {
            return Ok(await _newsEventsService.CreateNews_EventsAsync(news_EventsDTO));
        }

        [HttpPost("upload-image")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UploadImage(IFormFile imageFile)
        {
            return Ok(new { ImagePath = await _newsEventsService.UploadImageAsync(imageFile) });
        }

        [HttpPut("SetVisibility/{id}")]
        [Authorize(Roles = "Manager")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> SetVisibility(int id, [FromBody] bool isVisible)
        {
            return Ok(await _newsEventsService.SetVisibility(id, isVisible));
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateNews_Events(int id, [FromBody] News_eventsDTO news_EventsDTO)
        {
            return Ok(await _newsEventsService.UpdateNews_EventsAsync(id, news_EventsDTO));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteNews_Events(int id)
        {
            return Ok(await _newsEventsService.DeleteNews_EventsAsync(id));
        }

        /********************************************Phần User*****************************************/
        [HttpGet("{name}")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetNews_EventsByName(string name)
        {
            return Ok(await _newsEventsService.GetNews_EventsByNameAsync(name));
        }

        [HttpGet("{nameCategogy}")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetNews_EventsByNameCategory(string nameCategogy)
        {
            return Ok(await _newsEventsService.GetNews_EventsByNameCategogyAsync(nameCategogy));
        }

        [HttpGet("top-viewed/{count}")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetTopViewedNews(int count)
        {
            var result = await _newsEventsService.GetTopViewedNews(count);
            return Ok(result);
        }


        [HttpGet("featured")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<ActionResult<List<News_eventsDTO>>> GetFeaturedNews()
        {
            try
            {
                var featuredNews = await _newsEventsService.GetFeaturedNewsAsync();
                return Ok(featuredNews);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetTop5LatestNews_Events()
        {
            return Ok(await _newsEventsService.GetTop5LatestNews_EventsAsync());
        }

        [HttpGet("{categoryName}")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thựce]
        public async Task<IActionResult> GetTop5LatestNews_EventsByCategory(string categoryName)
        {
            var result = await _newsEventsService.GetTop5LatestNews_EventsByCategoryAsync(categoryName);
            return Ok(result);
        }

        //y
        [HttpGet("{title}")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetRelatedNews_Events(string title)
        {
            return Ok(await _newsEventsService.GetRelatedNews_EventsAsync(title));
        }
    }
}
