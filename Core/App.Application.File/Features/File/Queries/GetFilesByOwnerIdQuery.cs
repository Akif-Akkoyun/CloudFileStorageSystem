using App.Application.File.Features.File.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.File.Features.File.Queries
{
    public class GetFilesByOwnerIdQuery :IRequest<List<GetFilesByOwnerIdQueryResult>>
    {
        public int OwnerId { get; set; }

        public GetFilesByOwnerIdQuery(int ownerId)
        {
            OwnerId = ownerId;
        }
    }
}
