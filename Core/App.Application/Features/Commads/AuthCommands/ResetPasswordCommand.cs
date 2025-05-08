using Ardalis.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Features.Commads.AuthCommands
{
    public class ResetPasswordCommand : IRequest<Result>
    {
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string PasswordRepeat { get; set; } = null!;
    }
}
