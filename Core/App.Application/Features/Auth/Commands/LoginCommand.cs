using App.Application.Dtos.AuthDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Features.Auth.Commands
{
    public class LoginCommand: IRequest<AuthTokenDto>
    {
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
    }
}
