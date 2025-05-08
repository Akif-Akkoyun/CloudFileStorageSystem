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
        [HttpGet("/register")]
        public IActionResult Register()
        {
            return Ok("Register");
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
