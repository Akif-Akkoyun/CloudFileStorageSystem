using App.Application.Dtos.AuthDtos;
using App.Application.Features.Commads.AuthCommands;
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
        [HttpPost("/login")]
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
        [HttpPost("/register")]
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
        [HttpGet("/forgot-password")]
        public IActionResult ForgotPassword()
        {
            return Ok("Forgot Password");
        }       
        
        [HttpGet("/refresh-token")]
        public IActionResult RefreshToken()
        {
            return Ok("Refresh Token");
        }
        [HttpGet("/logout")]
        public IActionResult Logout()
        {
            return Ok("Logout");
        }
    }
}
