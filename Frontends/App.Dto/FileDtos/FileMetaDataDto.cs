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
        public string FileName { get; set; }
        public string Description { get; set; }
        public int OwnerId { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadDate { get; set; }
        public FileVisibility Visibility { get; set; }
    }
}
