using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.File.Features.File.Results
{
    public class GetFileSharesByFileIdQueryResult
    {
        public int UserId { get; set; }
        public string Permission { get; set; } = string.Empty;
    }
}
