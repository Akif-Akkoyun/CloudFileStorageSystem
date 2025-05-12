using App.Application.File.Dtos;
using App.Application.File.Features.File.Queries;
using App.Application.Interfaces.File;
using MediatR;

namespace App.Application.File.Features.File.Handlers
{
    public class GetSharedWithMeQueryHandler : IRequestHandler<GetSharedWithMeQuery, List<SharedWithMeDto>>
    {
        private readonly IFileShareRepository _fileShareRepository;
        public GetSharedWithMeQueryHandler(IFileShareRepository fileShareRepository)
        {
            _fileShareRepository = fileShareRepository;
        }
        public async Task<List<SharedWithMeDto>> Handle(GetSharedWithMeQuery request, CancellationToken cancellationToken)
        {
            return await _fileShareRepository.GetSharedWithMeAsync(request.Id);
        }
    }
}