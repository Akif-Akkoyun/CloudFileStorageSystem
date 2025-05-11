using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Dto.FileDtos
{
    public class FileMetaDataDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int OwnerId { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadDate { get; set; }
        public Visibility Visibility { get; set; } // Public / Private
    }
    public enum Visibility
    {
        Private,
        Public,
        Shared
    }
}
