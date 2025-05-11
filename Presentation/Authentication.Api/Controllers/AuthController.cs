using App.Application.Dtos.AuthDtos;
using App.Application.Features.Auth.Commands;
using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            try
            {
                var tokenDto = await _mediator.Send(command);
                Response.Cookies.Append("access_token", tokenDto.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(30)
                });
                return Ok(tokenDto);
            }
            catch (UnauthorizedAccessException ex)
            {
                ModelState.AddModelError("Login", ex.Message);
                return Unauthorized(ModelState);
            }
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (registerDto is null)
            {
                return BadRequest("Geçersiz veri");
            }
            var result = await _mediator.Send(new RegisterCommand(registerDto));
            if (result is null)
            {
                return BadRequest("Kullanıcı oluşturulamadı");
            }
            return Ok(new { result.Id, result.Name, result.Email });
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var command = new ForgotPasswordCommand
            {
                Email = dto.Email
            };
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            if (result.Status == ResultStatus.NotFound)
            {
                return NotFound(result);
            }
            return BadRequest(result);
        }

        [HttpPost("renew-password")]
        public async Task<IActionResult> RenewPassword([FromBody] ResetPasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var command = new ResetPasswordCommand
            {
                Email = dto.Email,
                Token = dto.Token,
                PasswordHash = dto.PasswordHash,
                PasswordRepeat = dto.PasswordRepeat
            };
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            if (result.Status == ResultStatus.NotFound)
            {
                return NotFound(result);
            }
            return BadRequest(result);
        }
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("access_token");
            return Ok(new { message = "Çıkış yapıldı" });
        }
    }
}