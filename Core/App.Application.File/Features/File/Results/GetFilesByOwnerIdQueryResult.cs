using App.Domain.Enums;

namespace App.Application.File.Features.File.Results
{
    public class GetFilesByOwnerIdQueryResult
    {
        public int Id { get; set; }
        public string FileName { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string FilePath { get; set; } = default!;
        public DateTime UploadDate { get; set; }
        public FileVisibility Visibility { get; set; }
    }
}
