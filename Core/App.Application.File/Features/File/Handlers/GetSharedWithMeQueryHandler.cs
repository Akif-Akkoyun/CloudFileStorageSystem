using App.Application.File.Features.File.Queries;
using App.Application.File.Features.File.Results;
using App.Application.Interfaces.File;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.File.Features.File.Handlers
{
    public class GetSharedWithMeQueryHandler : IRequestHandler<GetSharedWithMeQuery, List<GetSharedWithMeQueryResult>>
    {
        private readonly IFileShareRepository _fileShareRepository;

        public GetSharedWithMeQueryHandler(IFileShareRepository fileShareRepository)
        {
            _fileShareRepository = fileShareRepository;
        }

        public async Task<List<GetSharedWithMeQueryResult>> Handle(GetSharedWithMeQuery request, CancellationToken cancellationToken)
        {
            var result = await _fileShareRepository.GetFilesSharedWithUserAsync(request.Id);
            return result.Select(x => new GetSharedWithMeQueryResult
            {
                Id = x.Id,
                FileName = x.FileName,
                Description = x.Description,
                Visibility = x.Visibility,
                UploadDate = x.UploadDate,
                FilePath = x.FilePath,
            }).ToList();
        }
    }
}