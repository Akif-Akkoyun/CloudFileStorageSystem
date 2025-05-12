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
    public class MyPageController(IHttpClientFactory _httpClientFactory,IMapper _mapper) : Controller
    {
        [Authorize(Roles = "User")]
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
        [Authorize(Roles = "User")]
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
        [Authorize(Roles = "User")]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> SharedWithMe()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var client = _httpClientFactory.CreateClient("GatewayAPI");
            var token = Request.Cookies["auth-token"];
            if (string.IsNullOrWhiteSpace(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"/api/files/shared-with-me/{userId}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Paylaşılan dosyalar alınamadı.";
                return View(new List<SharedWithMeViewModel>());
            }

            var dtoList = await response.Content.ReadFromJsonAsync<List<SharedWithMeDto>>() ?? new();
            var viewModelList = _mapper.Map<List<SharedWithMeViewModel>>(dtoList);

            foreach (var file in viewModelList)
            {
                var userResponse = await client.GetAsync($"/api/auth/users/{file.OwnerId}");
                if (userResponse.IsSuccessStatusCode)
                {
                    var owner = await userResponse.Content.ReadFromJsonAsync<UserListDto>();
                    file.OwnerName = owner?.Name ?? "Bilinmiyor";
                }
                else
                {
                    file.OwnerName = "Bilinmiyor";
                }
            }
            return View(viewModelList);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> SharedFileDetails(int id)
        {
            var client = _httpClientFactory.CreateClient("GatewayAPI");
            var token = Request.Cookies["auth-token"];
            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync($"/api/files/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Dosya detayına ulaşılamadı.";
                return RedirectToAction("SharedWithMe");
            }
            var dto = await response.Content.ReadFromJsonAsync<FileDetailDto>();
            var viewModel = _mapper.Map<FileDetailViewModel>(dto);
            return View(viewModel);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Share(int id)
        {
            var client = _httpClientFactory.CreateClient("GatewayAPI");
            var token = Request.Cookies["auth-token"];
            if (!string.IsNullOrWhiteSpace(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var userResponse = await client.GetAsync("/api/auth/all-users");
            if (!userResponse.IsSuccessStatusCode)
                return View("Error");

            var users = await userResponse.Content.ReadFromJsonAsync<List<AuthUserDto>>() ?? new();

            var sharedUserIds = new List<int>();
            var sharedResponse = await client.GetAsync($"/api/files/get-shares-by-file/{id}");
            if (sharedResponse.IsSuccessStatusCode)
            {
                var shares = await sharedResponse.Content.ReadFromJsonAsync<List<GetSharesByFileIdDto>>() ?? new();
                sharedUserIds = shares.Select(s => s.UserId).ToList();
            }
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var model = new ShareFileViewModel
            {
                Id = id,
                Permission = "ReadOnly",
                AllUsers = users
                    .Where(u => u.Id != currentUserId && !sharedUserIds.Contains(u.Id))
                    .Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }).ToList()
            };
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Share(ShareFileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient("GatewayAPI");
            var token = Request.Cookies["auth-token"];
            if (string.IsNullOrWhiteSpace(token))
                return RedirectToAction("Login", "Auth");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (string.IsNullOrWhiteSpace(model.Permission))
            {
                TempData["Error"] = "Paylaşım yetkisi seçilmedi.";
                return View(model);
            }

            var dto = new ShareFileDto
            {
                FileId = model.Id,
                SharedWithUserIds = model.SelectedUserIds,
                Permission = model.Permission
            };

            var response = await client.PostAsJsonAsync("/api/files/share-file", dto);
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("API Hatası: " + errorContent);
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Paylaşım başarısız oldu.";
                return View(model);
            }

            TempData["Success"] = "Dosya başarıyla paylaşıldı.";
            return RedirectToAction("MyFiles", "MyPage");
        }
    }
}