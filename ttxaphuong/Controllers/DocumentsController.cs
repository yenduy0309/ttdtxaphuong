using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ttxaphuong.DTO.Documents;
using ttxaphuong.Interfaces;

namespace ttxaphuong.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class DocumentsController : Controller
    {
        private readonly IDocumentsService _documentsService;
        public DocumentsController(IDocumentsService documentsService)
        {
            _documentsService = documentsService;
        }

        [HttpGet]
        //[Authorize(Roles = "Manager")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetDocuments([FromQuery] bool? isVisible = null)
        {
            return Ok(await _documentsService.GetAllDocumentsAsync(isVisible));
        }


        [HttpPut("SetVisibility/{id}")]
        //[Authorize(Roles = "Manager")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> SetVisibility(int id, [FromBody] bool isVisible)
        {
            return Ok(await _documentsService.SetVisibility(id, isVisible));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Manager")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetDocumentsById(int id)
        {
            return Ok(await _documentsService.GetDocumentsByIdAsync(id));
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateDocuments([FromBody] DocumentsDTO documents)
        {
            return Ok(await _documentsService.CreateDocumentsAsync(documents));
        }

        [HttpPost("UploadPdfDocument")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UploadPdfDocument(IFormFile pdfFile)
        {
            return Ok(new { filePath = await _documentsService.UploadPdfDocumentsAsync(pdfFile) });
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateDocuments(int id, [FromBody] DocumentsDTO documents)
        {
            return Ok(await _documentsService.UpdateDocumentsAsync(id, documents));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteDocuments(int id)
        {
            return Ok(await _documentsService.DeleteDocumentsAsync(id));
        }

        //*************************************User******************************************//
        [HttpGet("{name}")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetDocByName(string name)
        {
            return Ok(await _documentsService.GetDocByNameAsync(name));
        }

        [HttpGet("{nameCategogy}")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetDocByNameCategory(string nameCategogy)
        {
            return Ok(await _documentsService.GetDocByNameCategogyAsync(nameCategogy));
        }

        [HttpGet("featured")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<ActionResult<List<DocumentsDTO>>> GetFeaturedDoc()
        {
            try
            {
                var featuredDoc = await _documentsService.GetFeaturedDocAsync();
                return Ok(featuredDoc);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetTop5LatestDoc()
        {
            return Ok(await _documentsService.GetTop5LatestDocAsync());
        }

        [HttpGet("{categoryId}")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetTop5LatestDocByCategory(int categoryId)
        {
            var result = await _documentsService.GetTop5LatestDocByCategoryAsync(categoryId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetRelatedDoc(int id)
        {
            return Ok(await _documentsService.GetRelatedDocAsync(id));
        }

        [HttpGet("{id}")]
        [AllowAnonymous] // Cho phép đăng nhập mà không cần xác thực
        public async Task<IActionResult> GetDocumentFile(int id)
        {
            try
            {
                var filePath = await _documentsService.GetDocumentFilePathAsync(id);
                if (filePath == null)
                {
                    return NotFound("Không tìm thấy tài liệu.");
                }

                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                var fileName = Path.GetFileName(filePath);

                // Xác định loại MIME dựa trên phần mở rộng của file
                var fileExtension = Path.GetExtension(fileName).ToLower();
                string mimeType;

                switch (fileExtension)
                {
                    case ".pdf":
                        mimeType = "application/pdf";
                        break;
                    case ".docx":
                        mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        break;
                    case ".doc":
                        mimeType = "application/msword";
                        break;
                    case ".txt":
                        mimeType = "text/plain";
                        break;
                    default:
                        mimeType = "application/octet-stream"; // Loại mặc định cho các file không xác định
                        break;
                }

                return File(fileBytes, mimeType, fileName);
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi khi lấy tài liệu: {ex.Message}");
            }
        }
    }
}
