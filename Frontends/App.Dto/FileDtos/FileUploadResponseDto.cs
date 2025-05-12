using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Dto.FileDtos
{
    public class FileUploadResponseDto
    {
        public string FilePath { get; set; } = string.Empty;
    }
    public class FileUploadResponseDtoValidator : AbstractValidator<FileUploadResponseDto>
    {
        public FileUploadResponseDtoValidator()
        {
            RuleFor(x => x.FilePath).NotEmpty().WithMessage("File path cannot be empty.");
        }
    }
}