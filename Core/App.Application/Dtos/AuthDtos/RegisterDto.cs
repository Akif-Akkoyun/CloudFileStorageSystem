using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Dtos.AuthDtos
{
    public class RegisterDto
    {
        public string UserName { get; set; } = default!;
        public string UserSurName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public int RoleId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class RegisterDTOValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDTOValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.UserSurName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Email).NotEmpty().MaximumLength(50);
            RuleFor(x => x.PasswordHash).NotEmpty().MaximumLength(255);
            RuleFor(x => x.CreatedAt).NotEmpty();
        }
    }
}
