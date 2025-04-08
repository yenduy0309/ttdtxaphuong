using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ttxaphuong.DTO.News_events;
using ttxaphuong.DTO.Uploads;
using ttxaphuong.Interfaces;

namespace ttxaphuong.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class FolderController : Controller
    {
        private readonly IFolderService _folderService;
        public FolderController(IFolderService folderService)
        {
            _folderService = folderService;
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<IEnumerable<FolderDTO>>> GetFolder()
        {
            return Ok(await _folderService.GetAllFolderAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetFolderById(int id)
        {
            return Ok(await _folderService.GetFolderByIdAsync(id));
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateFolder([FromBody] FolderDTO folder)
        {
            return Ok(await _folderService.CreateFolderAsync(folder));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateFolder(int id, [FromBody] FolderDTO folder)
        {
            return Ok(await _folderService.UpdateFolderAsync(id, folder));
        }

        // Xóa từng danh mục
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteFolder(int id)
        {
            return Ok(await _folderService.DeleteFolderAsync(id));
        }
    }
}

