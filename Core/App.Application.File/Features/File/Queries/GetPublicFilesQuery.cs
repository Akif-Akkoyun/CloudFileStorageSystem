using App.Application.File.Features.File.Results;
using MediatR;

namespace App.Application.File.Features.File.Queries
{
    public class GetPublicFilesQuery :IRequest<List<GetPublicFilesQueryResult>>
    {
    }
}
