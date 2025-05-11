using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileStorage.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoragesController : ControllerBase
    {
        private readonly string _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        public StoragesController()
        {
            if (!Directory.Exists(_storagePath))
                Directory.CreateDirectory(_storagePath);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Dosya bulunamadı.");
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
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var filePath = $"{baseUrl}/uploads/{fileName}";

            return Ok(new { filePath });
        }
        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> Download(string fileName)
        {
            var fullPath = Path.Combine(_storagePath, fileName);
            if (!System.IO.File.Exists(fullPath))
                return NotFound("Dosya bulunamadı.");

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
