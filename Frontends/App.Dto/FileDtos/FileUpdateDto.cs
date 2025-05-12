using App.Dto.Enums;
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
}
