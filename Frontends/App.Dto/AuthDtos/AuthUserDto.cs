using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Dto.AuthDtos
{
    public class AuthUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
    public class AuthUserListDtoValidator : AbstractValidator<AuthUserDto>
    {
        public AuthUserListDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }
}
