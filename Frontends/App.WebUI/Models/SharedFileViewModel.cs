namespace App.WebUI.Models
{
    public class SharedWithMeViewModel
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Permission { get; set; } = "ReadOnly";
        public DateTime UploadDate { get; set; }
        public int OwnerId { get; set; }
        public string OwnerName { get; set; } = string.Empty;
    }
}
