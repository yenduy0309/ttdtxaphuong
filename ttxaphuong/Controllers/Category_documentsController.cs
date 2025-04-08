using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ttxaphuong.DTO.Documents;
using ttxaphuong.Interfaces;
using ttxaphuong.Services;

namespace ttxaphuong.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class Category_documentsController : Controller
    {
        private readonly ICategory_documentsService _categoryDocumentsService;
        public Category_documentsController(ICategory_documentsService category_documentsService)
        {
            _categoryDocumentsService = category_documentsService;
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<ActionResult<IEnumerable<Category_documentsDTO>>> GetCategory_documents()
        {
            return Ok(await _categoryDocumentsService.GetCategory_documentsAsync());
        }

        [HttpGet("{id}")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetCategory_documentsById(int id)
        {
            return Ok(await _categoryDocumentsService.GetCategory_documentsByIdAsync(id));
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateCategory_documents([FromBody] Category_documentsDTO categories_documents)
        {
            return Ok(await _categoryDocumentsService.CreateCategory_documentsAsync(categories_documents));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateCategory_documents(int id, [FromBody] Category_documentsDTO categories_documents)
        {
            return Ok(await _categoryDocumentsService.UpdateCategory_documentsAsync(id, categories_documents));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteCategory_documents(int id)
        {
            return Ok(await _categoryDocumentsService.DeleteCategory_documentsAsync(id));
        }

        //**************************************User***********************************//
        //Phân cấp danh mục 
        [HttpGet("")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetCat_DocHierarchy()
        {
            var catdocument = await _categoryDocumentsService.GetCat_DocHierarchy();
            return Ok(catdocument);
        }

        [HttpGet("{parentName}")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetAllSubCat_DocumentNamesByName(string parentName)
        {
            var subCat_DocNames = await _categoryDocumentsService.GetAllSubCat_DocNamesByNameAsync(parentName);
            return Ok(subCat_DocNames);
        }

        //lấy tên category từ id category
        [HttpGet("{id}")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryDocumentsService.GetCategoryById(id);
            return Ok(category);
        }
    }
}
