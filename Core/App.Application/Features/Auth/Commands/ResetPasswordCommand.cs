using Ardalis.Result;
using MediatR;

namespace App.Application.Features.Auth.Commands
{
    public class ResetPasswordCommand : IRequest<Result>
    {
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string PasswordRepeat { get; set; } = null!;
    }
}
