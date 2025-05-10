using App.Application.File.Features.File.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.File.Features.File.Queries
{
    public class GetUsersWithFileSharedQuery :IRequest<List<GetUsersWithFileSharedQueryResult>>
    {
        public int Id { get; set; }

        public GetUsersWithFileSharedQuery(int id)
        {
            Id = id;
        }
    }
}
