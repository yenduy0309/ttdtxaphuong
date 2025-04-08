using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ttxaphuong.DTO.News_events;
using ttxaphuong.Interfaces;

namespace ttxaphuong.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService _categoryService;
        public CategoriesController(ICategoriesService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        //[Authorize(Roles = "Manager")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<ActionResult<IEnumerable<CategoriesDTO>>> GetCategories()
        {
            return Ok(await _categoryService.GetAllCategoriesAsync());
        }

        [HttpGet("{id}")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetCategoriesById(int id)
        {
            return Ok(await _categoryService.GetCategoriesByIdAsync(id));
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateCategories([FromBody] CategoriesDTO categories)
        {
            return Ok(await _categoryService.CreateCategoriesAsync(categories));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateCategories(int id, [FromBody] CategoriesDTO categories)
        {
            return Ok(await _categoryService.UpdateCategoriesAsync(id, categories));
        }

        // Xóa từng danh mục
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteCategory(int id) 
        {
            return Ok(await _categoryService.DeleteCategoryAsync(id));
        }

        // Xóa nhiều danh mục
        [HttpDelete]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteCategories([FromBody] List<int> ids) 
        {
            return Ok(await _categoryService.DeleteCategoriesAsync(ids));
        }

        /************************************User**************************************/
        [HttpGet("")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetCategoryHierarchy()
        {
            var categories = await _categoryService.GetCategoryHierarchy();
            return Ok(categories);
        }
        [HttpGet("{parentName}")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetAllSubCategoryNamesByName(string parentName)
        {
            var subCategoryNames = await _categoryService.GetAllSubCategoryNamesByNameAsync(parentName);
            return Ok(subCategoryNames);
        }

        //lấy tên category từ id category
        [HttpGet("{id}")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryById(id);
            return Ok(category);
        }

    }
}
