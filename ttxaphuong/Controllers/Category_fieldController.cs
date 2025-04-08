using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ttxaphuong.DTO.Category_field;
using ttxaphuong.Interfaces;

namespace ttxaphuong.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class Category_fieldController : Controller
    {
        private readonly ICategory_fieldService _categoryfieldService;
        public Category_fieldController(ICategory_fieldService categoryfieldService)
        {
            _categoryfieldService = categoryfieldService;
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<ActionResult<IEnumerable<Category_fieldDTO>>> GetCategory_field()
        {
            return Ok(await _categoryfieldService.GetAllCategory_fieldAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Manager")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetCategory_fieldById(int id)
        {
            return Ok(await _categoryfieldService.GetCategory_fieldByIdAsync(id));
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateCategory_field([FromBody] Category_fieldDTO category_Field)
        {
            return Ok(await _categoryfieldService.CreateCategory_fieldAsync(category_Field));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateCategory_field(int id, [FromBody] Category_fieldDTO category_Field)
        {
            return Ok(await _categoryfieldService.UpdateCategory_fieldAsync(id, category_Field));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteCategory_field(int id)
        {
            return Ok(await _categoryfieldService.DeleteCategory_fieldAsync(id));
        }

        /************************************User**************************************/
        [HttpGet]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<ActionResult<IEnumerable<Category_fieldDTO>>> GetAllCategories()
        {
            return Ok(await _categoryfieldService.GetAllCategoriesAsync());
        }

        [HttpGet("")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetCategoryHierarchy()
        {
            var categories = await _categoryfieldService.GetCategoryHierarchy();
            return Ok(categories);
        }

        [HttpGet("{parentName}")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetAllSubCategoryNamesByName(string parentName)
        {
            var subCategoryNames = await _categoryfieldService.GetAllSubCategoryNamesByNameAsync(parentName);
            return Ok(subCategoryNames);
        }

        [HttpGet("{id}")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetAllSubCategoryIdById(int id)
        {
            var subCategoryId = await _categoryfieldService.GetAllSubCategoryIdByIdAsync(id);
            return Ok(subCategoryId);
        }

        //lấy tên category từ id category
        [HttpGet("{id}")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryfieldService.GetCategoryById(id);
            return Ok(category);
        }

    }
}
