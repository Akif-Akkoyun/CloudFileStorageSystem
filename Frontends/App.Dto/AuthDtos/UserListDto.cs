using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Dto.AuthDtos
{
    public class UserListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string SurName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public string? RefreshPasswordToken { get; set; }
        public int RoleId { get; set; }
    }
}
