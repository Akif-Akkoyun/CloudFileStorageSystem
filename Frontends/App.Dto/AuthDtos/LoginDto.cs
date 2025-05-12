using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Dto.AuthDtos
{
    public class LoginDto
    {
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
    }
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(x => x.PasswordHash)
                .NotEmpty();
        }
    }
}
