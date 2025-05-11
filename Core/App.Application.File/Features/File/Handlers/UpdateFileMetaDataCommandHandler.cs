using App.Application.Features.File.Commands;
using App.Application.Interfaces.File;
using App.Domain.Entities;
using App.Domain.Enums;
using MediatR;
namespace App.Application.Features.File.Handlers
{
    public class UpdateFileMetaDataCommandHandler : IRequestHandler<UpdateFileMetaDataCommand, bool>
    {
        private readonly IFileRepository _fileRepository;
        private readonly IFileShareRepository _fileShareRepository;
        public UpdateFileMetaDataCommandHandler(IFileRepository fileRepository, IFileShareRepository fileShareRepository)
        {
            _fileRepository = fileRepository;
            _fileShareRepository = fileShareRepository;
        }
        public async Task<bool> Handle(UpdateFileMetaDataCommand request, CancellationToken cancellationToken)
        {
            var dto = request.File;

            var existing = await _fileRepository.GetByIdAsync(dto.Id, includeShares: true);
            if (existing == null)
                return false;
            existing.FileName = dto.FileName;
            existing.Description = dto.Description;
            existing.Visibility = dto.Visibility;
            existing.UploadDate = DateTime.UtcNow;
            existing.FilePath = dto.FilePath;

            await _fileRepository.UpdateAsync(existing);

            if (dto.Visibility == FileVisibility.Shared)
            {
                await _fileShareRepository.DeleteByFileIdAsync(dto.Id);
                if (dto.SharedUserIds?.Any() == true)
                {
                    foreach (var userId in dto.SharedUserIds)
                    {
                        await _fileShareRepository.CreateAsync(new FileShareEntity
                        {
                            FileId = dto.Id,
                            UserId = userId,
                            Permission = "Read"
                        });
                    }
                }
            }
            else
            {
                await _fileShareRepository.DeleteByFileIdAsync(dto.Id);
            }
            return true;
        }
    }
}