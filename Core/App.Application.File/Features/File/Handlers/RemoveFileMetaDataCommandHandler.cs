using App.Application.Features.File.Commands;
using App.Application.Interfaces.File;
using MediatR;

namespace App.Application.Features.File.Handlers
{
    public class RemoveFileMetaDataCommandHandler : IRequestHandler<RemoveFileMetaDataCommand, bool>
    {
        private readonly IFileRepository _fileRepository;

        public RemoveFileMetaDataCommandHandler(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public async Task<bool> Handle(RemoveFileMetaDataCommand request, CancellationToken cancellationToken)
        {
            var file = await _fileRepository.GetByIdAsync(request.Id, includeShares: false);
            if (file == null)
                return false;

            await _fileRepository.DeleteAsync(file);
            return true;
        }
    }
}