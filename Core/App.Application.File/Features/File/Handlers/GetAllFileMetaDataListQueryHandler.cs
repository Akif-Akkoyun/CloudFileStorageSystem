using App.Application.Features.File.Queries;
using App.Application.Features.File.Results;
using App.Application.Interfaces.File;
using MediatR;
namespace App.Application.Features.File.Handlers
{
    public class GetAllFileMetaDataListQueryHandler : IRequestHandler<GetAllFileMetaDataListQuery, List<GetAllFileMetaDataListQueryResult>>
    {
        private readonly IFileRepository _fileRepository;
        public GetAllFileMetaDataListQueryHandler(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }
        public async Task<List<GetAllFileMetaDataListQueryResult>> Handle(GetAllFileMetaDataListQuery request, CancellationToken cancellationToken)
        {
            var files = await _fileRepository.GetAllAsync(request.OwnerId);

            return files.Select(f => new GetAllFileMetaDataListQueryResult
            {
                Id = f.Id,
                FileName = f.FileName,
                Description = f.Description,
                UploadDate = f.UploadDate,
                Visibility = f.Visibility,
                FilePath = f.FilePath
            }).ToList();
        }
    }
}