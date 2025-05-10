using App.Application.Features.File.Queries;
using App.Application.Features.File.Results;
using App.Application.Interfaces.File;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Features.File.Handlers
{
    public class GetFileMetaDataByIdQueryHandler : IRequestHandler<GetFileMetaDataByIdQuery, GetFileMetaDataByIdQueryResult?>
    {
        private readonly IFileRepository _fileRepository;

        public GetFileMetaDataByIdQueryHandler(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public async Task<GetFileMetaDataByIdQueryResult?> Handle(GetFileMetaDataByIdQuery request, CancellationToken cancellationToken)
        {
            var file = await _fileRepository.GetByIdAsync(request.Id);
            if (file == null)
            {
                return null;
            }
            return new GetFileMetaDataByIdQueryResult
            {
                Id = file.Id,
                Name = file.Name,
                Description = file.Description,
                Visibility = file.Visibility,
                UploadDate = file.UploadDate,
                OwnerId = file.OwnerId
            };
        }
    }
}
