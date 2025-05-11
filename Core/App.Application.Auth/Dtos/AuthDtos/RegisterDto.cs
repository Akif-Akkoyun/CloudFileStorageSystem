using App.Application.Features.Auth.Commands;
using FluentValidation;
namespace App.Application.Dtos.AuthDtos
{
    public class RegisterDto
    {
        public string UserName { get; set; } = default!;
        public string UserSurName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public int RoleId { get; set; }
    }
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.RegisterDto.Email)
                .NotEmpty().WithMessage("Email boş olamaz.")
                .EmailAddress().WithMessage("Geçersiz email.");

            RuleFor(x => x.RegisterDto.PasswordHash)
                .NotEmpty().WithMessage("Şifre boş olamaz.");
        }
    }
}
