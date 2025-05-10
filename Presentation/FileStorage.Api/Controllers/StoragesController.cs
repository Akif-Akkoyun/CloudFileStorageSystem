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

        [HttpPost("/upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Dosya bulunamadı.");

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var fullPath = Path.Combine(_storagePath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { FileName = fileName, FilePath = $"uploads/{fileName}" });
        }

        [HttpGet("/download")]
        public async Task<IActionResult> Download([FromQuery] string fileName)
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
