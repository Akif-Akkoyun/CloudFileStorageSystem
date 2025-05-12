using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Dto.FileDtos
{
    public class GetSharesByFileIdDto
    {
        public int UserId { get; set; }
        public string Permission { get; set; } = "";
    }
}
