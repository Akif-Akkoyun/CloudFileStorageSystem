using App.Application.Features.File.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Features.File.Queries
{
    public class GetFileMetaDataByIdQuery : IRequest<GetFileMetaDataByIdQueryResult>
    {
        public int Id { get; set; }

        public GetFileMetaDataByIdQuery(int id)
        {
            Id = id;
        }
    }
}
