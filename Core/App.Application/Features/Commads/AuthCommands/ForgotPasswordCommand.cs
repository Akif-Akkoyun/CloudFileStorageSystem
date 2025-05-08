using Ardalis.Result;
using MediatR;

namespace App.Application.Features.Commads.AuthCommands
{
    public class ForgotPasswordCommand : IRequest<Result>
    {
        public string Email { get; set; } = string.Empty;
    }
}
