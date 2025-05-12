using App.Application.File.Features.File.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.File.Features.File.Queries
{
    public class GetFileSharesByFileIdQuery : IRequest<List<GetFileSharesByFileIdQueryResult>>
    {
        public int FileId { get; }

        public GetFileSharesByFileIdQuery(int fileId)
        {
            FileId = fileId;
        }
    }
}
