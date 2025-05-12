using App.Dto.Enums;
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
}
