using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ttxaphuong.DTO.Introduces;
using ttxaphuong.Interfaces;

namespace ttxaphuong.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class Categories_introduceController : ControllerBase
    {
        private readonly ICategories_introduceService _categoriesIntroducesService;

        public Categories_introduceController(ICategories_introduceService categoriesIntroducesService)
        {
            _categoriesIntroducesService = categoriesIntroducesService;
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetCategories_introduces()
        {
            return Ok(await _categoriesIntroducesService.GetAllCategories_introducesAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Manager")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetCategories_introducesById(int id)
        {
            return Ok(await _categoriesIntroducesService.GetCategories_introducesByIdAsync(id));
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateCategories_introduces([FromBody] Categories_introduceDTO categories_Introduce)
        {
            return Ok(await _categoriesIntroducesService.CreateCategories_introducesAsync(categories_Introduce));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateCategories_introduces(int id, [FromBody] Categories_introduceDTO categories_Introduce)
        {
            return Ok(await _categoriesIntroducesService.UpdateCategories_introducesAsync(id, categories_Introduce));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteCategories_introduces(int id)
        {
            return Ok(await _categoriesIntroducesService.DeleteCategories_introducesAsync(id));
        }

        //*************************************************************************//
        [HttpGet]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<ActionResult<IEnumerable<Categories_introduceDTO>>> GetCategories()
        {
            return Ok(await _categoriesIntroducesService.GetAllCategories());
        }

        //lấy tên category từ id category
        [HttpGet("{id}")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoriesIntroducesService.GetCategoryById(id);
            return Ok(category);
        }
    }
}
