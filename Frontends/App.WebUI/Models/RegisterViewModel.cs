using System.ComponentModel.DataAnnotations;

namespace App.WebUI.Models
{
    public class RegisterViewModel
    {
        [Required, MaxLength(50)]
        public string Name { get; set; } = default!;
        [Required, MaxLength(50)]
        public string SurName { get; set; } = default!;
        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; } = default!;
        [Required, MinLength(4), DataType(DataType.Password)]
        public string Password { get; set; } = default!;
        [Required, MinLength(4), DataType(DataType.Password), Compare(nameof(Password))]
        public string PasswordConfirm { get; set; } = default!;
    }
}
