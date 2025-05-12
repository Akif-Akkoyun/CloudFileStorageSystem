using App.Application.File.Features.File.Queries;
using App.Application.File.Features.File.Results;
using App.Application.Interfaces.File;
using MediatR;

namespace App.Application.File.Features.File.Handlers
{
    public class GetFileSharesByFileIdQueryHandler : IRequestHandler<GetFileSharesByFileIdQuery, List<GetFileSharesByFileIdQueryResult>>
    {
        private readonly IFileShareRepository _fileShareRepository;

        public GetFileSharesByFileIdQueryHandler(IFileShareRepository fileShareRepository)
        {
            _fileShareRepository = fileShareRepository;
        }

        public async Task<List<GetFileSharesByFileIdQueryResult>> Handle(GetFileSharesByFileIdQuery request, CancellationToken cancellationToken)
        {
            var shares = await _fileShareRepository.GetByFileIdAsync(request.FileId);

            return shares.Select(s => new GetFileSharesByFileIdQueryResult
            {
                UserId = s.UserId,
                Permission = s.Permission
            }).ToList();
        }
    }

}
