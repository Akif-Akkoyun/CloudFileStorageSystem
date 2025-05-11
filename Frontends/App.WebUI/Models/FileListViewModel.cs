namespace App.WebUI.Models
{
    public class FileListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int OwnerId { get; set; }
        public DateTime UploadDate { get; set; }
        public Visibility Visibility { get; set; }
    }
    public enum Visibility
    {
        Public,
        Private,
        Shared
    }
}
