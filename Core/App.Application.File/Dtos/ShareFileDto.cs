using App.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.File.Dtos
{
    public class ShareFileDto
    {
        public int FileId { get; set; }
        public List<int> SharedWithUserIds { get; set; } = new();
        public FilePermission Permission { get; set; } = FilePermission.ReadOnly;
    }
}
