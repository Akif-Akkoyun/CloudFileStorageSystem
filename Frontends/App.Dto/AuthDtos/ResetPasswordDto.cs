using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Dto.AuthDtos
{
    public class ResetPasswordDto
    {
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string PasswordRepeat { get; set; } = null!;
    }
    public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Token).NotEmpty();
            RuleFor(x => x.PasswordHash).NotEmpty().MinimumLength(6);
            RuleFor(x => x.PasswordRepeat).Equal(x => x.PasswordHash);
        }
    }
}
