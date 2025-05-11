using App.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.File.Features.File.Results
{
    public class GetSharedWithMeQueryResult
    {
        public int Id { get; set; }
        public string FileName { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string FilePath { get; set; } = default!;
        public FileVisibility Visibility { get; set; }
        public DateTime UploadDate { get; set; }
    }
}
