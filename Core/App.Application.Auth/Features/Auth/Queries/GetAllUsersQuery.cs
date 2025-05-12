using App.Application.Auth.Features.Auth.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Auth.Features.Auth.Queries
{
    public class GetAllUsersQuery : IRequest<List<GetAllUsersQueryResult>>
    {
    }
}
