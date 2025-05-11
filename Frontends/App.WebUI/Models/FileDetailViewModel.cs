using System.ComponentModel.DataAnnotations;

namespace App.WebUI.Models
{
    public class FileDetailViewModel
    {
        [Display(Name = "Dosya ID")]
        public int Id { get; set; }

        [Display(Name = "Dosya Adı")]
        public string FileName { get; set; } = default!;

        [Display(Name = "Açıklama")]
        public string Description { get; set; } = default!;

        [Display(Name = "Dosya Yolu")]
        public string FilePath { get; set; } = default!;

        [Display(Name = "Yüklenme Tarihi")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime UploadDate { get; set; }

        [Display(Name = "Görünürlük")]
        public Visibility Visibility { get; set; }

        [Display(Name = "Yükleyen Kullanıcı")]
        public string OwnerName { get; set; } = default!;
    }
}
