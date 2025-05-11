using App.Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Dtos.FileDtos
{
    public class FileCreateDto
    {
        public string FileName { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string FilePath { get; set; } = default!;
        public int OwnerId { get; set; }
        public FileVisibility Visibility { get; set; } = FileVisibility.Private;
        public List<int>? SharedUserIds { get; set; }
    }
    public class  FileValidator :AbstractValidator<FileCreateDto>
    {
        public FileValidator()
        {
            RuleFor(x => x.FileName).NotEmpty().MinimumLength(3);
            RuleFor(x => x.FilePath).NotEmpty().MinimumLength(3);
            RuleFor(x => x.Description).NotEmpty().MinimumLength(10);
            RuleFor(x => x.OwnerId).NotEmpty();
            RuleFor(x => x.Visibility).IsInEnum();
        }
    }
}
