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
        public Visibility Visibility { get; set; }
        public string OwnerName { get; set; } = default!;
    }
}
