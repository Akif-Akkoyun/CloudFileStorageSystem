using App.Application.File.Features.File.Commands;
using App.Application.Interfaces.File;
using App.Domain.Enums;
using Ardalis.Result;
using MediatR;

namespace App.Application.File.Features.File.Handlers
{
    public class ToggleFileVisibilityCommandHandler : IRequestHandler<ToggleFileVisibilityCommand, Result>
    {
        private readonly IFileRepository _fileRepository;

        public ToggleFileVisibilityCommandHandler(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public async Task<Result> Handle(ToggleFileVisibilityCommand request, CancellationToken cancellationToken)
        {
            var file = await _fileRepository.GetByIdAsync(request.FileId);
            if (file == null)
            {
                return Result.NotFound();
            }

            file.Visibility = file.Visibility == FileVisibility.Public ? FileVisibility.Private : FileVisibility.Public;
            await _fileRepository.UpdateAsync(file);
            return Result.Success();
        }
    }
}