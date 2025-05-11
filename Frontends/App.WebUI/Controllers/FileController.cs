using App.Dto.FileDtos;
using App.WebUI.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace App.WebUI.Controllers
{
    public class FileController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;
        public FileController(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
        }
        [Authorize(Roles ="User")]
        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] FileUploadViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();
            // 1. Dosya fiziksel olarak kaydedilsin
            var client = _httpClientFactory.CreateClient("GatewayAPI");
            var token = Request.Cookies["auth-token"];
            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            Console.WriteLine("Token -> " + token);
            
            using var content = new MultipartFormDataContent();
            content.Add(new StreamContent(viewModel.File.OpenReadStream()), "file", viewModel.File.FileName);

            var uploadResponse = await client.PostAsync("/api/storages/upload", content);
            if (!uploadResponse.IsSuccessStatusCode)
            {
                TempData["Error"] = "Dosya yüklenemedi. Lütfen tekrar deneyin.";
                return RedirectToAction("Upload");
            }               

            var filePath = await uploadResponse.Content.ReadAsStringAsync();

            var dto = _mapper.Map<FileMetaDataDto>(viewModel);
            dto.Name = viewModel.File.FileName;
            dto.FilePath = filePath;
            dto.OwnerId = int.Parse(userId);
            dto.UploadDate = DateTime.UtcNow;

            var metaResponse = await client.PostAsJsonAsync("/api/files/create", dto);
            if (!metaResponse.IsSuccessStatusCode)
                return View("Error");

            TempData["Success"] = "Dosya başarıyla yüklendi.";
            return RedirectToAction("Index");
        }
    }
}
