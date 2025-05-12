using App.Dto.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Dto.FileDtos
{
    public class FileMetaDataDto
    {
        public string FileName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int OwnerId { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; }
        public FileVisibility Visibility { get; set; }
    }
    public class FileMetaDataDtoValidator : AbstractValidator<FileMetaDataDto>
    {
        public FileMetaDataDtoValidator()
        {
            RuleFor(x => x.FileName).NotEmpty().WithMessage("File name is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
            RuleFor(x => x.OwnerId).NotEmpty().WithMessage("Owner ID is required.");
            RuleFor(x => x.FilePath).NotEmpty().WithMessage("File path is required.");
            RuleFor(x => x.UploadDate).NotEmpty().WithMessage("Upload date is required.");
            RuleFor(x => x.Visibility).IsInEnum().WithMessage("Invalid visibility value.");
        }
    }
}