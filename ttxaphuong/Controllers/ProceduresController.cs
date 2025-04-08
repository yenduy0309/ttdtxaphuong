using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ttxaphuong.DTO.News_events;
using ttxaphuong.DTO.Procedures;
using ttxaphuong.Interfaces;
using ttxaphuong.Services;

namespace ttxaphuong.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class ProceduresController : Controller
    {
        private readonly IProceduresService _proceduresService;

        public ProceduresController(IProceduresService proceduresService)
        {
            _proceduresService = proceduresService;
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetProcedures([FromQuery] bool? isVisible = null)
        {
            return Ok(await _proceduresService.GetAllProceduresAsync(isVisible));
        }

        [HttpPut("SetVisibility/{id}")]
        [Authorize(Roles = "Manager")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> SetVisibility(int id, [FromBody] bool isVisible)
        {
            return Ok(await _proceduresService.SetVisibility(id, isVisible));
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateProcedures([FromBody] ProceduresDTO proceduresDTO)
        {
            return Ok(await _proceduresService.CreateProceduresAsync(proceduresDTO));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetProceduresById(int id)
        {
            return Ok(await _proceduresService.GetProceduresByIdAsync(id));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateProcedures(int id, [FromBody] ProceduresDTO proceduresDTO)
        {
            return Ok(await _proceduresService.UpdateProceduresAsync(id, proceduresDTO));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteProcedures(int id)
        {
            return Ok(await _proceduresService.DeleteProceduresAsync(id));
        }

        /************************************/
        [HttpGet("{id}")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetProceduresByIdField(int id)
        {
            return Ok(await _proceduresService.GetProceduresByIdField(id));
        }

        [HttpGet("{id_thutuc}")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetProceduresById_thutuc(string id_thutuc)
        {
            return Ok(await _proceduresService.GetProceduresById_thutuc(id_thutuc));
        }
    }
}

