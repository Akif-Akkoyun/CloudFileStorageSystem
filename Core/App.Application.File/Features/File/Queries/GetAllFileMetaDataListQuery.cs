using App.Application.Features.File.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Features.File.Queries
{
    public class GetAllFileMetaDataListQuery : IRequest<List<GetAllFileMetaDataListQueryResult>>
    {
        public int OwnerId { get; set; }

        public GetAllFileMetaDataListQuery(int ownerId)
        {
            OwnerId = ownerId;
        }
    }
}
