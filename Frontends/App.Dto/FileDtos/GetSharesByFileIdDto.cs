using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Dto.FileDtos
{
    public class GetSharesByFileIdDto
    {
        public int UserId { get; set; }
        public string Permission { get; set; } = "";
    }
    public class  GetSaresByFileIdDtoValidator : AbstractValidator<GetSharesByFileIdDto>
    {
        public GetSaresByFileIdDtoValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");
            RuleFor(x => x.Permission).NotEmpty().WithMessage("Permission is required.");
        }
    }
}
