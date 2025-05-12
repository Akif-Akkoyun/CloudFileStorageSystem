using App.Dto.Enums;
using FluentValidation;

namespace App.Dto.FileDtos
{
    public class SharedWithMeDto
    {
        public int Id { get; set; }
        public string FileName { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string FilePath { get; set; } = default!;
        public FileVisibility Visibility { get; set; }
        public DateTime UploadDate { get; set; }
        public int OwnerId { get; set; }
        public string Permission { get; set; } = "";
    }
    public class SharedWithMeDtoValidator : AbstractValidator<SharedWithMeDto>
    {
        public SharedWithMeDtoValidator()
        {
            RuleFor(x => x.FileName)
                .NotEmpty()
                .WithMessage("File name is required.")
                .MaximumLength(255)
                .WithMessage("File name must not exceed 255 characters.");
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required.")
                .MaximumLength(500)
                .WithMessage("Description must not exceed 500 characters.");
            RuleFor(x => x.FilePath)
                .NotEmpty()
                .WithMessage("File path is required.")
                .MaximumLength(500)
                .WithMessage("File path must not exceed 500 characters.");
        }
    }
}
