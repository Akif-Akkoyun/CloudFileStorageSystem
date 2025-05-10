using Ardalis.Result;
using MediatR;

namespace App.Application.Features.Auth.Commands
{
    public class ForgotPasswordCommand : IRequest<Result>
    {
        public string Email { get; set; } = string.Empty;
    }
}
