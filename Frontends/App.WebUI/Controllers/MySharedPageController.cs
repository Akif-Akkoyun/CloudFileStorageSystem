using App.Dto.AuthDtos;
using App.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using AutoMapper;
using App.Dto.FileDtos;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;

namespace App.WebUI.Controllers
{
    [Authorize(Roles = "User")]
    public class MySharedPageController(IHttpClientFactory _httpClientFactory, IMapper _mapper) : Controller
    {
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
        [HttpGet]
        public async Task<IActionResult> ShareFile(int id)
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
        [HttpPost]
        public async Task<IActionResult> ShareFile(ShareFileViewModel model)
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
        [HttpGet]
        public async Task<IActionResult> SharedByMe()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized();
            var client = _httpClientFactory.CreateClient("GatewayAPI");
            var token = Request.Cookies["auth-token"];
            if (string.IsNullOrWhiteSpace(token))
                return RedirectToAction("Login", "Auth");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync($"/api/files/shared-by-me/{userId}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Paylaştığınız dosyalar alınamadı.";
                return View(new List<SharedByMeViewModel>());
            }
            var dtoList = await response.Content.ReadFromJsonAsync<List<SharedByMeDto>>() ?? new();
            var viewModelList = _mapper.Map<List<SharedByMeViewModel>>(dtoList);
            foreach (var file in viewModelList)
            {
                foreach (var sharedUserId in file.SharedWithUsers)
                {
                    var userResponse = await client.GetAsync($"/api/auth/users/{sharedUserId}");
                    if (userResponse.IsSuccessStatusCode)
                    {
                        var userDto = await userResponse.Content.ReadFromJsonAsync<UserListDto>();
                        if (userDto != null)
                            file.SharedWithUserNames.Add(userDto.Name);
                        else
                            file.SharedWithUserNames.Add("Bilinmiyor");
                    }
                    else
                    {
                        file.SharedWithUserNames.Add("Bilinmiyor");
                    }
                }
            }
            return View(viewModelList);
        }
        [HttpGet]
        public async Task<IActionResult> MyPageDetailsPartial(int id)
        {
            var client = _httpClientFactory.CreateClient("GatewayAPI");
            var token = Request.Cookies["auth-token"];

            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.GetAsync($"/api/files/get-by/{id}");
            if (!response.IsSuccessStatusCode || response.Content.Headers.ContentLength == 0)
            {
                return Content("<div class='text-danger p-3'>Dosya getirilemedi.</div>", "text/html");
            }

            var dto = await response.Content.ReadFromJsonAsync<FileDetailDto>();
            if (dto == null)
            {
                return Content("<div class='text-danger p-3'>Dosya bulunamadı.</div>", "text/html");
            }

            var viewModel = _mapper.Map<FileDetailViewModel>(dto);
            return PartialView("_FileDetailModalPartial", viewModel);
        }
    }
}
