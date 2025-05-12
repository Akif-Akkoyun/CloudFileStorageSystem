using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Dto.AuthDtos
{
    public class RegisterDto
    {
        public string UserName { get; set; } = default!;
        public string UserSurName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public int RoleId { get; set; }
    }
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required.");
            RuleFor(x => x.UserSurName).NotEmpty().WithMessage("UserSurName is required.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Valid Email is required.");
            RuleFor(x => x.PasswordHash).NotEmpty().WithMessage("Password is required.");
            RuleFor(x => x.RoleId).GreaterThan(0).WithMessage("RoleId must be greater than 0.");
        }
    }
}
