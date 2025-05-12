using App.Dto.FileDtos;
using App.WebUI.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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
            {
                return View(viewModel);
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();
            var client = _httpClientFactory.CreateClient("GatewayAPI");
            var token = Request.Cookies["auth-token"];
            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            using var content = new MultipartFormDataContent();
            if (viewModel.File == null || viewModel.File.Length == 0)
            {
                TempData["Error"] = "Lütfen bir dosya seçiniz.";
                return RedirectToAction("Upload");
            }
            content.Add(new StreamContent(viewModel.File.OpenReadStream()), "file", viewModel.File.FileName);
            var uploadResponse = await client.PostAsync("/api/storages/upload", content);
            if (!uploadResponse.IsSuccessStatusCode)
            {
                TempData["Error"] = "Dosya yüklenemedi. Lütfen tekrar deneyin.";
                return RedirectToAction("Upload");
            }
            var uploadResult = await uploadResponse.Content.ReadFromJsonAsync<FileUploadResponseDto>();
            var filePath = uploadResult?.FilePath;
            if (string.IsNullOrEmpty(filePath))
            {
                TempData["Error"] = "Dosya yüklenemedi. Lütfen tekrar deneyin.";
                return RedirectToAction("Upload");
            }
            var dto = _mapper.Map<FileMetaDataDto>(viewModel);
            dto.FileName = viewModel.File.FileName;
            dto.Description = viewModel.Description;
            dto.FilePath = filePath;
            dto.OwnerId = int.Parse(userId);
            dto.UploadDate = DateTime.UtcNow;
            dto.Visibility = (Dto.Enums.FileVisibility)viewModel.Visibility;
            var metaResponse = await client.PostAsJsonAsync("/api/files/create", dto);
            if (!metaResponse.IsSuccessStatusCode)
            {
                return View("Error");
            }
            TempData["Success"] = "Dosya başarıyla yüklendi.";
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Details([FromRoute] int id)
        {
            var client = _httpClientFactory.CreateClient("GatewayAPI");
            var token = Request.Cookies["auth-token"];
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await client.GetAsync($"/api/files/get-by/{id}");
            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                TempData["Error"] = "Bu dosyanın detaylarını görmeye yetkiniz yok.";
                return RedirectToAction("Index", "Home");
            }
            if (!response.IsSuccessStatusCode || response.Content.Headers.ContentLength == 0)
            {
                TempData["Error"] = "Dosya verisi alınamadı.";
                return RedirectToAction("Index", "Home");
            }
            if (!response.IsSuccessStatusCode)
            {
                return View("Error");
            }
            var dto = await response.Content.ReadFromJsonAsync<FileDetailDto>();
            if (dto is null)
            {
                TempData["Error"] = "Dosya bulunamadı.";
                return RedirectToAction("Index", "Home");
            }
            var viewModel = _mapper.Map<FileDetailViewModel>(dto);
            if(viewModel is null)
            {
                TempData["Error"] = "Dosya detayları alınamadı.";
                return RedirectToAction("Index", "Home");
            }
            return View(viewModel);
        }
        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> Download([FromRoute]int id)
        {
            var client = _httpClientFactory.CreateClient("GatewayAPI");
            var token = Request.Cookies["auth-token"];
            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var metaResponse = await client.GetAsync($"/api/files/get-by/{id}");
            if (!metaResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            var fileMeta = await metaResponse.Content.ReadFromJsonAsync<FileDetailDto>();
            if (fileMeta == null || string.IsNullOrEmpty(fileMeta.FilePath))
            {
                return RedirectToAction("Index", "Home");
            }
            var fileName = Path.GetFileName(fileMeta.FilePath);
            var fileResponse = await client.GetAsync($"/api/storages/download/{fileName}");
            if (!fileResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            var fileBytes = await fileResponse.Content.ReadAsByteArrayAsync();
            var contentType = fileResponse.Content.Headers.ContentType?.MediaType ?? "application/octet-stream";
            return File(fileBytes, contentType, fileName);
        }        
    }
}