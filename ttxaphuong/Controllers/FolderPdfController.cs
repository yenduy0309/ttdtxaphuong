using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ttxaphuong.DTO.Uploads;
using ttxaphuong.Interfaces;

namespace ttxaphuong.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class FolderPdfController : Controller
    {
        private readonly IFolderPdfService _folderPdfService;
        public FolderPdfController(IFolderPdfService folderPdfService)
        {
            _folderPdfService = folderPdfService;
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<IEnumerable<FolderPdfDTO>>> GetFolderPdf()
        {
            return Ok(await _folderPdfService.GetAllFolderPdfAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetFolderPdfById(int id)
        {
            return Ok(await _folderPdfService.GetFolderPdfByIdAsync(id));
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        //[AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> CreateFolderPdf([FromBody] FolderPdfDTO folder)
        {
            return Ok(await _folderPdfService.CreateFolderPdfAsync(folder));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateFolderPdf(int id, [FromBody] FolderPdfDTO folder)
        {
            return Ok(await _folderPdfService.UpdateFolderPdfAsync(id, folder));
        }

        // Xóa từng danh mục
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteFolderPdf(int id)
        {
            return Ok(await _folderPdfService.DeleteFolderPdfAsync(id));
        }
    }
}


