using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Dtos.AuthDtos
{
    public class ForgotPasswordDto
    {
        public string Email { get; set; } = string.Empty;
    }
    public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordDto>
    {
        public ForgotPasswordCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
