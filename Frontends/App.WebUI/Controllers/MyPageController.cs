using App.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using App.Dto.FileDtos;
using AutoMapper;
using System.Text.Json;
using App.Dto.AuthDtos;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security;
using App.Dto.Enums;

namespace App.WebUI.Controllers
{
    [Authorize(Roles = "User")]
    public class MyPageController(IHttpClientFactory _httpClientFactory,IMapper _mapper) : Controller
    {
        public async Task<IActionResult> MyFiles()
        {
            var client = _httpClientFactory.CreateClient("GatewayAPI");
            var token = Request.Cookies["auth-token"];
            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Auth");

            var response = await client.GetAsync($"/api/files/owner/{userId}");
            if (!response.IsSuccessStatusCode)
                return View("Error");

            var files = await response.Content.ReadFromJsonAsync<List<MyPageListViewModel>>();
            return View(files);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("GatewayAPI");
            var token = Request.Cookies["auth-token"];
            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.DeleteAsync($"/api/files/remove/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Dosya silinirken bir hata oluştu.";
            }
            else
            {
                TempData["Success"] = "Dosya başarıyla silindi.";
            }

            return RedirectToAction("MyFiles");
        }
        [HttpPost]
        public async Task<IActionResult> ToggleVisibility(int id)
        {
            var client = _httpClientFactory.CreateClient("GatewayAPI");
            var token = Request.Cookies["auth-token"];
            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.PutAsync($"/api/files/toggle-visibility/{id}", null);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Görünürlük değiştirilemedi.";
            }
            else
            {
                TempData["Success"] = "Görünürlük başarıyla güncellendi.";
            }

            return RedirectToAction("MyFiles");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var client = _httpClientFactory.CreateClient("GatewayAPI");

            var token = Request.Cookies["auth-token"];
            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.GetAsync($"/api/files/get-by/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Dosya bilgisi alınamadı.";
                return RedirectToAction("MyFiles");
            }

            var dto = await response.Content.ReadFromJsonAsync<FileDetailDto>();
            if (dto is null)
            {
                TempData["Error"] = "Dosya bulunamadı.";
                return RedirectToAction("MyFiles");
            }

            var viewModel = _mapper.Map<FileUpdateViewModel>(dto);
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Update(FileUpdateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient("GatewayAPI");
            var token = Request.Cookies["auth-token"];
            if (!string.IsNullOrWhiteSpace(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string? newFilePath = null;

            if (model.NewFile != null)
            {
                using var content = new MultipartFormDataContent();
                content.Add(new StreamContent(model.NewFile.OpenReadStream()), "file", model.NewFile.FileName);

                var uploadResponse = await client.PostAsync("/api/storages/upload", content);
                if (!uploadResponse.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Dosya yüklenemedi.";
                    return View(model);
                }

                var responseContent = await uploadResponse.Content.ReadAsStringAsync();
                var uploadResult = JsonSerializer.Deserialize<FileUploadResponseDto>(responseContent,new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                newFilePath = uploadResult?.FilePath;
            }

            var dto = _mapper.Map<FileUpdateDto>(model);
            if (!string.IsNullOrEmpty(newFilePath))
                dto.FilePath = newFilePath;

            var updateResponse = await client.PutAsJsonAsync("/api/files/update", dto);
            if (!updateResponse.IsSuccessStatusCode)
            {
                TempData["Error"] = "Güncelleme başarısız.";
                return View(model);
            }

            TempData["Success"] = "Dosya başarıyla güncellendi.";
            return RedirectToAction("MyFiles");
        }        
    }
}