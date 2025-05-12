using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Dto.FileDtos
{
    public class ShareFileDto
    {
        public int FileId { get; set; }
        public List<int> SharedWithUserIds { get; set; } = new();
        public string Permission { get; set; } = "ReadOnly";
    }
}
