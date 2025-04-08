using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ttxaphuong.DTO.Feedbacks;
using ttxaphuong.Interfaces;

namespace ttxaphuong.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class FeedbacksController : ControllerBase
    {
        private readonly IFeedbacksService _feedbacksService;

        public FeedbacksController(IFeedbacksService feedbacksService)
        {
            _feedbacksService = feedbacksService;
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetFeedbacks()
        {
            return Ok(await _feedbacksService.GetAllFeedbacksAsync());
        }

        [HttpPut("ApproveFeedback/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ApproveFeedback(int id)
        {
            var feedback = await _feedbacksService.ApproveFeedbackAsync(id);
            if (feedback == null)
            {
                return NotFound("Không tìm thấy phản hồi.");
            }
            return Ok(new { message = "Phản hồi đã được duyệt", feedback });
        }

        [HttpPut("RejectFeedback/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> RejectFeedback(int id)
        {
            var feedback = await _feedbacksService.RejectFeedbackAsync(id);
            if (feedback == null)
            {
                return NotFound("Không tìm thấy phản hồi.");
            }
            return Ok(new { message = "Phản hồi đã bị từ chối", feedback });
        }

        [HttpGet("GetFeedbackStatistics")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetFeedbackStatistics()
        {
            return Ok(await _feedbacksService.GetFeedbackStatisticsAsync());
        }

        [HttpPost]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> CreateFeedback([FromBody] FeedbacksDTO feedbacksDTO)
        {
            return Ok(await _feedbacksService.CreateFeedbackAsync(feedbacksDTO));
        }
    }
}
