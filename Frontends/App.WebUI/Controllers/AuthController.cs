using App.Dto.AuthDtos;
using App.WebUI.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace App.WebUI.Controllers
{
    public class AuthController(IHttpClientFactory _httpClientFactory, IMapper mapper) : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var mapperDto = mapper.Map<LoginViewModel, LoginDto>(model);
            if (mapperDto is null)
            {
                ViewBag.Error = "Veri işlenemedi.";
                return View(model);
            }

            var client = _httpClientFactory.CreateClient("GatewayAPI");
            var response = await client.PostAsJsonAsync("/api/auth/login", mapperDto);
            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Kullanıcı adı veya şifre hatalı.";
                return View(model);
            }

            var tokenDto = await response.Content.ReadFromJsonAsync<AuthTokenDto>();
            if (tokenDto is null)
            {
                ViewBag.Error = "Giriş yapılamadı, lütfen tekrar deneyin.";
                return View(model);
            }

            Response.Cookies.Append("auth-token", tokenDto.Token);
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Lütfen tüm alanları doğru şekilde doldurun.";
                return View(model);
            }

            var dto = new RegisterDto
            {
                Email = model.Email,
                UserName = model.Name,
                UserSurName = model.SurName,
                PasswordHash = model.Password,
            };

            if (dto is null)
            {
                ViewBag.Error = "Veri işlenemedi. Lütfen tekrar deneyin.";
                return View(model);
            }

            var client = _httpClientFactory.CreateClient("GatewayAPI");
            var response = await client.PostAsJsonAsync("/api/auth/register", dto);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Kayıt başarısız oldu. Bu email zaten kullanılıyor olabilir.";
                return View(model);
            }

            var registerResponse = await response.Content.ReadFromJsonAsync<RegisterDto>();

            if (registerResponse is null)
            {
                ViewBag.Error = "Sunucudan yanıt alınamadı. Lütfen tekrar deneyin.";
                return View(model);
            }

            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromForm] ForgotPasswordViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Lütfen geçerli bir e-posta adresi giriniz.";
                return View(viewModel);
            }

            var mapperDto = mapper.Map<ForgotPasswordViewModel, ForgotPasswordDto>(viewModel);

            if (mapperDto is null)
            {
                ViewBag.Error = "Veri işlenemedi. Lütfen tekrar deneyin.";
                return View(viewModel);
            }

            var client = _httpClientFactory.CreateClient("GatewayAPI");
            var response = await client.PostAsJsonAsync("/api/auth/forgot-password", mapperDto);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Mail gönderilemedi. Lütfen daha sonra tekrar deneyiniz.";
                return View(viewModel);
            }

            var forgotPasswordResponse = await response.Content.ReadFromJsonAsync<ForgotPasswordDto>();

            if (forgotPasswordResponse is null)
            {
                ViewBag.Error = "Sunucudan yanıt alınamadı.";
                return View(viewModel);
            }
            TempData["Success"] = "E-posta adresinize şifre sıfırlama bağlantısı gönderildi.";
            return RedirectToAction("ForgotPassword");
        }

        [Route("/renew-password/{verificationCode}")]
        [HttpGet]
        public IActionResult RenewPassword([FromRoute] string verificationCode)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(verificationCode))
            {
                return RedirectToAction(nameof(ForgotPassword));
            }

            return View(new RenewPasswordViewModel
            {
                Email = string.Empty,
                Token = verificationCode,
                Password = string.Empty,
                ConfirmPassword = string.Empty,
            });
        }
        [Route("/renew-password/{verificationCode}")]
        [HttpPost]
        public IActionResult RenewPassword([FromForm] RenewPasswordViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var dto = new ResetPasswordDto
            {
                Email = viewModel.Email,
                Token = viewModel.Token,
                PasswordHash = viewModel.Password,
                PasswordRepeat = viewModel.ConfirmPassword,
            };
            if (dto is null)
            {
                return View(viewModel);
            }
            var client = _httpClientFactory.CreateClient("GatewayAPI");
            var response = client.PostAsJsonAsync("/api/auth/renew-password", dto);
            if (!response.Result.IsSuccessStatusCode)
            {
                return View(viewModel);
            }
            var renewPasswordResponse = response.Result.Content.ReadFromJsonAsync<ResetPasswordDto>();
            if (renewPasswordResponse == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid code or email");
                return View(viewModel);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("auth-token");
            return RedirectToAction("Index", "Home");
        }
    }
}
