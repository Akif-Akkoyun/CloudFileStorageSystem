using App.Dto.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Dto.FileDtos
{
    public class FileUpdateDto
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? FilePath { get; set; }
        public FileVisibility Visibility { get; set; }
    }
    public class FileUpdateDtoValidator : AbstractValidator<FileUpdateDto>
    {
        public FileUpdateDtoValidator()
        {
            RuleFor(x => x.FileName)
                .NotEmpty()
                .WithMessage("File name is required.")
                .MaximumLength(100)
                .WithMessage("File name cannot exceed 100 characters.");
            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description cannot exceed 500 characters.");
            RuleFor(x => x.Visibility)
                .IsInEnum()
                .WithMessage("Invalid visibility option.");
        }
    }
}
