using App.Application.Features.File.Commands;
using App.Application.Interfaces.File;
using App.Domain.Entities;
using App.Domain.Enums;
using MediatR;

namespace App.Application.Features.File.Handlers
{
    public class CreateFileMetaDataCommandHandler : IRequestHandler<CreateFileMetaDataCommand, int>
    {
        private readonly IFileRepository _fileRepository;
        private readonly IFileShareRepository _fileShareRepository;

        public CreateFileMetaDataCommandHandler(IFileRepository fileRepository, IFileShareRepository fileShareRepository)
        {
            _fileRepository = fileRepository;
            _fileShareRepository = fileShareRepository;
        }

        public async Task<int> Handle(CreateFileMetaDataCommand request, CancellationToken cancellationToken)
        {
            var dto = request.File;

            var file = new FileEntity
            {
                FileName = dto.FileName,
                Description = dto.Description,
                OwnerId = dto.OwnerId,
                UploadDate = DateTime.UtcNow,
                Visibility = dto.Visibility,
                FilePath = dto.FilePath,
            };

            var fileId = await _fileRepository.CreateAsync(file);

            if (dto.Visibility == FileVisibility.Shared && dto.SharedUserIds?.Any() == true)
            {
                foreach (var userId in dto.SharedUserIds)
                {
                    await _fileShareRepository.CreateAsync(new FileShareEntity
                    {
                        FileId = fileId,
                        UserId = userId,
                        Permission = "Read"
                    });
                }
            }

            return fileId;
        }
    }
}
