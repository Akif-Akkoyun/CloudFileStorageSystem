using System.ComponentModel.DataAnnotations;

namespace App.WebUI.Models
{
    public class FileUploadViewModel
    {
        [Required(ErrorMessage = "Açıklama gereklidir.")]
        public string Description { get; set; } = default!;

        [Required(ErrorMessage = "Bir dosya seçmelisiniz.")]
        public IFormFile? File { get; set; }

        [Required(ErrorMessage = "Görünürlük seçiniz.")]
        public Visibility Visibility { get; set; } 
    }
}
