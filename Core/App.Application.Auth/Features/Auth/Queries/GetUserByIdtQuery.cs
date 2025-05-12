using App.Application.Auth.Features.Auth.Results;
using MediatR;

namespace App.Application.Auth.Features.Auth.Queries
{
    public class GetUserByIdQuery : IRequest<GetUserByIdQueryResult>
    {
        public int Id { get; set; }

        public GetUserByIdQuery(int id)
        {
            Id = id;
        }
    }
}
