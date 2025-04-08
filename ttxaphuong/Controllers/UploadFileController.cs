using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ttxaphuong.DTO.News_events;
using ttxaphuong.DTO.Uploads;
using ttxaphuong.Interfaces;
using ttxaphuong.Services;

namespace ttxaphuong.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class UploadFileController : ControllerBase
    {
        private readonly IUploadFileService _uploadFileService;

        public UploadFileController(IUploadFileService uploadFileService)
        {
            _uploadFileService = uploadFileService;
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetImages()
        {
            return Ok(await _uploadFileService.GetAllPostImageAsync());
        }

        [HttpPut("MoveImage/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> MoveImage(int id, [FromQuery] int targetFolderId)
        {
            try
            {
                var result = await _uploadFileService.MoveImageAsync(id, targetFolderId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdatePostImage(int id, [FromBody] PostImageDTO postImageDTO)
        {
            return Ok(await _uploadFileService.UpdatePostImageAsync(id, postImageDTO));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeletePostImage(int id)
        {
            return Ok(await _uploadFileService.DeletePostImageAsync(id));
        }

        [HttpPost("UploadImage")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<PostImageDTO>> UploadImage(IFormFile file, [FromForm] int? folderId)
        {
            try
            {
                var result = await _uploadFileService.UploadImageAsync(file, folderId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
