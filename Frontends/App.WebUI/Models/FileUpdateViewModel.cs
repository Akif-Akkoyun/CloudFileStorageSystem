using System.ComponentModel.DataAnnotations;

namespace App.WebUI.Models
{
    public class FileUpdateViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Dosya adı gereklidir.")]
        public string FileName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public IFormFile? NewFile { get; set; }

        [Required]
        public string Visibility { get; set; } = "Public";

        public string FilePath { get; set; } = string.Empty;
    }
}
