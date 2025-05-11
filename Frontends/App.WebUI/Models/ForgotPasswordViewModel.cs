using System.ComponentModel.DataAnnotations;

namespace App.WebUI.Models
{
    public class ForgotPasswordViewModel
    {

        [Required, MaxLength(256), EmailAddress]
        public string Email { get; set; } = null!;
    }
}
