using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileStorage.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoragesController : ControllerBase
    {
        private readonly string _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        private readonly ILogger<StoragesController> _logger;
        public StoragesController(ILogger<StoragesController> logger)
        {
            _logger = logger;
            if (!Directory.Exists(_storagePath))
                Directory.CreateDirectory(_storagePath);
        }
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("File upload attempted with empty or null file.");
                return BadRequest("Dosya bulunamadı.");
            }
            _logger.LogInformation("File upload requested: {OriginalFileName}", file.FileName);
            var originalFileName = file.FileName.Replace(" ", "_").Replace(")", "").Replace("(", "").ToLower();
            var fileExtension = Path.GetExtension(originalFileName);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
            var fileName = originalFileName;
            var fullPath = Path.Combine(_storagePath, fileName);
            int count = 1;
            while (System.IO.File.Exists(fullPath))
            {
                fileName = $"{fileNameWithoutExtension}-{count}{fileExtension}";
                fullPath = Path.Combine(_storagePath, fileName);
                count++;
            }
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            _logger.LogInformation("File saved successfully: {SavedFileName}", fileName);
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var filePath = $"{baseUrl}/uploads/{fileName}";
            _logger.LogInformation("Generated file path: {FilePath}", filePath);
            return Ok(new { filePath });
        }
        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> Download(string fileName)
        {
            _logger.LogInformation("File download requested: {FileName}", fileName);
            var fullPath = Path.Combine(_storagePath, fileName);
            if (!System.IO.File.Exists(fullPath))
            {
                _logger.LogWarning("File not found for download: {FileName}", fileName);
                return NotFound("Dosya bulunamadı.");
            }
            var memory = new MemoryStream();
            using (var stream = new FileStream(fullPath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/octet-stream", fileName);
        }
    }
}