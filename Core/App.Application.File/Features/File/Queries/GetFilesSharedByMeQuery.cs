using App.Application.File.Features.File.Results;
using Ardalis.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.File.Features.File.Queries
{
    public class GetFilesSharedByMeQuery : IRequest<Result<List<GetFilesSharedByMeQueryResult>>>
    {
        public int UserId { get; set; }

        public GetFilesSharedByMeQuery(int userId)
        {
            UserId = userId;
        }
    }
}
