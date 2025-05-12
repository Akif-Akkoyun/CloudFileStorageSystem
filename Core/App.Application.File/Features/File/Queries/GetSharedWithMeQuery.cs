using App.Application.File.Dtos;
using App.Application.File.Features.File.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.File.Features.File.Queries
{
    public class GetSharedWithMeQuery :IRequest<List<SharedWithMeDto>>
    {
        public int Id { get; set; }

        public GetSharedWithMeQuery(int id)
        {
            Id = id;
        }
    }
}
