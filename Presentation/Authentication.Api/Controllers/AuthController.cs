using App.Application.Auth.Features.Auth.Queries;
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
        private readonly ILogger<AuthController> _logger;

        public AuthController(IMediator mediator, ILogger<AuthController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            _logger.LogInformation("Login attempt for email: {Email}", command.Email);
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

                _logger.LogInformation("Login successful for user with email: {Email}", command.Email);
                return Ok(tokenDto);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Unauthorized login attempt: {Message}", ex.Message);
                ModelState.AddModelError("Login", ex.Message);
                return Unauthorized(ModelState);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (registerDto is null)
            {
                _logger.LogWarning("Register attempt with null data");
                return BadRequest("Geçersiz veri");
            }

            var result = await _mediator.Send(new RegisterCommand(registerDto));
            if (result is null)
            {
                _logger.LogError("User registration failed for email: {Email}", registerDto.Email);
                return BadRequest("Kullanıcı oluşturulamadı");
            }

            _logger.LogInformation("New user registered with email: {Email}", result.Email);
            return Ok(new { result.Id, result.Name, result.Email });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ForgotPassword validation failed for email: {Email}", dto.Email);
                return BadRequest(ModelState);
            }

            var command = new ForgotPasswordCommand { Email = dto.Email };
            var result = await _mediator.Send(command);

            _logger.LogInformation("ForgotPassword requested for email: {Email}, Status: {Status}", dto.Email, result.Status);
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
                _logger.LogWarning("RenewPassword validation failed for email: {Email}", dto.Email);
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
            _logger.LogInformation("RenewPassword attempted for email: {Email}, Status: {Status}", dto.Email, result.Status);

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
            _logger.LogInformation("User logged out.");
            return Ok(new { message = "Çıkış yapıldı" });
        }

        [HttpGet("users/{ownerId}")]
        public async Task<IActionResult> Users([FromRoute] int ownerId)
        {
            _logger.LogInformation("Fetching user with ID: {OwnerId}", ownerId);
            var userList = await _mediator.Send(new GetUserByIdQuery(ownerId));
            return Ok(userList);
        }

        [HttpGet("all-users")]
        public async Task<IActionResult> AllUser()
        {
            _logger.LogInformation("Fetching all users");
            var userList = await _mediator.Send(new GetAllUsersQuery());
            return Ok(userList);
        }
    }
}