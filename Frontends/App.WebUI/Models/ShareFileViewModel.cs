using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace App.WebUI.Models
{
    public class ShareFileViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Kullanıcılar")]
        public List<int> SelectedUserIds { get; set; } = new();

        [Display(Name = "Paylaşım Yetkisi")]
        public string Permission { get; set; } = "ReadOnly";

        public List<SelectListItem> AllUsers { get; set; } = new();
    }
}
