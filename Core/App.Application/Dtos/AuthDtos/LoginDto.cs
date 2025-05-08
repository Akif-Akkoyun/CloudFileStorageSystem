using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Dtos.AuthDtos
{
    public class LoginDto
    {
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
    }
    public class LoginCommandValidator : AbstractValidator<LoginDto>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.PasswordHash).NotEmpty().MinimumLength(6);
        }
    }
}
