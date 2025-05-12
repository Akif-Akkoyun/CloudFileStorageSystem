using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Dto.FileDtos
{
    public class SharedByMeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string FilePath { get; set; } = default!;
        public string Visibility { get; set; } = default!;
        public List<int> SharedWithUsers { get; set; } = new();
    }
    public class ShareByMeDtoValidator : AbstractValidator<SharedByMeDto>
    {
        public ShareByMeDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("File name is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
            RuleFor(x => x.FilePath).NotEmpty().WithMessage("File path is required.");
            RuleFor(x => x.Visibility).NotEmpty().WithMessage("Visibility is required.");
        }
    }
}
