using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ttxaphuong.DTO.Uploads;
using ttxaphuong.Interfaces;

namespace ttxaphuong.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class UploadFilePdfController : ControllerBase
    {
        private readonly IUploadFilePdfService _uploadFilePdfService;

        public UploadFilePdfController(IUploadFilePdfService uploadFilePdfService)
        {
            _uploadFilePdfService = uploadFilePdfService;
        }

        [HttpGet]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetPdf()
        {
            return Ok(await _uploadFilePdfService.GetAllPostPdfAsync());
        }

        [HttpPut("MovePdf/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> MovePdf(int id, [FromQuery] int targetFolderId)
        {
            try
            {
                var result = await _uploadFilePdfService.MovePdfAsync(id, targetFolderId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdatePostPdf(int id, [FromBody] PostPdfDTO postPdfDTO)
        {
            return Ok(await _uploadFilePdfService.UpdatePostPdfAsync(id, postPdfDTO));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeletePostPdf(int id)
        {
            return Ok(await _uploadFilePdfService.DeletePostPdfAsync(id));
        }

        [HttpPost("UploadPdf")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<PostPdfDTO>> UploadPdf(IFormFile file, [FromForm] int? folderId)
        {
            try
            {
                var result = await _uploadFilePdfService.UploadPdfAsync(file, folderId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}

