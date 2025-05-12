using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Auth.Features.Auth.Results
{
    public class GetAllUsersQueryResult
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
    }
}
