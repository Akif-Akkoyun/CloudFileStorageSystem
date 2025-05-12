using App.Dto.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Dto.FileDtos
{
    public class FileDetailDto
    {
        public int Id { get; set; }
        public string FileName { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string FilePath { get; set; } = default!;
        public DateTime UploadDate { get; set; }
        public FileVisibility Visibility { get; set; }
        public string? OwnerName { get; set; }
    }
    public class FileDetailDtoValidator : AbstractValidator<FileDetailDto>
    {
        public FileDetailDtoValidator()
        {
            RuleFor(x => x.FileName).NotEmpty().WithMessage("File name is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
            RuleFor(x => x.FilePath).NotEmpty().WithMessage("File path is required.");
            RuleFor(x => x.UploadDate).NotEmpty().WithMessage("Upload date is required.");
            RuleFor(x => x.Visibility).IsInEnum().WithMessage("Visibility must be a valid enum value.");
        }
    }
}
