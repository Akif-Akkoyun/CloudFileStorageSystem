using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Dto.AuthDtos
{
    public class UserListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string SurName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public string? RefreshPasswordToken { get; set; }
        public int RoleId { get; set; }
    }
    public class UserListDtoValidator : AbstractValidator<UserListDto>
    {
        public UserListDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.SurName).NotEmpty().WithMessage("SurName is required.");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.");
            RuleFor(x => x.PasswordHash).NotEmpty().WithMessage("PasswordHash is required.");
            RuleFor(x => x.RoleId).GreaterThan(0).WithMessage("RoleId must be greater than 0.");
        }
    }
}
