using System.ComponentModel.DataAnnotations;

namespace App.WebUI.Models
{
    public class LoginViewModel
    {
        [Required, EmailAddress, MaxLength(150)]
        public string Email { get; set; } = default!;
        [Required, DataType(DataType.Password), MinLength(4)]
        public string PasswordHash { get; set; } = default!;
    }
}
