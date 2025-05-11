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
    public class GetPublicFileQueryHandler : IRequestHandler<GetPublicFilesQuery, List<GetPublicFilesQueryResult>>
    {
        private readonly IFileRepository _fileRepository;

        public GetPublicFileQueryHandler(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public async Task<List<GetPublicFilesQueryResult>> Handle(GetPublicFilesQuery request, CancellationToken cancellationToken)
        {
            var files =await _fileRepository.GetByFilterAsync(f => f.Visibility == Domain.Enums.FileVisibility.Public);
            return files.Select(f => new GetPublicFilesQueryResult
            {
                Id = f.Id,
                FileName = f.FileName,
                Description = f.Description,
                Visibility = f.Visibility,
                UploadDate = f.UploadDate,
                FilePath = f.FilePath,
            }).ToList();
        }
    }
}
