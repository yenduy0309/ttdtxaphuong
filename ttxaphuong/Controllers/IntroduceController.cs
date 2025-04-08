using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ttxaphuong.DTO.Introduces;
using ttxaphuong.Interfaces;
using ttxaphuong.Services;

namespace ttxaphuong.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class IntroduceController : ControllerBase
    {
        private readonly IIntroduceService _introduceService;

        public IntroduceController(IIntroduceService introduceService)
        {
            _introduceService = introduceService;
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetIntroduce()
        {
            return Ok(await _introduceService.GetAllIntroduceAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Manager")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetIntroduceById(int id)
        {
            return Ok(await _introduceService.GetIntroduceByIdAsync(id));
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateIntroduce([FromBody] IntroduceDTO abc)
        {
            return Ok(await _introduceService.CreateIntroduceAsync(abc));
        }

        [HttpPost("upload-image")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UploadImage(IFormFile imageFile)
        {
            return Ok(new { ImagePath = await _introduceService.UploadImageIntroduceAsync(imageFile) });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateIntroduce(int id, [FromBody] IntroduceDTO abc)
        {
            return Ok(await _introduceService.UpdateIntroduceAsync(id, abc));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteIntroduce(int id)
        {
            return Ok(await _introduceService.DeleteIntroduceAsync(id));
        }

        /******************************************/
        [HttpGet("{categoryName}")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<ActionResult> GetIntroByNameCategogyAsync(string categoryName)
        {
            return Ok(await _introduceService.GetIntroByNameCategogyAsync(categoryName));
        }

        [HttpGet("{name}")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<ActionResult> GetIntroByNameAsync(string name)
        {
            return Ok(await _introduceService.GetIntroByNameAsync(name));
        }

        [HttpGet("{id}")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetRelatedIntroduce(int id)
        {
            return Ok(await _introduceService.GetRelatedIntroAsync(id));
        }
    }
}

