using App.Application.File.Features.File.Queries;
using App.Application.File.Features.File.Results;
using App.Application.Interfaces.File;
using Ardalis.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.File.Features.File.Handlers
{
    public class GetFilesSharedByMeQueryHandler : IRequestHandler<GetFilesSharedByMeQuery, Result<List<GetFilesSharedByMeQueryResult>>>
    {
        private readonly IFileRepository _fileRepository;

        public GetFilesSharedByMeQueryHandler(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public async Task<Result<List<GetFilesSharedByMeQueryResult>>> Handle(GetFilesSharedByMeQuery request, CancellationToken cancellationToken)
        {
            var files = await _fileRepository.GetByFilterAsync(f => f.OwnerId == request.UserId && f.FileShares.Any());

            if (files == null || !files.Any())
                return Result.NotFound("Paylaşılan herhangi bir dosya bulunamadı.");

            var result = files.Select(file => new GetFilesSharedByMeQueryResult
            {
                Id = file.Id,
                Name = file.FileName,
                Description = file.Description,
                FilePath = file.FilePath,
                Visibility = file.Visibility.ToString(),
                SharedWithUsers = file.FileShares.Select(fs => fs.UserId).ToList()
            }).ToList();

            return Result.Success(result);
        }
    }

}
