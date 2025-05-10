using App.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Features.File.Results
{
    public class GetFileMetaDataByIdQueryResult
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public FileVisibility Visibility { get; set; }
        public DateTime UploadDate { get; set; }
        public int OwnerId { get; set; }
    }
}
