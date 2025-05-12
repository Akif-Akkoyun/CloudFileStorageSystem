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
    public class GetFilesByOwnerIdQueryHandler : IRequestHandler<GetFilesByOwnerIdQuery, List<GetFilesByOwnerIdQueryResult>>
    {
        private readonly IFileRepository _fileRepository;

        public GetFilesByOwnerIdQueryHandler(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public async Task<List<GetFilesByOwnerIdQueryResult>> Handle(GetFilesByOwnerIdQuery request, CancellationToken cancellationToken)
        {
            var files = await _fileRepository.GetByFilterAsync(f => f.OwnerId == request.OwnerId);
            return files.Select(f => new GetFilesByOwnerIdQueryResult
            {
                Id = f.Id,
                FileName = f.FileName,
                Description = f.Description,
                FilePath = f.FilePath,
                UploadDate = f.UploadDate,
                Visibility = f.Visibility,
            }).ToList();
        }
    }
}
