using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.File.Features.File.Results
{
    public class GetFilesSharedByMeQueryResult
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string FilePath { get; set; } = default!;
        public string Visibility { get; set; } = default!;
        public List<int> SharedWithUsers { get; set; } = new();
    }
}
